using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace micdah.LrControl.Core
{
    public class MetroWindowDialogProvider : IDialogProvider
    {
        private readonly MetroWindow _window;

        public MetroWindowDialogProvider(MetroWindow window)
        {
            _window = window;
        }

        public async Task<DialogResult> ShowMessage(string message, string title,
            DialogButtons buttons = DialogButtons.OkCancel)
        {
            MessageDialogStyle dialogStyle;
            var dialogSettings = new MetroDialogSettings
            {
                AnimateShow = true,
                AnimateHide = true,
                ColorScheme = MetroDialogColorScheme.Accented
            };

            switch (buttons)
            {
                case DialogButtons.YesNo:
                    dialogStyle = MessageDialogStyle.AffirmativeAndNegative;
                    dialogSettings.AffirmativeButtonText = "Yes";
                    dialogSettings.NegativeButtonText = "No";
                    break;
                case DialogButtons.OkCancel:
                    dialogStyle = MessageDialogStyle.AffirmativeAndNegative;
                    dialogSettings.AffirmativeButtonText = "Ok";
                    dialogSettings.NegativeButtonText = "Cancel";
                    break;
                case DialogButtons.Ok:
                    dialogStyle = MessageDialogStyle.Affirmative;
                    dialogSettings.AffirmativeButtonText = "Ok";
                    break;
                default:
                    throw new InvalidOperationException("Unknown DialogButtons option");
            }

            var dialogResult = await _window.ShowMessageAsync(title, message, dialogStyle, dialogSettings);
            switch (buttons)
            {
                case DialogButtons.YesNo:
                    return dialogResult == MessageDialogResult.Affirmative
                        ? DialogResult.Yes
                        : DialogResult.No;
                case DialogButtons.OkCancel:
                    return dialogResult == MessageDialogResult.Affirmative
                        ? DialogResult.Ok
                        : DialogResult.Cancel;
                case DialogButtons.Ok:
                    return DialogResult.Ok;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}