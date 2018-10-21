using System.Windows;

namespace LrControl.Ui.Gui
{
    /// <summary>
    ///     Interaction logic for FunctionGroupView.xaml
    /// </summary>
    public partial class FunctionGroupView
    {
        public static readonly DependencyProperty FunctionGroupProperty = DependencyProperty.Register(
            "FunctionGroup", typeof (FunctionGroupViewModel), typeof (FunctionGroupView),
            new PropertyMetadata(default(FunctionGroupViewModel)));

        public FunctionGroupView()
        {
            InitializeComponent();
        }

        public FunctionGroupViewModel FunctionGroup
        {
            get => (FunctionGroupViewModel) GetValue(FunctionGroupProperty);
            set => SetValue(FunctionGroupProperty, value);
        }
    }
}