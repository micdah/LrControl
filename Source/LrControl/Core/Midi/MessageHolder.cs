using System.Diagnostics;

namespace micdah.LrControl.Core.Midi
{
    internal abstract class MessageHolder<T> where T : class
    {
        public MessageHolder(T msg, Stopwatch timestampStopwatch)
        {
            SetMessage(msg, timestampStopwatch);
        }

        public T Message { get; private set; }
        public long MessageTimestamp { get; private set; }
        public T LastSent { get; private set; }
        public long LastSentTimestamp { get; private set; }

        public bool HasChanged
        {
            get
            {
                if (Message == null) return false;
                if (LastSent == null) return true;
                return CalculateHasChanged();
            }
        }

        protected abstract bool CalculateHasChanged();

        public void SetMessage(T msg, Stopwatch timestampStopwatch)
        {
            MessageTimestamp = timestampStopwatch.ElapsedTicks;
            Message = msg;
        }

        public void SetLastSent(T lastSent, Stopwatch timestampStopwatch)
        {
            LastSentTimestamp = timestampStopwatch.ElapsedTicks;
            LastSent = lastSent;
        }
    }
}