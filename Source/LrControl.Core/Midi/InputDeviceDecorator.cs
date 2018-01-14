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

            _inputDevice.Nrpn += InputDeviceOnNrpn;
            _inputDevice.ControlChange += InputDeviceOnControlChange;
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

                if (!TrySendOldest(_controlChangeMessages.Values, msg => ControlChange?.Invoke(this, msg)))
                {
                    TrySendOldest(_nrpnMessages.Values, msg => Nrpn?.Invoke(this, msg));
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
            Action<TMessage> eventInvocator)
        {
            var holder = FindOldestMessage(messageHolders);
            if (holder == null) return false;

            var message = holder.Message;
            eventInvocator(message);
            holder.SetLastSent(message);
            return true;
        }

        private static MessageHolder<TMessage> FindOldestMessage<TMessage>(
            IEnumerable<MessageHolder<TMessage>> messageHolders)
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
            var nrpnMsg = msg;
            _nrpnMessages.AddOrUpdate(new NrpnKey(msg),
                key => new NrpnMessageHolder(nrpnMsg),
                (key, holder) =>
                {
                    holder.SetMessage(nrpnMsg);
                    return holder;
                });
        }

        private void InputDeviceOnControlChange(IMidiInputDevice sender, in ControlChangeMessage msg)
        {
            var ccMsg = msg;
            _controlChangeMessages.AddOrUpdate(new ControlChangeKey(msg),
                key => new ControlChangeMessageHolder(ccMsg),
                (key, holder) =>
                {
                    holder.SetMessage(ccMsg);
                    return holder;
                });
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