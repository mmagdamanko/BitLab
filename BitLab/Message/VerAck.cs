using BitLab.Serializator;

namespace BitLab.Message
{
    [Payload("verack")]
    public class VerAck : Payload, ISerializable
    {
        public override void ReadWrite(BitcoinStream stream)
        {
        }
    }
}