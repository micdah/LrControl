using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;
using LrControlCore.Mapping;

namespace micdah.LrControl.Gui
{
    /// <summary>
    ///     Interaction logic for ModuleGroupView.xaml
    /// </summary>
    public partial class ModuleGroupView : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ModuleGroupProperty = DependencyProperty.Register(
            "ModuleGroup", typeof (ModuleGroup), typeof (ModuleGroupView),
            new PropertyMetadata(default(ModuleGroup), ModuleGroupChanged));

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
            "Selected", typeof (FunctionGroup), typeof (ModuleGroupView), new PropertyMetadata(default(FunctionGroup)));

        public ModuleGroupView()
        {
            InitializeComponent();
        }

        public ModuleGroup ModuleGroup
        {
            get { return (ModuleGroup) GetValue(ModuleGroupProperty); }
            set { SetValue(ModuleGroupProperty, value); }
        }

        public FunctionGroup Selected
        {
            get { return (FunctionGroup) GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}