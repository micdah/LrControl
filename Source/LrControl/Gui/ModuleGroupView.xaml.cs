using System.Windows;
using System.Windows.Controls;
using micdah.LrControl.Mapping;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for ModuleGroupView.xaml
    /// </summary>
    public partial class ModuleGroupView
    {
        public ModuleGroupView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ModuleGroupProperty = DependencyProperty.Register(
            "ModuleGroup", typeof (ModuleGroup), typeof (ModuleGroupView), new PropertyMetadata(default(ModuleGroup)));

        public ModuleGroup ModuleGroup
        {
            get { return (ModuleGroup) GetValue(ModuleGroupProperty); }
            set { SetValue(ModuleGroupProperty, value); }
        }

    }
}