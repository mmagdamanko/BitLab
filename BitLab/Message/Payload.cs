using BitLab.Serializator;

namespace BitLab.Message
{
    public class Payload
    {
        public virtual void ReadWrite(BitcoinStream stream)
        {
        }
        
        public virtual string Command
        {
            get
            {
                return PayloadAttribute.GetCommandName(this.GetType());
            }
        }
        
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}