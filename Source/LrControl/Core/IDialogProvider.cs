using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace micdah.LrControl.Core
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