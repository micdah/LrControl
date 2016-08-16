using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControlCore.Functions.Factories;

namespace LrControlCore.Functions.Catalog
{
    public class FunctionCatalogGroup : INotifyPropertyChanged, IFunctionCatalogGroup
    {
        private string _displayName;
        private ObservableCollection<IFunctionFactory> _functions;
        private string _key;
            
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value == _displayName) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public string Key
        {
            get { return _key; }
            set
            {
                if (value == _key) return;
                _key = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IFunctionFactory> Functions
        {
            get { return _functions; }
            set
            {
                if (Equals(value, _functions)) return;
                _functions = value;
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