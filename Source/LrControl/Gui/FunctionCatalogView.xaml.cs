using System.Windows;
using System.Windows.Controls;
using micdah.LrControl.Mapping.Catalog;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for FunctionCatalogView.xaml
    /// </summary>
    public partial class FunctionCatalogView
    {
        public FunctionCatalogView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FunctionCatalogProperty = DependencyProperty.Register(
            "FunctionCatalog", typeof (FunctionCatalog), typeof (FunctionCatalogView), new PropertyMetadata(default(FunctionCatalog)));

        public FunctionCatalog FunctionCatalog
        {
            get { return (FunctionCatalog) GetValue(FunctionCatalogProperty); }
            set { SetValue(FunctionCatalogProperty, value); }
        }
    }
}