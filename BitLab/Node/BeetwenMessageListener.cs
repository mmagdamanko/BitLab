using System.Collections.Concurrent;
using System.Threading;
using BitLab.Message;

namespace BitLab.Node
{
    public class BeetwenMessageListener<T> : IMessageListener<T>
    {
        private BlockingCollection<T> _MessageQueue = new BlockingCollection<T>((IProducerConsumerCollection<T>) new ConcurrentQueue<T>());

        public BlockingCollection<T> MessageQueue
        {
            get
            {
                return this._MessageQueue;
            }
        }

        public virtual T ReceiveMessage(CancellationToken cancellationToken = default (CancellationToken))
        {
            return this.MessageQueue.Take(cancellationToken);
        }

        public virtual void PushMessage(T message)
        {
            this._MessageQueue.Add(message);
        }
    }
}