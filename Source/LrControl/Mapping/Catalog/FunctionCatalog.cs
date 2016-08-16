using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using micdah.LrControl.Mapping.Functions;
using micdah.LrControlApi;

namespace micdah.LrControl.Mapping.Catalog
{
    public partial class FunctionCatalog : INotifyPropertyChanged
    {
        private ObservableCollection<FunctionCatalogGroup> _groups;

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

        public FunctionFactory GetFunctionFactory(string functionKey)
        {
            foreach (var group in Groups)
            {
                foreach (var functionFactory in @group.Functions)
                {
                    if (functionFactory.Key == functionKey)
                    {
                        return functionFactory;
                    }
                }
            }

            return null;
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