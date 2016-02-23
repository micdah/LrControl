using System.Windows;
using System.Windows.Controls;
using micdah.LrControl.Mapping;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for ControllerFunctionView.xaml
    /// </summary>
    public partial class ControllerFunctionView : UserControl
    {
        public ControllerFunctionView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ControllerFunctionProperty = DependencyProperty.Register(
            "ControllerFunction", typeof (ControllerFunction), typeof (ControllerFunctionView), new PropertyMetadata(default(ControllerFunction)));

        public ControllerFunction ControllerFunction
        {
            get { return (ControllerFunction) GetValue(ControllerFunctionProperty); }
            set { SetValue(ControllerFunctionProperty, value); }
        }
    }
}