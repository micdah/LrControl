using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using LrControl.Core.Configurations;
using LrControl.Core.Midi.Messages;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Nrpn;
using RtMidi.Core.Messages;
using Serilog;

namespace LrControl.Core.Midi
{
    internal class InputDeviceDecorator : IMidiInputDevice, IMidiInputDeviceEventDispatcher
    {
        private delegate MessageHolder<TMessage> MessageHolderFactory<TMessage>(in TMessage msg)
            where TMessage : struct;

        private static readonly ILogger Log = Serilog.Log.ForContext<InputDeviceDecorator>();
        private readonly IMidiInputDevice _inputDevice;
        private readonly ISettings _settings;
        private readonly ConcurrentDictionary<string, MessageHolder> _holders;
        private readonly ManualResetEvent _stopTimerEvent = new ManualResetEvent(false);
        private bool _disposed;
        private Thread _timerThread;
        private int _updateInterval;

        public InputDeviceDecorator(IMidiInputDevice inputDevice, ISettings settings)
        {
            _inputDevice = inputDevice;
            _settings = settings;
            _holders = new ConcurrentDictionary<string, MessageHolder>();
            UpdateInterval = _settings.ParameterUpdateFrequency / 1000;
            _settings.PropertyChanged += SettingsOnPropertyChanged;

            // Start timer thread
            _timerThread = new Thread(TimerThreadStart)
            {
                IsBackground = true,
                Name = "InputDeviceDecorator Timer Thread"
            };
            _timerThread.Start();

            _inputDevice.NoteOff += InputDeviceOnNoteOff;
            _inputDevice.NoteOn += InputDeviceOnNoteOn;
            _inputDevice.PolyphonicKeyPressure += InputDeviceOnPolyphonicKeyPressure;
            _inputDevice.ControlChange += InputDeviceOnControlChange;
            _inputDevice.ProgramChange += InputDeviceOnProgramChange;
            _inputDevice.ChannelPressure += InputDeviceOnChannelPressure;
            _inputDevice.PitchBend += InputDeviceOnPitchBend;
            _inputDevice.Nrpn += InputDeviceOnNrpn;
            _inputDevice.SysEx += InputDeviceOnSysEx;
        }

        public int UpdateInterval
        {
            private get { return _updateInterval; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(UpdateInterval));

                _updateInterval = value;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (_timerThread != null)
            {
                _stopTimerEvent.Set();
                _timerThread = null;
            }

            try
            {
                if (_inputDevice.IsOpen)
                    _inputDevice.Close();
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception while trying to stop and close input device");
            }
            
            _inputDevice.NoteOff -= InputDeviceOnNoteOff;
            _inputDevice.NoteOn -= InputDeviceOnNoteOn;
            _inputDevice.PolyphonicKeyPressure -= InputDeviceOnPolyphonicKeyPressure;
            _inputDevice.ControlChange -= InputDeviceOnControlChange;
            _inputDevice.ProgramChange -= InputDeviceOnProgramChange;
            _inputDevice.ChannelPressure -= InputDeviceOnChannelPressure;
            _inputDevice.PitchBend -= InputDeviceOnPitchBend;
            _inputDevice.Nrpn -= InputDeviceOnNrpn;

            _inputDevice.Dispose();

            _settings.PropertyChanged -= SettingsOnPropertyChanged;

            _disposed = true;
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_disposed) return;
            if (e.PropertyName != nameof(ISettings.ParameterUpdateFrequency)) return;
            
            if (_settings.ParameterUpdateFrequency < 0)
                throw new ArgumentOutOfRangeException(nameof(ISettings.ParameterUpdateFrequency));

            _updateInterval = _settings.ParameterUpdateFrequency / 1000;
        }

        private void TimerThreadStart()
        {
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                // Find and send oldest message
                FindOldest()?.SendMessage(this);

                // Sleep
                var remainingTicks = (int) (UpdateInterval - stopwatch.ElapsedMilliseconds);
                if (_stopTimerEvent.WaitOne(Math.Max(0, remainingTicks)))
                {
                    break;
                }
            }
        }
        
        
        
        private MessageHolder FindOldest()
        {
            MessageHolder oldest = null;

            foreach (var holder in _holders.Values)
            {
                if (!holder.HasChanged) continue;

                if (oldest == null)
                {
                    oldest = holder;
                }
                else if (holder.LastSentTimestamp <= oldest.LastSentTimestamp)
                {
                    oldest = holder;
                }
            }

            return oldest;
        }

        private void InputDeviceOnNoteOff(IMidiInputDevice sender, in NoteOffMessage msg)
        {
            OnMessageHandler(in msg, (in NoteOffMessage message) => new NoteOffMessageHolder(in message));
        }

        private void InputDeviceOnNoteOn(IMidiInputDevice sender, in NoteOnMessage msg)
        {
            OnMessageHandler(in msg, (in NoteOnMessage message) => new NoteOnMessageHolder(in message));
        }

        private void InputDeviceOnPolyphonicKeyPressure(IMidiInputDevice sender, in PolyphonicKeyPressureMessage msg)
        {
            OnMessageHandler(in msg,
                (in PolyphonicKeyPressureMessage message) => new PolyphonicKeyPressureMessageHolder(message));
        }

        private void InputDeviceOnControlChange(IMidiInputDevice sender, in ControlChangeMessage msg)
        {
            OnMessageHandler(in msg, (in ControlChangeMessage message) => new ControlChangeMessageHolder(in message));
        }

        private void InputDeviceOnProgramChange(IMidiInputDevice sender, in ProgramChangeMessage msg)
        {
            OnMessageHandler(in msg, (in ProgramChangeMessage message) => new ProgramChangeMessageHolder(in message));
        }

        private void InputDeviceOnChannelPressure(IMidiInputDevice sender, in ChannelPressureMessage msg)
        {
            OnMessageHandler(in msg,
                (in ChannelPressureMessage message) => new ChannelPressureMessageHolder(in message));
        }

        private void InputDeviceOnPitchBend(IMidiInputDevice sender, in PitchBendMessage msg)
        {
            OnMessageHandler(in msg, (in PitchBendMessage message) => new PitchBendMessageHolder(in message));
        }

        private void InputDeviceOnNrpn(IMidiInputDevice sender, in NrpnMessage msg)
        {
            OnMessageHandler(in msg, (in NrpnMessage message) => new NrpnMessageHolder(in message));
        }
        
        private void InputDeviceOnSysEx(IMidiInputDevice sender, in SysExMessage msg)
        {
            OnMessageHandler(in msg, (in SysExMessage message) => new SysExMessageHolder(in message));
        }
        
        private void OnMessageHandler<TMessage>(in TMessage msg, MessageHolderFactory<TMessage> factory)
            where TMessage : struct
        {
            var key = GetMessageKey(in msg);

            // Fetch or create message holder for specific key
            MessageHolder<TMessage> typedHolder;
            if (_holders.TryGetValue(key, out var holder))
            {
                // Use existing holder for message key
                typedHolder = (MessageHolder<TMessage>) holder;
            }
            else
            {
                // Try register new holder for message key
                typedHolder = factory(in msg);
                if (!_holders.TryAdd(key, typedHolder))
                {
                    // Holder registered in the mean-time, must exist now
                    typedHolder = (MessageHolder<TMessage>) _holders[key];
                }
            }

            typedHolder.SetMessage(in msg);
        }

        private static string GetMessageKey<TMessage>(in TMessage msg) where TMessage : struct
        {
            switch (msg)
            {
                case NoteOffMessage message:
                    return $"{nameof(NoteOffMessage)}_{message.Channel}_{message.Key}";
                case NoteOnMessage message:
                    return $"{nameof(NoteOnMessage)}_{message.Channel}_{message.Key}";
                case PolyphonicKeyPressureMessage message:
                    return $"{nameof(PolyphonicKeyPressureMessage)}_{message.Channel}_{message.Key}";
                case ControlChangeMessage message:
                    return $"{nameof(ControlChangeMessage)}_{message.Channel}_{message.Control}";
                case ProgramChangeMessage message:
                    return $"{nameof(ProgramChangeMessage)}_{message.Channel}";
                case ChannelPressureMessage message:
                    return $"{nameof(ChannelPressureMessage)}_{message.Channel}";
                case PitchBendMessage message:
                    return $"{nameof(PitchBendMessage)}_{message.Channel}";
                case NrpnMessage message:
                    return $"{nameof(NrpnMessage)}_{message.Channel}_{message.Parameter}";

                default:
                    throw new InvalidOperationException($"Unknown message type {msg}");
            }
        }

        #region Delegated members

        public bool Open() => _inputDevice.Open();
        public void Close() => _inputDevice.Close();
        public bool IsOpen => _inputDevice.IsOpen;
        public string Name => _inputDevice.Name;
        public void SetNrpnMode(NrpnMode mode) => _inputDevice.SetNrpnMode(mode);

        public event NoteOffMessageHandler NoteOff;
        public event NoteOnMessageHandler NoteOn;
        public event PolyphonicKeyPressureMessageHandler PolyphonicKeyPressure;
        public event ControlChangeMessageHandler ControlChange;
        public event ProgramChangeMessageHandler ProgramChange;
        public event ChannelPressureMessageHandler ChannelPressure;
        public event PitchBendMessageHandler PitchBend;
        public event NrpnMessageHandler Nrpn;
        public event SysExMessageHandler SysEx;

        #endregion

        #region Event dispatchers

        public void OnNoteOff(in NoteOffMessage msg)
        {
            NoteOff?.Invoke(this, in msg);
        }

        public void OnNoteOn(in NoteOnMessage msg)
        {
            NoteOn?.Invoke(this, in msg);
        }

        public void OnPolyphonicKeyPressure(in PolyphonicKeyPressureMessage msg)
        {
            PolyphonicKeyPressure?.Invoke(this, in msg);
        }

        public void OnControlChange(in ControlChangeMessage msg)
        {
            ControlChange?.Invoke(this, in msg);
        }

        public void OnProgramChange(in ProgramChangeMessage msg)
        {
            ProgramChange?.Invoke(this, in msg);
        }

        public void OnChannelPressure(in ChannelPressureMessage msg)
        {
            ChannelPressure?.Invoke(this, in msg);
        }

        public void OnPitchBend(in PitchBendMessage msg)
        {
            PitchBend?.Invoke(this, in msg);
        }

        public void OnNrpn(in NrpnMessage msg)
        {
            Nrpn?.Invoke(this, in msg);
        }

        public void OnSysEx(in SysExMessage msg)
        {
            SysEx?.Invoke(this, in msg);
        }

        #endregion
    }
}