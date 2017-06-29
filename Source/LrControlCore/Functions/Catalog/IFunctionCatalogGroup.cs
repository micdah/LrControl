using System.Collections.ObjectModel;
using System.ComponentModel;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public interface IFunctionCatalogGroup : INotifyPropertyChanged
    {
        string DisplayName { get; set; }
        string Key { get; set; }
        ObservableCollection<IFunctionFactory> Functions { get; set; }
    }
}