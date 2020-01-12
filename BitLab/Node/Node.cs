using System;
using System.Threading;
using System.Threading.Tasks;
using BitLab.Message;

namespace BitLab.Node
{
    public class Node : IDisposable
    {
        private readonly MessageCreator<IncomingMessage> _messageCreator = new MessageCreator<IncomingMessage>();
        private NodeStatus _status = NodeStatus.Offline;
        
        
        public NodeStatus Status
        {
            get
            {
                return this._status;
            }
            private set
            {
                Console.WriteLine($"Status się zmienił z {_status} na {value}");

                NodeStatus status = this._status;
                this._status = value;
                
                if (status == this._status)
                {
                    return;
                }
                
                if (value != NodeStatus.Failed && value != NodeStatus.Offline)
                {
                    return;
                }

                Console.WriteLine("POLACZENIE ZERWANE !!!!");
            }
        }

        /// <summary>
        /// Metoda która wyświetla komunikat podczas usunięcia node'a
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("Node został zniszczony");
        }
        
        
        /// <summary>Wyślij wiadomość do peera asynchronicznie</summary>
        /// <param name="payload">Payload do wysłánia</param>
        /// <param name="System.OperationCanceledException.OperationCanceledException">Połączenie z nodem zostało zerwane</param>
        public Task SendMessage(Payload payload)
        {
            if (payload == null)
            {
                throw new Exception(nameof (payload));
            }
            
            TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            
            if (!this.IsConnected)
            {
                completion.SetException((Exception) new OperationCanceledException("Połączenie z peerem zostało zerwane"));
                return (Task) completion.Task;
            }
            
            return (Task) completion.Task;
        }
        
        public void VersionHandshake(NodeRequirement requirements, CancellationToken cancellationToken = default(CancellationToken))
        {
            requirements = requirements ?? new NodeRequirement();
            using (var listener = CreateListener()
                .Where(p => p.Message.Payload is Version ||
                            p.Message.Payload is RejectPayload ||
                            p.Message.Payload is VerAckPayload))
            {

                SendMessageAsync(MyVersion);
                var payload = listener.ReceivePayload<Payload>(cancellationToken);
                if (payload is RejectPayload)
                {
                    throw new ProtocolException("Handshake rejected : " + ((RejectPayload)payload).Reason);
                }
                var version = (VersionPayload)payload;
                _PeerVersion = version;
                SetVersion(Math.Min(MyVersion.Version, version.Version));

                if (!version.AddressReceiver.Address.Equals(MyVersion.AddressFrom.Address))
                    Logs.NodeServer.LogWarning("Different external address detected by the node {addressReceiver} instead of {addressFrom}", version.AddressReceiver.Address, MyVersion.AddressFrom.Address);

                if (ProtocolCapabilities.PeerTooOld)
                {
                    Logs.NodeServer.LogWarning("Outdated version {version} disconnecting", version.Version);
                    Disconnect("Outdated version");
                    return;
                }

                if (!requirements.Check(version, ProtocolCapabilities))
                {
                    Disconnect("The peer does not support the required services requirement");
                    return;
                }

                SendMessageAsync(new VerAckPayload());
                listener.ReceivePayload<VerAckPayload>(cancellationToken);
                State = NodeState.HandShaked;
                if (Advertize && MyVersion.AddressFrom.Address.IsRoutable(true))
                {
                    SendMessageAsync(new AddrPayload(new NetworkAddress(MyVersion.AddressFrom)
                    {
                        Time = DateTimeOffset.UtcNow
                    }));
                }

            }
        }
        
        /// <summary>
        /// Tworzymy obiekt który będzie kolejkował wiadomości póki nie zniknie
        /// </summary>
        /// <returns>Zwraca obiekt który nasłuchuje danego node'a</returns>
        public NodeListener CreateListener()
        {
            return new NodeListener(this);
        }
        
        /// <summary>
        /// Property o statusie połączenia z nodem
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (this.Status != NodeStatus.Connected)
                {
                    Console.WriteLine("Udało się połączyć z nodem");
                    return this.Status == NodeStatus.HandShaked;
                }
                
                return true;
            }
        }
        
        public MessageCreator<IncomingMessage> MessageCreator
        {
            get
            {
                return this._messageCreator;
            }
        }
    }
}