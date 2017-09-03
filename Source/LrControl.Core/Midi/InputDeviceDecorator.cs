using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Midi.Devices;
using Midi.Messages;
using Serilog;

namespace LrControl.Core.Midi
{
    internal class InputDeviceDecorator : IInputDevice, IDisposable
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<InputDeviceDecorator>();

        private readonly ConcurrentDictionary<ControlChangeKey, ControlChangeMessageHolder> _controlChangeMessages;
        private readonly IInputDevice _inputDevice;
        private readonly ConcurrentDictionary<NrpnKey, NrpnMessageHolder> _nrpnMessages;
        private readonly ManualResetEvent _stopTimerEvent = new ManualResetEvent(false);
        private bool _disposed;
        private Thread _timerThread;
        private int _updateInterval;

        public InputDeviceDecorator(IInputDevice inputDevice, int updateInterval)
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

            if (_inputDevice.IsReceiving)
                _inputDevice.StopReceiving();

            if (_inputDevice.IsOpen)
                _inputDevice.Close();

            _inputDevice.Nrpn -= InputDeviceOnNrpn;
            _inputDevice.ControlChange -= InputDeviceOnControlChange;

            _disposed = true;
        }

        private void TimerThreadStart()
        {
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                if (!TrySendOldest(_controlChangeMessages.Values, msg => ControlChange?.Invoke(msg)))
                {
                    TrySendOldest(_nrpnMessages.Values, msg => Nrpn?.Invoke(msg));
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
            where TMessage : class
        {
            var holder = FindOldestMessage(messageHolders);
            if (holder == null) return false;

            var message = holder.Message;
            eventInvocator(message);
            holder.SetLastSent(message);
            return true;
        }

        private static MessageHolder<TMessage> FindOldestMessage<TMessage>(
            IEnumerable<MessageHolder<TMessage>> messageHolders) where TMessage : class
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

        private void InputDeviceOnNrpn(NrpnMessage msg)
        {
            _nrpnMessages.AddOrUpdate(new NrpnKey(msg),
                key => new NrpnMessageHolder(msg),
                (key, holder) =>
                {
                    holder.SetMessage(msg);
                    return holder;
                });
        }

        private void InputDeviceOnControlChange(ControlChangeMessage msg)
        {
            _controlChangeMessages.AddOrUpdate(new ControlChangeKey(msg),
                key => new ControlChangeMessageHolder(msg),
                (key, holder) =>
                {
                    holder.SetMessage(msg);
                    return holder;
                });
        }

        #region Delegated members

        public string Name => _inputDevice.Name;
        public bool IsOpen => _inputDevice.IsOpen;

        public bool IsReceiving => _inputDevice.IsReceiving;

        public event ControlChangeHandler ControlChange;

        public event ProgramChangeHandler ProgramChange
        {
            add => _inputDevice.ProgramChange += value;
            remove => _inputDevice.ProgramChange -= value;
        }

        public event NoteOnHandler NoteOn
        {
            add => _inputDevice.NoteOn += value;
            remove => _inputDevice.NoteOn -= value;
        }

        public event NoteOffHandler NoteOff
        {
            add => _inputDevice.NoteOff += value;
            remove => _inputDevice.NoteOff -= value;
        }

        public event PitchBendHandler PitchBend
        {
            add => _inputDevice.PitchBend += value;
            remove => _inputDevice.PitchBend -= value;
        }

        public event SysExHandler SysEx
        {
            add => _inputDevice.SysEx += value;
            remove => _inputDevice.SysEx -= value;
        }

        public event NrpnHandler Nrpn;


        public void RemoveAllEventHandlers()
        {
            _inputDevice.RemoveAllEventHandlers();
        }

        public void Open()
        {
            _inputDevice.Open();
        }

        public void Close()
        {
            _inputDevice.Close();
        }

        public void StartReceiving(Clock clock)
        {
            _inputDevice.StartReceiving(clock);
        }

        public void StartReceiving(Clock clock, bool handleSysEx)
        {
            _inputDevice.StartReceiving(clock, handleSysEx);
        }

        public void StopReceiving()
        {
            _inputDevice.StopReceiving();
        }

        #endregion
    }
}