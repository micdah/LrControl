namespace LrControlApi.Modules.LrDialogs
{
    internal class LrDialogs : ILrDialogs
    {
        public ConfirmResult Confirm(string message, string info = null, string actionVerb = "OK", string cancelVerb = "Cancel",
            string otherVerb = null)
        {
            throw new System.NotImplementedException();
        }

        public void Message(string message, string info = null, DialogStyle style = null)
        {
            throw new System.NotImplementedException();
        }

        public void ShowBezel(string message, double? fadeDelay = null)
        {
            throw new System.NotImplementedException();
        }

        public void ShowError(string errorString)
        {
            throw new System.NotImplementedException();
        }
    }
}