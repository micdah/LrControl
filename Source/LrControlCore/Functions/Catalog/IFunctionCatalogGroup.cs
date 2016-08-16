using System.Collections.ObjectModel;
using System.ComponentModel;
using LrControlCore.Functions.Factories;

namespace LrControlCore.Functions.Catalog
{
    public interface IFunctionCatalogGroup
    {
        string DisplayName { get; set; }
        string Key { get; set; }
        ObservableCollection<IFunctionFactory> Functions { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}