using System.Collections.ObjectModel;
using System.ComponentModel;
using LrControl.Core.Functions.Factories;

namespace LrControl.Core.Functions.Catalog
{
    public interface IFunctionCatalog : INotifyPropertyChanged
    {
        ObservableCollection<IFunctionCatalogGroup> Groups { get; set; }
        IFunctionFactory GetFunctionFactory(string functionKey);
    }
}