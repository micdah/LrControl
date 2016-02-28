using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using micdah.LrControl.Configurations;
using Midi.Devices;
using Midi.Messages;

namespace micdah.LrControl.Core.Midi
{
    public class InputDeviceDecorator : IInputDevice, IDisposable
    {
        private readonly ConcurrentDictionary<ControlChangeKey, ControlChangeMessageHolder> _controlChangeMessages;
        private readonly IInputDevice _inputDevice;
        private readonly ConcurrentDictionary<NrpnKey, NrpnMessageHolder> _nrpnMessages;
        private readonly ManualResetEvent _stopTimerEvent = new ManualResetEvent(false);
        private Thread _timerThread;
        private Stopwatch _timestampStopwatch;
        private int _updateInterval;

        public InputDeviceDecorator(IInputDevice inputDevice)
        {
            _inputDevice = inputDevice;
            _controlChangeMessages = new ConcurrentDictionary<ControlChangeKey, ControlChangeMessageHolder>();
            _nrpnMessages = new ConcurrentDictionary<NrpnKey, NrpnMessageHolder>();

            // Start timestamp stopwatch
            _timestampStopwatch = new Stopwatch();
            _timestampStopwatch.Start();

            // Read and keep up-2-date with update interval
            UpdateTimerInterval();
            Settings.Current.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Settings.ParameterUpdateFrequency))
                {
                    UpdateTimerInterval();
                }
            };

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

        public void Dispose()
        {
            if (_timerThread != null)
            {
                _stopTimerEvent.Set();
                _timerThread = null;
            }
            if (_timestampStopwatch != null)
            {
                _timestampStopwatch.Stop();
                _timestampStopwatch = null;
            }
        }

        public string Name => _inputDevice.Name;
        public bool IsOpen => _inputDevice.IsOpen;

        public bool IsReceiving => _inputDevice.IsReceiving;

        public event ControlChangeHandler ControlChange;

        public event ProgramChangeHandler ProgramChange
        {
            add { _inputDevice.ProgramChange += value; }
            remove { _inputDevice.ProgramChange -= value; }
        }

        public event NoteOnHandler NoteOn
        {
            add { _inputDevice.NoteOn += value; }
            remove { _inputDevice.NoteOn -= value; }
        }

        public event NoteOffHandler NoteOff
        {
            add { _inputDevice.NoteOff += value; }
            remove { _inputDevice.NoteOff -= value; }
        }

        public event PitchBendHandler PitchBend
        {
            add { _inputDevice.PitchBend += value; }
            remove { _inputDevice.PitchBend -= value; }
        }

        public event SysExHandler SysEx
        {
            add { _inputDevice.SysEx += value; }
            remove { _inputDevice.SysEx -= value; }
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

        private void TimerThreadStart()
        {
            var stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();

                NrpnMessageHolder nrpnHolder;
                ControlChangeMessageHolder ccHolder;
                FindOldestMessage(out ccHolder, out nrpnHolder);

                // Send message, if any found
                if (ccHolder != null)
                {
                    var msg = ccHolder.Message;
                    OnControlChange(msg);
                    ccHolder.SetLastSent(msg, _timestampStopwatch);
                }
                else if (nrpnHolder != null)
                {
                    var msg = nrpnHolder.Message;
                    OnNrpn(msg);
                    nrpnHolder.SetLastSent(msg, _timestampStopwatch);
                }

                // Sleep
                var remainingTicks = (int) (_updateInterval - stopwatch.ElapsedMilliseconds);
                if (_stopTimerEvent.WaitOne(Math.Max(0, remainingTicks)))
                {
                    break;
                }
            }
        }

        private void FindOldestMessage(out ControlChangeMessageHolder ccHolder, out NrpnMessageHolder nrpnHolder)
        {
            var timeStamp = long.MaxValue;
            ccHolder = null;
            nrpnHolder = null;

            // Find oldest message to send
            foreach (var holder in _nrpnMessages.Values)
            {
                if (!holder.HasChanged) continue;
                if (holder.LastSentTimestamp >= timeStamp) continue;

                nrpnHolder = holder;
                timeStamp = holder.LastSentTimestamp;
            }
            foreach (var holder in _controlChangeMessages.Values)
            {
                if (!holder.HasChanged) continue;
                if (holder.LastSentTimestamp > timeStamp) continue;

                ccHolder = holder;
                nrpnHolder = null;
                timeStamp = holder.LastSentTimestamp;
            }
        }

        private void UpdateTimerInterval()
        {
            _updateInterval = 1000/Settings.Current.ParameterUpdateFrequency;
        }

        private void InputDeviceOnNrpn(NrpnMessage msg)
        {
            _nrpnMessages.AddOrUpdate(new NrpnKey(msg),
                key => new NrpnMessageHolder(msg, _timestampStopwatch),
                (key, holder) =>
                {
                    holder.SetMessage(msg, _timestampStopwatch);
                    return holder;
                });
        }

        private void InputDeviceOnControlChange(ControlChangeMessage msg)
        {
            _controlChangeMessages.AddOrUpdate(new ControlChangeKey(msg),
                key => new ControlChangeMessageHolder(msg, _timestampStopwatch),
                (key, holder) =>
                {
                    holder.SetMessage(msg, _timestampStopwatch);
                    return holder;
                });
        }

        private void OnControlChange(ControlChangeMessage msg)
        {
            ThreadPool.QueueUserWorkItem(state => ControlChange?.Invoke((ControlChangeMessage) state), msg);
        }

        private void OnNrpn(NrpnMessage msg)
        {
            ThreadPool.QueueUserWorkItem(state => Nrpn?.Invoke((NrpnMessage) state), msg);
        }
    }
}