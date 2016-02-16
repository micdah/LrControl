namespace LrControlApi.Modules.LrControl
{
    public interface ILrControl
    {
        /// <summary>
        ///     Gets the running plugin version (major.minor)
        /// </summary>
        /// <returns></returns>
        bool GetApiVersion(out string apiVersion);
    }
}