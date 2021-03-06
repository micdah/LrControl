﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControl.Api.Common;
using LrControl.Core.Configurations;
using LrControl.Core.Devices.Enums;
using Midi.Enums;

namespace LrControl.Core.Devices
{
    public delegate void ControllerValueChangedHandler(int controllerValue);

    public class Controller : INotifyPropertyChanged
    {
        private readonly Device _device;
        private int _lastValue;

        public Controller(Device device, ControllerMessageType messageType, ControllerType controllerType, Channel channel, int controlNumber, Range range)
        {
            _device = device;
            MessageType = messageType;
            ControllerType = controllerType;
            Channel = channel;
            ControlNumber = controlNumber;
            Range = range;
        }

        public ControllerMessageType MessageType { get; }
        public ControllerType ControllerType { get; }
        public Channel Channel { get; }
        public int ControlNumber { get; }
        public Range Range { get; }

        public int LastValue
        {
            get => _lastValue;
            private set
            {
                if (value == _lastValue) return;
                _lastValue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ControllerValueChangedHandler ControllerValueChanged;

        public void SetControllerValue(int controllerValue)
        {
            _device.OnDeviceOutput(this, controllerValue);
        }
        
        public void Reset()
        {
            if (Range != null)
                SetControllerValue((int) Range.Minimum);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void OnDeviceInput(int value)
        {
            if (value < Range)
            {
                Range.Minimum = value;
                OnPropertyChanged(nameof(Range));
            }
            else if (value > Range)
            {
                Range.Maximum = value;
                OnPropertyChanged(nameof(Range));
            }
            
            LastValue = value;
            ControllerValueChanged?.Invoke(value);
        }

        public bool IsController(ControllerConfigurationKey controllerKey)
        {
            return Channel == controllerKey.Channel
                   && ControlNumber == controllerKey.ControlNumber
                   && MessageType == controllerKey.MessageType;
        }
    }
}