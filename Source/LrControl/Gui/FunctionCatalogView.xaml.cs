using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using micdah.LrControl.Mapping.Catalog;
using micdah.LrControl.Mapping.Functions;

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

        private void FunctionFactoryTextBlock_OnMouseMove(object sender, MouseEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var functionFactory = textBlock?.Tag as FunctionFactory;
            if (functionFactory != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var dataObject = new DataObject(typeof (FunctionFactory), functionFactory);
                DragDrop.DoDragDrop(textBlock, dataObject, DragDropEffects.Move);
            }
        }
    }
}