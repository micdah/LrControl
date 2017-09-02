using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;

namespace LrControl.Ui.Gui
{
    /// <summary>
    ///     Interaction logic for ModuleGroupView.xaml
    /// </summary>
    public partial class ModuleGroupView : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ModuleGroupProperty = DependencyProperty.Register(
            "ModuleGroup", typeof (ModuleGroupViewModel), typeof (ModuleGroupView),
            new PropertyMetadata(default(ModuleGroupViewModel), ModuleGroupChanged));

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
            "Selected", typeof (FunctionGroupViewModel), typeof (ModuleGroupView), new PropertyMetadata(default(FunctionGroupViewModel)));

        public ModuleGroupView()
        {
            InitializeComponent();
        }

        public ModuleGroupViewModel ModuleGroup
        {
            get => (ModuleGroupViewModel) GetValue(ModuleGroupProperty);
            set => SetValue(ModuleGroupProperty, value);
        }

        public FunctionGroupViewModel Selected
        {
            get => (FunctionGroupViewModel) GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        private static void ModuleGroupChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((ModuleGroupView)dependencyObject).SelectFirst();
            ((ModuleGroupView)dependencyObject).OnPropertyChanged(nameof(ModuleGroup));
        }

        private void SelectFirst()
        {
            if (ModuleGroup != null)
            {
                Selected = ModuleGroup.FunctionGroups.FirstOrDefault();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}