using System.Windows;
using LrControlCore.Mapping;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for FunctionGroupView.xaml
    /// </summary>
    public partial class FunctionGroupView
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
            get => (FunctionGroup) GetValue(FunctionGroupProperty);
            set => SetValue(FunctionGroupProperty, value);
        }
    }
}