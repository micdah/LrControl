using System.Diagnostics;

namespace LrControl.Core.Midi
{
    public abstract class MessageHolder<T>
        where T : struct
    {
        private T _message;
        private T _lastSent;
        
        protected MessageHolder(in T msg)
        {
            SetMessage(in msg);
        }

        public ref readonly T Message => ref _message;
        public long MessageTimestamp { get; private set; }
        public ref readonly T LastSent => ref _lastSent;
        public long LastSentTimestamp { get; private set; }

        public bool HasChanged => CalculateHasChanged();

        protected abstract bool CalculateHasChanged();

        public void SetMessage(in T msg)
        {
            MessageTimestamp = Stopwatch.GetTimestamp();
            _message = msg;
        }

        public void SetLastSent(in T lastSent)
        {
            LastSentTimestamp = Stopwatch.GetTimestamp();
            _lastSent = lastSent;
        }
    }
}