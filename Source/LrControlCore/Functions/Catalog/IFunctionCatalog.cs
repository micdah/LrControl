using System.Collections.ObjectModel;
using System.ComponentModel;
using LrControlCore.Functions.Factories;

namespace LrControlCore.Functions.Catalog
{
    public interface IFunctionCatalog : INotifyPropertyChanged
    {
        ObservableCollection<IFunctionCatalogGroup> Groups { get; set; }
        IFunctionFactory GetFunctionFactory(string functionKey);
    }
}