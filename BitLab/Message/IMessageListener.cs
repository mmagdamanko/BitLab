namespace BitLab.Message
{
    public interface IMessageListener<in T>
    {
        void PushMessage(T message);
    }
}