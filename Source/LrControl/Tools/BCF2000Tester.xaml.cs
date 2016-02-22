using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Midi.Devices;
using Midi.Enums;
using Midi.Messages;
using Control = Midi.Enums.Control;

namespace micdah.LrControl.Tools
{
    /// <summary>
    ///     Interaction logic for BCF2000Tester.xaml
    /// </summary>
    public partial class BCF2000Tester : Window
    {
        private readonly ToggleButton[] _buttons;
        private readonly Slider[] _sliders;

        private readonly ManualResetEvent _stopAnimation = new ManualResetEvent(false);
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;

        public BCF2000Tester()
        {
            InitializeComponent();

            _buttons = new ToggleButton[38];
            _sliders = new Slider[16];

            MakeFaders();
            MakeEncoders();
            MakeButtons();

            OpenDevices("BCF2000");

            SetAllButtons(false);
            SetAllSliders(slider => slider.Maximum/2);
        }

        private void OpenDevices(string deviceName)
        {
            _inputDevice = DeviceManager.InputDevices.Single(x => x.Name == deviceName);
            _inputDevice.Nrpn += InputDeviceOnNrpn;
            _inputDevice.ControlChange += InputDeviceOnControlChange;
            _inputDevice.Open();
            _inputDevice.StartReceiving(null);

            _outputDevice = DeviceManager.OutputDevices.Single(x => x.Name == deviceName);
            _outputDevice.Open();
        }

        private void MakeEncoders()
        {
            var grid = Encoders;
            AddRowsAndColumns(grid, 1, 8);

            for (var column = 0; column < 8; column++)
            {
                // Button
                var control = 16 + column;

                var toggleButton = new ToggleButton
                {
                    Margin = new Thickness(5),
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(toggleButton, 0);
                Grid.SetColumn(toggleButton, column);

                _buttons[control] = toggleButton;

                toggleButton.Checked +=
                    (sender, args) =>
                        _outputDevice.SendControlChange(Channel.Channel1, (Control) control, 127);

                toggleButton.Unchecked +=
                    (sender, args) =>
                        _outputDevice.SendControlChange(Channel.Channel1, (Control) control, 0);

                grid.Children.Add(toggleButton);

                // Slider
                var parameter = 8 + column;

                var slider = new Slider
                {
                    //Margin = new Thickness(5),
                    Minimum = 0,
                    Maximum = 511,
                    Orientation = Orientation.Horizontal
                };

                _sliders[parameter] = slider;

                toggleButton.Content = slider;

                slider.ValueChanged +=
                    (sender, args) =>
                        _outputDevice.SendNrpn(Channel.Channel1, parameter, Convert.ToInt32(args.NewValue));
            }
        }

        private void MakeFaders()
        {
            var grid = Faders;
            AddRowsAndColumns(grid, 1, 8);

            for (var column = 0; column < 8; column++)
            {
                var parameter = column;

                var slider = new Slider
                {
                    Margin = new Thickness(5),
                    Minimum = 0,
                    Maximum = 1023,
                    Orientation = Orientation.Vertical
                };
                Grid.SetRow(slider, 0);
                Grid.SetColumn(slider, column);

                _sliders[parameter] = slider;

                grid.Children.Add(slider);

                slider.ValueChanged +=
                    (sender, args) =>
                        _outputDevice.SendNrpn(Channel.Channel1, parameter, Convert.ToInt32(args.NewValue));
            }
        }

        private void MakeButtons()
        {
            MakeButtonGrid(TopButtons, 2, 8, 0);
            MakeButtonGrid(EncoderGroups, 2, 2, 24);
            MakeButtonGrid(FunctionButtons, 2, 2, 28);
            MakeButtonGrid(PresetButtons, 1, 2, 32);
            MakeButtonGrid(CustomButtons, 2, 2, 34);
        }

        private void MakeButtonGrid(Grid grid, int rows, int columns, int index)
        {
            AddRowsAndColumns(grid, rows, columns);

            for (var column = 0; column < columns; column++)
            {
                for (var row = 0; row < rows; row++)
                {
                    var control = index + column*rows + row;

                    var toggleButton = new ToggleButton
                    {
                        Margin = new Thickness(5)
                    };
                    Grid.SetRow(toggleButton, row);
                    Grid.SetColumn(toggleButton, column);

                    _buttons[control] = toggleButton;

                    grid.Children.Add(toggleButton);

                    toggleButton.Checked +=
                        (sender, args) =>
                            _outputDevice.SendControlChange(Channel.Channel1, (Control) control, 127);

                    toggleButton.Unchecked +=
                        (sender, args) =>
                            _outputDevice.SendControlChange(Channel.Channel1, (Control) control, 0);
                }
            }
        }

        private static void AddRowsAndColumns(Grid grid, int rows, int columns)
        {
            for (var i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }

            for (var i = 0; i < columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
        }

        private void InputDeviceOnNrpn(NrpnMessage msg)
        {
            var parameter = msg.Parameter;
            if (parameter < _sliders.Length)
            {
                var value = msg.Value;
                var slider = _sliders[parameter];
                if (slider != null)
                {
                    Dispatcher.InvokeAsync(() => slider.Value = value);
                }
            }
        }

        private void InputDeviceOnControlChange(ControlChangeMessage msg)
        {
            var control = (int) msg.Control;
            if (control < _buttons.Length)
            {
                var isChecked = msg.Value == 127;
                var button = _buttons[control];
                if (button != null)
                {
                    Dispatcher.InvokeAsync(() => button.IsChecked = isChecked);
                }
            }
        }

        private void SetAllButtons(bool isChecked)
        {
            foreach (var button in _buttons) button.IsChecked = isChecked;
        }

        private void SetAllSliders(Func<Slider, double> sliderValue)
        {
            foreach (var slider in _sliders) slider.Value = sliderValue(slider);
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            SetAllButtons(false);
            SetAllSliders(slider => slider.Maximum/2);
        }

        private void Aniamte_OnClick(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton) sender;

            if (toggleButton.IsChecked == true)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    _stopAnimation.Reset();

                    int steps = 90;
                    int step = 0;

                    while (_stopAnimation.WaitOne(30) == false)
                    {
                        var length = _sliders.Length;
                        for (var i = 0; i < length; i++)
                        { 
                            var slider = _sliders[i];
                            var localStep = step;
                            var localI = i;
                            Dispatcher.InvokeAsync(() => slider.Value = (slider.Maximum/(steps-1))*((localStep + (steps/length*localI))%steps));
                        }


                        step++;
                    }
                });
            }
            else
            {
                _stopAnimation.Set();
            }
        }
    }
}