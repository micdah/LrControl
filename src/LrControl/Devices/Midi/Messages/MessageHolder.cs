using System.Diagnostics;

namespace LrControl.Devices.Midi.Messages
{
    internal abstract class MessageHolder
    {
        public bool HasChanged => MessageTimestamp != LastSentTimestamp && CalculateHasChanged();
        public long MessageTimestamp { get; protected set; }
        public long LastSentTimestamp { get; protected set; }
        
        public abstract void SendMessage(IMidiEventDispatcher dispatcher);

        protected abstract bool CalculateHasChanged();
    }

    internal abstract class MessageHolder<T> : MessageHolder where T : struct
    {
        private T _message;
        private T _lastSent;

        protected MessageHolder(in T msg)
        {
            SetMessage(in msg);
        }

        protected ref readonly T Message => ref _message;
        protected ref readonly T LastSent => ref _lastSent;

        public override void SendMessage(IMidiEventDispatcher dispatcher)
        {
            ref readonly var msg = ref _message;
            SendMessage(dispatcher, in msg);
            
            LastSentTimestamp = Stopwatch.GetTimestamp();
            _lastSent = msg;
        }

        public void SetMessage(in T msg)
        {
            MessageTimestamp = Stopwatch.GetTimestamp();
            _message = msg;
        }

        protected abstract void SendMessage(IMidiEventDispatcher dispatcher, in T msg);
    }
}