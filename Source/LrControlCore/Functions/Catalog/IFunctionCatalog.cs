using System.Collections.ObjectModel;
using System.ComponentModel;
using LrControlCore.Functions.Factories;

namespace LrControlCore.Functions.Catalog
{
    public interface IFunctionCatalog
    {
        ObservableCollection<IFunctionCatalogGroup> Groups { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        IFunctionFactory GetFunctionFactory(string functionKey);
    }
}