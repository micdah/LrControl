using System;
using System.Linq;
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
        private static readonly TimeSpan ReceivedTimeout = TimeSpan.FromSeconds(1);
        private readonly ManualResetEventSlim _receivedEvent = new ManualResetEventSlim(false);
        private readonly TestMidiInputDevice _inputDevice;
        private readonly TestMidiOutputDevice _outputDevice;
        
        protected static readonly Module DefaultModule = Module.Library;
        
        protected static readonly ControllerInfo Info1 = new ControllerInfo(
            new ControllerId(MessageType.Nrpn, Channel.Channel1, 1),
            new Range(0, 128));
        
        protected static readonly ControllerInfo Info2 = new ControllerInfo(
            new ControllerId(MessageType.Nrpn, Channel.Channel1, 2),
            new Range(0, 128));
        
        protected static ControllerId Id1 => Info1.ControllerId;
        protected static ControllerId Id2 => Info2.ControllerId;
        protected static Range Range1 => Info1.Range;
        protected static Range Range2 => Info2.Range;
        
        protected readonly IProfileManager ProfileManager;
        protected readonly Mock<ILrDevelopController> LrDevelopController;
        protected readonly Mock<ILrApplicationView> LrApplicationView;
        protected readonly Mock<ILrApi> LrApi;
        protected readonly Mock<ISettings> Settings;
        protected int Value;
        
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
            
            // Mock LrApplicationView
            LrApplicationView = new Mock<ILrApplicationView>();
            
            // Mock ILrApi
            LrApi = new Mock<ILrApi>();
            
            LrApi
                .Setup(m => m.LrDevelopController)
                .Returns(LrDevelopController.Object);

            LrApi
                .Setup(m => m.LrApplicationView)
                .Returns(LrApplicationView.Object);
            
            // Mock ISettings
            Settings = new Mock<ISettings>();
        }

        protected void ControllerInput(in ControllerId controllerId, params double[] values)
            => ControllerInput(in controllerId, values?.Select(x => (int) x).ToArray());

        protected void ControllerInput(in ControllerId controllerId, params int[] values)
        {
            foreach (var value in values)
            {
                _receivedEvent.Reset();
                
                var msg = new NrpnMessage(controllerId.Channel, controllerId.Parameter, value);
                Log.Debug("Sending message {@Message}", msg);
                _inputDevice.OnNrpn(msg);

                Log.Debug("Waiting until message has been received...");
                if (!_receivedEvent.Wait(ReceivedTimeout))
                {
                    Log.Warning("Failed to receive message within {Timeout}", ReceivedTimeout);
                    throw new Exception("Message not received as expected");
                }
            }
        }

        protected NrpnMessage TakeOutput()
        {
            Assert.True(_outputDevice.Messages.TryTake(out var msg));
            Log.Debug("Took output: {@Message}", msg);
            
            Assert.True(msg is NrpnMessage);
            return (NrpnMessage) msg;
        }

        protected void ClearOutput()
        {
            Log.Debug("Clearing output device message buffer");
            
            while (_outputDevice.Messages.TryTake(out var msg))
            {
                Log.Debug("Throwing away message: {@Message}", msg);
            }
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