using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LrControl.Core.Functions.Catalog;
using LrControl.Core.Functions.Factories;

namespace LrControl.Gui
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
            "FunctionCatalog", typeof (IFunctionCatalog), typeof (FunctionCatalogView), new PropertyMetadata(default(FunctionCatalog)));

        public FunctionCatalog FunctionCatalog
        {
            get => (FunctionCatalog) GetValue(FunctionCatalogProperty);
            set => SetValue(FunctionCatalogProperty, value);
        }

        private void FunctionFactoryTextBlock_OnMouseMove(object sender, MouseEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var functionFactory = textBlock?.Tag as IFunctionFactory;
            if (functionFactory != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var dataObject = new DataObject(typeof (IFunctionFactory), functionFactory);
                DragDrop.DoDragDrop(textBlock, dataObject, DragDropEffects.Move);
            }
        }
    }
}