using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using RtMidi.Core.Devices;
using RtMidi.Core.Devices.Nrpn;
using RtMidi.Core.Messages;
using Serilog;

namespace LrControl.Core.Midi
{
    public delegate void EventInvocator<T>(in T msg);
    
    internal class InputDeviceDecorator : IMidiInputDevice
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<InputDeviceDecorator>();

        private readonly ConcurrentDictionary<ControlChangeKey, ControlChangeMessageHolder> _controlChangeMessages;
        private readonly IMidiInputDevice _inputDevice;
        private readonly ConcurrentDictionary<NrpnKey, NrpnMessageHolder> _nrpnMessages;
        private readonly ManualResetEvent _stopTimerEvent = new ManualResetEvent(false);
        private bool _disposed;
        private Thread _timerThread;
        private int _updateInterval;

        public InputDeviceDecorator(IMidiInputDevice inputDevice, int updateInterval)
        {
            _inputDevice = inputDevice;
            _controlChangeMessages = new ConcurrentDictionary<ControlChangeKey, ControlChangeMessageHolder>();
            _nrpnMessages = new ConcurrentDictionary<NrpnKey, NrpnMessageHolder>();
            UpdateInterval = updateInterval;

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
            
        }

        private void InputDeviceOnPitchBend(IMidiInputDevice sender, in PitchBendMessage msg)
        {
            throw new NotImplementedException();
        }

        private void InputDeviceOnChannelPressure(IMidiInputDevice sender, in ChannelPressureMessage msg)
        {
            throw new NotImplementedException();
        }

        private void InputDeviceOnProgramChange(IMidiInputDevice sender, in ProgramChangeMessage msg)
        {
            throw new NotImplementedException();
        }

        private void InputDeviceOnPolyphonicKeyPressure(IMidiInputDevice sender, in PolyphonicKeyPressureMessage msg)
        {
            throw new NotImplementedException();
        }

        private void InputDeviceOnNoteOn(IMidiInputDevice sender, in NoteOnMessage msg)
        {
            throw new NotImplementedException();
        }

        private void InputDeviceOnNoteOff(IMidiInputDevice sender, in NoteOffMessage msg)
        {
            throw new NotImplementedException();
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

            _inputDevice.Nrpn -= InputDeviceOnNrpn;
            _inputDevice.ControlChange -= InputDeviceOnControlChange;
            
            _inputDevice.Dispose();

            _disposed = true;
        }

        private void TimerThreadStart()
        {
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                if (!TrySendOldest(_controlChangeMessages.Values, 
                    (in ControlChangeMessage msg) => ControlChange?.Invoke(this, in msg)))
                {
                    TrySendOldest(_nrpnMessages.Values, (in NrpnMessage msg) => Nrpn?.Invoke(this, in msg));
                }

                // Sleep
                var remainingTicks = (int) (UpdateInterval - stopwatch.ElapsedMilliseconds);
                if (_stopTimerEvent.WaitOne(Math.Max(0, remainingTicks)))
                {
                    break;
                }
            }
        }

        private static bool TrySendOldest<TMessage>(IEnumerable<MessageHolder<TMessage>> messageHolders,
            EventInvocator<TMessage> eventInvocator) where TMessage : struct
        {
            var holder = FindOldest(messageHolders);
            if (holder == null) return false;

            ref readonly var message = ref holder.Message;
            eventInvocator(in message);
            holder.SetLastSent(in message);
            return true;
        }

        private static MessageHolder<TMessage> FindOldest<TMessage>(
            IEnumerable<MessageHolder<TMessage>> messageHolders) where TMessage : struct
        {
            MessageHolder<TMessage> oldest = null;

            foreach (var holder in messageHolders)
            {
                if (!holder.HasChanged) continue;

                if (oldest == null)
                {
                    oldest = holder;
                }
                else if(holder.LastSentTimestamp <= oldest.LastSentTimestamp)
                {
                    oldest = holder;
                }
            }

            return oldest;
        }

        private void InputDeviceOnNrpn(IMidiInputDevice sender, in NrpnMessage msg)
        {
            var key = new NrpnKey(in msg);

            if (!_nrpnMessages.TryGetValue(key, out var holder))
            {
                holder = new NrpnMessageHolder(in msg);
                if (!_nrpnMessages.TryAdd(key, holder))
                {
                    holder = _nrpnMessages[key];
                }
            }

            holder.SetMessage(in msg);
        }

        private void InputDeviceOnControlChange(IMidiInputDevice sender, in ControlChangeMessage msg)
        {
            var key = new ControlChangeKey(in msg);

            if (!_controlChangeMessages.TryGetValue(key, out var holder))
            {
                holder = new ControlChangeMessageHolder(in msg);
                if (!_controlChangeMessages.TryAdd(key, holder))
                {
                    holder = _controlChangeMessages[key];
                }
            }

            holder.SetMessage(in msg);
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

        #endregion
    }
}