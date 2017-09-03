using System.Windows;
using LrControl.Core.Configurations;

namespace LrControl.Ui.Gui
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {

        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            "Settings", typeof(ISettings), typeof(SettingsView), new PropertyMetadata(default(ISettings)));

        public ISettings Settings
        {
            get => (ISettings) GetValue(SettingsProperty);
            set => SetValue(SettingsProperty, value);
        }
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
