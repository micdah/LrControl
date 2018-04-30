namespace LrControl.Api.Common
{
    public enum ProcessingThreadState
    {
        /// <summary>
        /// Procssing thread has been started
        /// </summary>
        Started,
        
        /// <summary>
        /// Processing thread has been stopped
        /// </summary>
        Stopped,
        
        /// <summary>
        /// Processing thread has been terminated
        /// </summary>
        Terminated
    }
}