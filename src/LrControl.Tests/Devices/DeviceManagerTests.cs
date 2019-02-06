using System;
using System.Collections.Concurrent;
using System.Linq;
using LrControl.Configurations;
using LrControl.Devices;
using Moq;
using RtMidi.Core.Enums;
using RtMidi.Core.Messages;
using Xunit;
using Xunit.Abstractions;
using Range = LrControl.LrPlugin.Api.Common.Range;

namespace LrControl.Tests.Devices
{
    public class DeviceManagerTests : TestSuite
    {
        private static readonly TimeSpan Wait = TimeSpan.FromSeconds(1);

        private readonly BlockingCollection<(ControllerId ControllerId, Range Range, int Value)> _controllerInput =
            new BlockingCollection<(ControllerId ControllerId, Range Range, int Value)>();

        private readonly TestMidiInputDevice _inputDevice;
        private readonly IDeviceManager _deviceManager;

        public DeviceManagerTests(ITestOutputHelper output) : base(output)
        {
            var settings = new Mock<ISettings>();
            _inputDevice = new TestMidiInputDevice("Test Input Device");
            var inputDeviceInfo = new TestInputDeviceInfo(_inputDevice);
            _deviceManager = new DeviceManager(settings.Object);

            _deviceManager.ControllerInput += (in ControllerId id, Range range, int value) =>
            {
                _controllerInput.Add((id, range, value));
            };

            _deviceManager.SetInputDevice(inputDeviceInfo);
        }

        [Fact]
        public void Should_Set_And_Open_Input_Device()
        {
            // Setup
            var device = new TestMidiInputDevice("Test Device");
            var info = new TestInputDeviceInfo(device);

            // Test
            _deviceManager.SetInputDevice(info);

            // Verify
            var testInfo = _deviceManager.InputDevice;
            Assert.NotNull(testInfo);
            Assert.Equal(device.Name, testInfo.Name);
        }

        [Fact]
        public void Should_Trigger_ControllerInput()
        {
            // Setup
            var msg = new NrpnMessage(Channel.Channel1, 12, 24);

            // Test
            _inputDevice.OnNrpn(msg);
            Assert.True(_controllerInput.TryTake(out var ev, Wait));

            // Verify
            Assert.Equal(new ControllerId(msg), ev.ControllerId);
            Assert.Equal(msg.Value, ev.Value);
        }

        [Fact]
        public void Should_Update_Range_Based_On_Seen_Values()
        {
            // Setup
            var msgLow = new NrpnMessage(Channel.Channel1, 12, 0);
            var msgHigh = new NrpnMessage(Channel.Channel1, 12, 255);

            // Test
            _inputDevice.OnNrpn(msgLow);
            Assert.True(_controllerInput.TryTake(out var evLow, Wait));

            _inputDevice.OnNrpn(msgHigh);
            Assert.True(_controllerInput.TryTake(out var evHigh, Wait));

            // Verify            
            Assert.Equal(new ControllerId(msgLow), evLow.ControllerId);
            Assert.Equal(msgLow.Value, evLow.Range.Minimum);

            Assert.Equal(new ControllerId(msgHigh), evHigh.ControllerId);
            Assert.Equal(msgLow.Value, evLow.Range.Minimum);
            Assert.Equal(msgHigh.Value, evHigh.Range.Maximum);
        }

        [Fact]
        public void Should_Track_Last_Seen_Value()
        {
            // Setup
            var msg0 = new NrpnMessage(Channel.Channel1, 12, 42);
            var msg1 = new NrpnMessage(Channel.Channel1, 12, 24);

            // Test
            _inputDevice.OnNrpn(msg0);
            Assert.True(_controllerInput.TryTake(out _, Wait));
            
            _inputDevice.OnNrpn(msg1);
            Assert.True(_controllerInput.TryTake(out _, Wait));

            // Verify
            var info = _deviceManager.ControllerInfos.SingleOrDefault(x => x.ControllerId == new ControllerId(msg1));
            Assert.NotNull(info);
            Assert.Equal(msg1.Value, info.LastValue);
        }

        [Fact]
        public void Clear_Should_Clear_All_Tracked_Controller_Infos()
        {
            // Setup
            var msg = new NrpnMessage(Channel.Channel1, 12, 24);
            var id = new ControllerId(msg);

            // Test
            Assert.Null(_deviceManager.ControllerInfos.SingleOrDefault(x => x.ControllerId == id));
            
            _inputDevice.OnNrpn(msg);
            Assert.True(_controllerInput.TryTake(out _, Wait));
            Assert.NotNull(_deviceManager.ControllerInfos.SingleOrDefault(x => x.ControllerId == id));

            _deviceManager.Clear();
            Assert.Null(_deviceManager.ControllerInfos.SingleOrDefault(x => x.ControllerId == id));
        }
        
        // TODO Test output device related methods
    }
}