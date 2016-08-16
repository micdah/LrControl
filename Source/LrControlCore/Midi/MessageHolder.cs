using System.Diagnostics;

namespace LrControlCore.Midi
{
    public abstract class MessageHolder<T> where T : class
    {
        protected MessageHolder(T msg)
        {
            SetMessage(msg);
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

        public void SetMessage(T msg)
        {
            MessageTimestamp = Stopwatch.GetTimestamp();
            Message = msg;
        }

        public void SetLastSent(T lastSent)
        {
            LastSentTimestamp = Stopwatch.GetTimestamp();
            LastSent = lastSent;
        }
    }
}