using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace LrControl.Ui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private MainWindowModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindowModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (Equals(value, _viewModel)) return;
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}