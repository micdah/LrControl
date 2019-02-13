using System;
using System.Threading;
using LrControl.Configurations;
using LrControl.Devices;
using LrControl.LrPlugin.Api;
using LrControl.LrPlugin.Api.Modules.LrApplicationView;
using LrControl.LrPlugin.Api.Modules.LrDevelopController;
using LrControl.Profiles;
using LrControl.Tests.Mocks;
using Moq;
using RtMidi.Core.Enums;
using RtMidi.Core.Messages;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Devices
{
    public class ProfileManagerTestSuite : TestSuite
    {
        protected static readonly Module DefaultModule = Module.Library;

        protected static readonly ControllerInfo Info1 = new ControllerInfo(
            new ControllerId(MessageType.Nrpn, Channel.Channel1, 1),
            new Range(0, 128));

        protected static readonly ControllerInfo Info2 = new ControllerInfo(
            new ControllerId(MessageType.Nrpn, Channel.Channel1, 2),
            new Range(0, 128));
        
        protected readonly IProfileManager ProfileManager;
        protected readonly Mock<ILrDevelopController> LrDevelopController;
        protected readonly Mock<ILrApi> LrApi;
        protected readonly Mock<ISettings> Settings;
        protected int Value;
        
        private readonly ManualResetEvent _receivedEvent = new ManualResetEvent(false);
        private readonly TestMidiInputDevice _inputDevice;
        private readonly TestMidiOutputDevice _outputDevice;
        private static readonly TimeSpan ReceivedTimeout = TimeSpan.FromSeconds(1);

        protected ProfileManagerTestSuite(ITestOutputHelper output) : base(output)
        {
            // Create test device manager
            var settings = new Mock<ISettings>();
            var deviceManager = new DeviceManager(settings.Object, new[] {Info1, Info2});

            _inputDevice = new TestMidiInputDevice("Test Input Device");
            deviceManager.SetInputDevice(new TestInputDeviceInfo(_inputDevice));

            _outputDevice = new TestMidiOutputDevice("Test Output Device");
            deviceManager.SetOutputDevice(new TestOutputDeviceInfo(_outputDevice));

            // Create test profile manager
            ProfileManager = new ProfileManager(deviceManager);

            deviceManager.Input += (in ControllerId id, Range range, int value) =>
            {
                Log.Debug("ControllerInput: {@Id}, {@Range}, {Value}", id, range, value);
                _receivedEvent.Set();
            };
            
            // Mock ILrDevelopController
            LrDevelopController = new Mock<ILrDevelopController>();
            
            // Mock ILrApi
            LrApi = new Mock<ILrApi>();
            
            LrApi
                .Setup(m => m.LrDevelopController)
                .Returns(LrDevelopController.Object);
            
            // Mock ISettings
            Settings = new Mock<ISettings>();
        }

        protected void ControllerInput(in ControllerId controllerId, int value, MessageType messageType = MessageType.Nrpn)
        {
            _receivedEvent.Reset();

            switch (messageType)
            {
                case MessageType.Nrpn:
                {
                    var msg = new NrpnMessage(controllerId.Channel, controllerId.Parameter, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnNrpn(msg);
                    break;
                }
                case MessageType.NoteOn:
                {
                    var msg = new NoteOnMessage(controllerId.Channel, (Key) controllerId.Parameter, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnNoteOn(msg);
                    break;
                }
                case MessageType.NoteOff:
                {
                    var msg = new NoteOffMessage(controllerId.Channel, (Key) controllerId.Parameter, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnNoteOff(msg);
                    break;
                }
                case MessageType.PitchBend:
                {
                    var msg = new PitchBendMessage(controllerId.Channel, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnPitchBend(msg);
                    break;
                }
                case MessageType.ControlChange:
                {
                    var msg = new ControlChangeMessage(controllerId.Channel, controllerId.Parameter, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnControlChange(msg);
                    break;
                }
                case MessageType.ProgramChange:
                {
                    var msg = new ProgramChangeMessage(controllerId.Channel, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnProgramChange(msg);
                    break;
                }
                case MessageType.ChannelPressure:
                {
                    var msg = new ChannelPressureMessage(controllerId.Channel, value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnChannelPressure(msg);
                    break;
                }
                case MessageType.PolyphonicKeyPressure:
                {
                    var msg = new PolyphonicKeyPressureMessage(controllerId.Channel, (Key) controllerId.Parameter,
                        value);
                    Log.Debug("Sending message {@Message}", msg);
                    _inputDevice.OnPolyphonicKeyPressure(msg);
                    break;
                }
                default:
                    throw new Exception($"Unknown {nameof(MessageType)} {messageType}");
            }

            Log.Debug("Waiting until message has been received...");
            if (_receivedEvent.WaitOne(ReceivedTimeout)) return;
            
            Log.Warning("Failed to receive message within {Timeout}", ReceivedTimeout);    
            throw new Exception("Message not received as expected");
        }

        protected void ClearOutputMessages()
        {
            Log.Debug("Clearing output device message buffer");
            
            while (_outputDevice.Messages.TryTake(out var msg))
            {
                Log.Debug("Throwing away message: {@Message}", msg);
            }
        }

        protected T TakeOutput<T>()
        {
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Log.Debug("Took output: {@Message}", msg);
            
            Assert.True(msg is T);
            return (T) msg;
        }

        protected void AssertNoMoreOutput()
        {
            var success = _outputDevice.Messages.TryTake(out var msg);
            if (success)
                Log.Warning("Unexpected output: {@Message}", msg);

            Assert.False(success);
        }
    }
}