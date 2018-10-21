using LrControl.Ui.Core;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace LrControl.Ui
{
    public interface IMainWindowDialogProvider : IDialogProvider
    {
        string ShowSaveDialog(string path);

        string ShowOpenDialog(string path);
    }

    public class MainWindowDialogProvider : MetroWindowDialogProvider, IMainWindowDialogProvider
    {
        public MainWindowDialogProvider(MetroWindow window) : base(window)
        {
        }

        public string ShowSaveDialog(string path)
        {
            var dialog = new SaveFileDialog
            {
                FileName = "Configuration",
                DefaultExt = ".xml",
                Filter = "Configuration files (.xml)|*.xml",
                InitialDirectory = path
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string ShowOpenDialog(string path)
        {
            var dialog = new OpenFileDialog
            {
                FileName = "Configuration",
                DefaultExt = ".xml",
                Filter = "Configuration filex (.xml)|*.xml",
                InitialDirectory = path
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}