using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using micdah.LrControl.Annotations;
using micdah.LrControlApi;

namespace micdah.LrControl.Mapping.Catalog
{
    public partial class FunctionCatalog : INotifyPropertyChanged
    {
        private ObservableCollection<FunctionCatalogGroup> _groups;
        private readonly object _groupsLock = new object();

        private FunctionCatalog()
        {
        }

        public ObservableCollection<FunctionCatalogGroup> Groups
        {
            get { return _groups; }
            set
            {
                if (Equals(value, _groups)) return;
                _groups = value;
                BindingOperations.EnableCollectionSynchronization(_groups, _groupsLock);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static FunctionCatalog DefaultCatalog(LrApi api)
        {
            return new FunctionCatalog
            {
                Groups = CreateGroups(api)
            };
        }

        private static ObservableCollection<FunctionCatalogGroup> CreateGroups(LrApi api)
        {
            var groups = new List<FunctionCatalogGroup>
            {
                CreateViewGroup(api),
                CreateUndoGroup(api),
                CreateSelectionGroup(api)
            };
            groups.AddRange(CreateDevelopGroups(api));

            return new ObservableCollection<FunctionCatalogGroup>(groups);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}