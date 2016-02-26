using System.Windows;
using micdah.LrControl.Mapping;
using micdah.LrControl.Mapping.Functions;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for ControllerFunctionView.xaml
    /// </summary>
    public partial class ControllerFunctionView
    {
        public static readonly DependencyProperty ControllerFunctionProperty = DependencyProperty.Register(
            "ControllerFunction", typeof (ControllerFunction), typeof (ControllerFunctionView),
            new PropertyMetadata(default(ControllerFunction)));

        public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register(
            "Highlight", typeof (bool), typeof (ControllerFunctionView), new PropertyMetadata(default(bool)));

        public ControllerFunctionView()
        {
            InitializeComponent();
        }

        public ControllerFunction ControllerFunction
        {
            get { return (ControllerFunction) GetValue(ControllerFunctionProperty); }
            set { SetValue(ControllerFunctionProperty, value); }
        }

        public bool Highlight
        {
            get { return (bool) GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        private void ControllerFunctionView_OnDragEnter(object sender, DragEventArgs e)
        {
            Highlight = e.Data.GetDataPresent(typeof (FunctionFactory));
        }

        private void ControllerFunctionView_OnDragLeave(object sender, DragEventArgs e)
        {
            Highlight = false;
        }

        private void ControllerFunctionView_OnDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (FunctionFactory))) return;

            e.Effects = DragDropEffects.Move;
            Highlight = true;
        }

        private void ControllerFunctionView_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (FunctionFactory))) return;

            Highlight = false;

            var functionFactory = (FunctionFactory) e.Data.GetData(typeof (FunctionFactory));
            if (ControllerFunction != null)
            {
                ControllerFunction.Function = functionFactory.CreateFunction();
            }
        }

        private void DeleteFunctionButton_OnClick(object sender, RoutedEventArgs e)
        {
            ControllerFunction.Function = null;
        }
    }
}