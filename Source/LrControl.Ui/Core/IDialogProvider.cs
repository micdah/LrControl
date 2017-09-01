using System.Threading.Tasks;

namespace LrControl.Ui.Core
{
    public interface IDialogProvider
    {
        Task<DialogResult> ShowMessage(string message, string title, DialogButtons buttons = DialogButtons.OkCancel);
    }

    public enum DialogResult
    {
        Ok,
        Cancel,
        Yes,
        No
    }

    public enum DialogButtons
    {
        Ok,
        OkCancel,
        YesNo,
    }
}