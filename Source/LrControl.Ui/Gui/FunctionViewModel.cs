using System.Windows.Threading;
using LrControl.Core.Functions;
using LrControl.Ui.Core;

namespace LrControl.Ui.Gui
{
    public class FunctionViewModel : ViewModel
    {
        private string _displayName;

        public FunctionViewModel(Dispatcher dispatcher, IFunction function) : base(dispatcher)
        {
            DisplayName = function.DisplayName;
        }

        public string DisplayName
        {
            get => _displayName;
            private set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }
    }
}