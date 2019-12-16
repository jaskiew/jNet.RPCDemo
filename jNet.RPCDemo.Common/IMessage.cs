namespace jNet.RPCDemo
{
    public interface IMessage
    {
        int MessageId { get; set; }
        string MessageContent { get; set; }
    }
}