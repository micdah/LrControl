using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using JetBrains.Annotations;
using micdah.LrControl.Mapping.Functions;

namespace micdah.LrControl.Mapping.Catalog
{
    public class FunctionCatalogGroup : INotifyPropertyChanged
    {
        private string _displayName;
        private ObservableCollection<FunctionFactory> _functions;
        private readonly object _functionsLock = new object();
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

        public ObservableCollection<FunctionFactory> Functions
        {
            get { return _functions; }
            set
            {
                if (Equals(value, _functions)) return;
                _functions = value;
                BindingOperations.EnableCollectionSynchronization(_functions, _functionsLock);
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