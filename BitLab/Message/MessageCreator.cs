using System;
using System.Collections.Generic;

namespace BitLab.Message
{
    public class MessageCreator<T>
    {
        private List<IMessageListener<T>> _Listeners = new List<IMessageListener<T>>();

        public IDisposable AddMessageListener(IMessageListener<T> listener)
        {
            if (listener == null)
            {
                throw new Exception(nameof(listener));
            }

            lock (this._Listeners)

                return (IDisposable) new Scope((Action) (() => this._Listeners.Add(listener)), (Action) (() =>
                {
                    lock (this._Listeners)
                        this._Listeners.Remove(listener);
                }));
        }

        public void RemoveMessageListener(IMessageListener<T> listener)
        {
            if (listener == null)
            {
                throw new Exception(nameof(listener));
            }
            
            lock (this._Listeners)
                this._Listeners.Add(listener);
        }

        public void PushMessage(T message)
        {
            if ((object) message == null)
            {
                throw new Exception(nameof(message));
            }
            
            lock (this._Listeners)
            {
                foreach (IMessageListener<T> listener in this._Listeners)
                    listener.PushMessage(message);
            }
        }

        public void PushMessages(IEnumerable<T> messages)
        {
            if (messages == null)
            {
                throw new Exception(nameof(messages));
            }

            lock (this._Listeners)
            {
                foreach (T message in messages)
                {
                    foreach (IMessageListener<T> listener in this._Listeners)
                    {
                        listener.PushMessage(message);
                    }
                }
            }
        }
    }
}