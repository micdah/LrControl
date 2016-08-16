using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LrControlCore.Functions.Factories;
using micdah.LrControlApi;

namespace LrControlCore.Functions.Catalog
{
    public partial class FunctionCatalog : IFunctionCatalog, INotifyPropertyChanged
    {
        private ObservableCollection<IFunctionCatalogGroup> _groups;

        private FunctionCatalog()
        {
        }

        public ObservableCollection<IFunctionCatalogGroup> Groups
        {
            get { return _groups; }
            set
            {
                if (Equals(value, _groups)) return;
                _groups = value;
                OnPropertyChanged();
            }
        }

        public IFunctionFactory GetFunctionFactory(string functionKey)
        {
            foreach (var group in Groups)
            {
                foreach (var functionFactory in group.Functions)
                {
                    if (functionFactory.Key == functionKey)
                    {
                        return functionFactory;
                    }
                }
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static IFunctionCatalog DefaultCatalog(LrApi api)
        {
            return new FunctionCatalog
            {
                Groups = CreateGroups(api)
            };
        }

        private static ObservableCollection<IFunctionCatalogGroup> CreateGroups(LrApi api)
        {
            var groups = new List<IFunctionCatalogGroup>
            {
                CreateViewGroup(api),
                CreateUndoGroup(api),
                CreateSelectionGroup(api)
            };
            groups.AddRange(CreateDevelopGroups(api));

            return new ObservableCollection<IFunctionCatalogGroup>(groups);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}