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
        private readonly TestMidiOutputDevice _outputDevice;
        private readonly IDeviceManager _deviceManager;

        public DeviceManagerTests(ITestOutputHelper output) : base(output)
        {
            var settings = new Mock<ISettings>();
            
            // Create device manager
            _deviceManager = new DeviceManager(settings.Object);
            _deviceManager.Input += (in ControllerId id, Range range, int value) =>
            {
                _controllerInput.Add((id, range, value));
            };

            // Set input device
            _inputDevice = new TestMidiInputDevice("Test Input Device");
            _deviceManager.SetInputDevice(new TestInputDeviceInfo(_inputDevice));
            
            // Set output device
            _outputDevice = new TestMidiOutputDevice("Test Output Device");
            _deviceManager.SetOutputDevice(new TestOutputDeviceInfo(_outputDevice));
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

        [Fact]
        public void Should_Send_Message_On_Output()
        {
            const int param = 12;
            const int value = 42;
            const Channel channel = Channel.Channel1;
            
            T Test<T>(MessageType messageType)
            {
                var controllerId = new ControllerId(messageType, channel, param);
                _deviceManager.OnOutput(controllerId, value);

                Assert.True(_outputDevice.Messages.TryTake(out var msg));
                Assert.True(msg is T);
                return (T) msg;
            }

            var noteOff = Test<NoteOffMessage>(MessageType.NoteOff);
            Assert.Equal(channel, noteOff.Channel);
            Assert.Equal(param, (int)noteOff.Key);
            Assert.Equal(value, noteOff.Velocity);

            var noteOn = Test<NoteOnMessage>(MessageType.NoteOn);
            Assert.Equal(channel, noteOn.Channel);
            Assert.Equal(param, (int) noteOn.Key);
            Assert.Equal(value, noteOn.Velocity);

            var poly = Test<PolyphonicKeyPressureMessage>(MessageType.PolyphonicKeyPressure);
            Assert.Equal(channel, poly.Channel);
            Assert.Equal(param, (int) poly.Key);
            Assert.Equal(value, poly.Pressure);

            var controlChange = Test<ControlChangeMessage>(MessageType.ControlChange);
            Assert.Equal(channel, controlChange.Channel);
            Assert.Equal(param, controlChange.Control);
            Assert.Equal(value, controlChange.Value);

            var programChange = Test<ProgramChangeMessage>(MessageType.ProgramChange);
            Assert.Equal(channel, programChange.Channel);
            Assert.Equal(value, programChange.Program);

            var channelPressure = Test<ChannelPressureMessage>(MessageType.ChannelPressure);
            Assert.Equal(channel, channelPressure.Channel);
            Assert.Equal(value, channelPressure.Pressure);

            var pitchBend = Test<PitchBendMessage>(MessageType.PitchBend);
            Assert.Equal(channel, pitchBend.Channel);
            Assert.Equal(value, pitchBend.Value);

            var nrpn = Test<NrpnMessage>(MessageType.Nrpn);
            Assert.Equal(channel, nrpn.Channel);
            Assert.Equal(param, nrpn.Parameter);
            Assert.Equal(value, nrpn.Value);
        }
    }
}