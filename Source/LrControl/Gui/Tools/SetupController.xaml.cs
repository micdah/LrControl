using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;

namespace micdah.LrControl.Gui.Tools
{
    /// <summary>
    ///     Interaction logic for SetupController.xaml
    /// </summary>
    public partial class SetupController : INotifyPropertyChanged
    {
        private SetupControllerModel _viewModel;

        public SetupController(SetupControllerModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
        }

        public SetupControllerModel ViewModel
        {
            get { return _viewModel; }
            private set
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