using System.Windows;
using LrControl.Core.Functions.Factories;
using LrControl.Core.Mapping;
using LrControl.Gui.Utils;

namespace LrControl.Gui
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
            get => (ControllerFunction) GetValue(ControllerFunctionProperty);
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
            var functionFactory = (IFunctionFactory) e.Data.GetData(typeof (IFunctionFactory));

            // Verify we have all needed parameters
            var moduleGroup = this.FindParent<ModuleGroupView>()?.ModuleGroup;
            var functionGroup = this.FindParent<FunctionGroupView>()?.FunctionGroup;
            var controllerFunction = ControllerFunction;
            if (moduleGroup == null || functionGroup == null || controllerFunction == null) return;

            if (moduleGroup.CanAssignFunction(controllerFunction.Controller, functionGroup.IsGlobal))
            {
                ControllerFunction.Function = functionFactory.CreateFunction();
                moduleGroup.RecalculateControllerFunctionState();
            }
        }

        private void DeleteFunctionButton_OnClick(object sender, RoutedEventArgs e)
        {
            var moduleGroup = this.FindParent<ModuleGroupView>()?.ModuleGroup;
            if (moduleGroup != null)
            {
                ControllerFunction.Function = null;
                moduleGroup.RecalculateControllerFunctionState();
            }
        }
    }
}