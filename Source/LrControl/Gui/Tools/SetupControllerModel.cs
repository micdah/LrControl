using System.ComponentModel;
using System.Runtime.CompilerServices;
using micdah.LrControl.Annotations;

namespace micdah.LrControl.Gui.Tools
{
    public class SetupControllerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}