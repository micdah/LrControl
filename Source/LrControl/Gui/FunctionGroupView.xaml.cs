using System.Windows;
using System.Windows.Controls;
using micdah.LrControl.Mapping;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for FunctionGroupView.xaml
    /// </summary>
    public partial class FunctionGroupView : UserControl
    {
        public static readonly DependencyProperty FunctionGroupProperty = DependencyProperty.Register(
            "FunctionGroup", typeof (FunctionGroup), typeof (FunctionGroupView),
            new PropertyMetadata(default(FunctionGroup)));

        public FunctionGroupView()
        {
            InitializeComponent();
        }

        public FunctionGroup FunctionGroup
        {
            get { return (FunctionGroup) GetValue(FunctionGroupProperty); }
            set { SetValue(FunctionGroupProperty, value); }
        }
    }
}