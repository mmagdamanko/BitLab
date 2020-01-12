using System.Net.Sockets;

namespace BitLab.Message
{
    public class IncomingMessage
    {
        public IncomingMessage()
        {
        }

        public IncomingMessage(Payload payload)
        {
            this.Message = new Message();
            this.Message.Payload = payload;
        }

        public Message Message { get; set; }

        internal Socket Socket { get; set; }

        public Node.Node Node { get; set; }

        public long Length { get; set; }
    }
}