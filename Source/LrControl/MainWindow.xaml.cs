using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Midi.Devices;
using Midi.Enums;
using Control = Midi.Enums.Control;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Control _lastControl;
        private bool _receivingValue;
        private NrpnDecocer _nrpnDecocer;
        private NrpnEncoder _nrpnEncoder;

        public MainWindow()
        {
            InitializeComponent();

            var deviceName = "BCF2000";

            StartReceiving(InputDevice.InstalledDevices.Single(x => x.Name == deviceName));

            var outputDevices = OutputDevice.InstalledDevices;
            var outputDevice = OutputDevice.InstalledDevices.Single(x => x.Name == deviceName);
            _nrpnEncoder = new NrpnEncoder(outputDevice);

            outputDevice.Open();

        }

        private void StartReceiving(InputDevice input)
        {
            input.Open();

            _nrpnDecocer = new NrpnDecocer(input);
            _nrpnDecocer.OnNrpn += HandleNrpn;

            input.StartReceiving(null);
        }

        private void HandleNrpn(int parameter, int value)
        {
            var slider = GetSlider(parameter);

            Dispatcher.InvokeAsync(() =>
            {
                _receivingValue = true;
                slider.Value = value;
                _receivingValue = false;
            });
        }

        private void Fader_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var parameter = GetParameter((Slider) sender);
            var value = Convert.ToInt32(e.NewValue);

            _nrpnEncoder.SendNrpn(parameter, value);
        }

        private void MakeRandon_OnClick(object sender, RoutedEventArgs e)
        {
            var rand = new Random();

            for (int i = 0; i < 8; i++)
            {
                var slider = GetSlider(i);
                var value = rand.Next(0, 1023);

                slider.Value = value;
            }
        }

        private void SetZero_OnClick(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < 8; i++)
            {
                GetSlider(i).Value = 0;
            }
        }

        private Slider GetSlider(int parameter)
        {
            if (parameter == 0) return Fader1;
            if (parameter == 1) return Fader2;
            if (parameter == 2) return Fader3;
            if (parameter == 3) return Fader4;
            if (parameter == 4) return Fader5;
            if (parameter == 5) return Fader6;
            if (parameter == 6) return Fader7;
            if (parameter == 7) return Fader8;

            throw new ArgumentException($"There is no fader for parameter {parameter}");
        }

        private int GetParameter(Slider fader)
        {
            if (ReferenceEquals(fader, Fader1)) return 0;
            if (ReferenceEquals(fader, Fader2)) return 1;
            if (ReferenceEquals(fader, Fader3)) return 2;
            if (ReferenceEquals(fader, Fader4)) return 3;
            if (ReferenceEquals(fader, Fader5)) return 4;
            if (ReferenceEquals(fader, Fader6)) return 5;
            if (ReferenceEquals(fader, Fader7)) return 6;
            if (ReferenceEquals(fader, Fader8)) return 7;

            throw new ArgumentException($"There is no parameter for fader {fader}");
        }
    }
}