namespace BitLab.Serializator
{
    public interface ISerializable
    {
        void ReadWrite(BitcoinStream stream);
    }
}