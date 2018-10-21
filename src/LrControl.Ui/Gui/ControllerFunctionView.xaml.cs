using System.Windows;
using LrControl.Core.Functions.Factories;
using LrControl.Ui.Gui.Utils;

namespace LrControl.Ui.Gui
{
    /// <summary>
    ///     Interaction logic for ControllerFunctionView.xaml
    /// </summary>
    public partial class ControllerFunctionView
    {
        public static readonly DependencyProperty ControllerFunctionProperty = DependencyProperty.Register(
            "ControllerFunction", typeof (ControllerFunctionViewModel), typeof (ControllerFunctionView),
            new PropertyMetadata(default(ControllerFunctionViewModel)));

        public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register(
            "Highlight", typeof (bool), typeof (ControllerFunctionView), new PropertyMetadata(default(bool)));

        public ControllerFunctionView()
        {
            InitializeComponent();
        }

        public ControllerFunctionViewModel ControllerFunction
        {
            get => (ControllerFunctionViewModel) GetValue(ControllerFunctionProperty);
            set => SetValue(ControllerFunctionProperty, value);
        }

        public bool Highlight
        {
            get => (bool) GetValue(HighlightProperty);
            set => SetValue(HighlightProperty, value);
        }

        private void ControllerFunctionView_OnDragEnter(object sender, DragEventArgs e)
        {
            if (ControllerFunction.Assignable)
            {
                Highlight = e.Data.GetDataPresent(typeof(IFunctionFactory));
            }
        }

        private void ControllerFunctionView_OnDragLeave(object sender, DragEventArgs e)
        {
            Highlight = false;
        }

        private void ControllerFunctionView_OnDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (IFunctionFactory))) return;

            if (ControllerFunction.Assignable)
            {
                e.Effects = DragDropEffects.Move;
                Highlight = true;
            }
        }

        private void ControllerFunctionView_OnDrop(object sender, DragEventArgs e)
        {
            Highlight = false;

            // Verify drop object contains needed object
            if (!e.Data.GetDataPresent(typeof (IFunctionFactory))) return;
            if (!(e.Data.GetData(typeof(IFunctionFactory)) is IFunctionFactory functionFactory)) return;

            // Verify we have all needed parameters
            var moduleGroupVm = this.FindParent<ModuleGroupView>()?.ModuleGroup;
            var functionGroupVm = this.FindParent<FunctionGroupView>()?.FunctionGroup;
            if (moduleGroupVm == null || functionGroupVm == null || ControllerFunction == null) return;

            if (moduleGroupVm.CanAssignFunction(ControllerFunction.Controller, functionGroupVm.IsGlobal))
            {
                ControllerFunction.SetFunction(functionFactory.CreateFunction());
                moduleGroupVm.RecalculateControllerFunctionState();
            }
        }

        private void DeleteFunctionButton_OnClick(object sender, RoutedEventArgs e)
        {
            var moduleGroupVm = this.FindParent<ModuleGroupView>()?.ModuleGroup;
            if (moduleGroupVm != null)
            {
                ControllerFunction.SetFunction(null);
                moduleGroupVm.RecalculateControllerFunctionState();
            }
        }
    }
}