using jNet.RPC;
using jNet.RPC.Client;
using Newtonsoft.Json;
using jNet.RPCDemo;

namespace jNet.RPCDemoClient.Proxy
{
    public class Message : ProxyBase, IMessage
    {

        [JsonProperty(nameof(MessageId))]
        private int _messageId;

        [JsonProperty(nameof(MessageContent))]
        private string _message;


        [JsonIgnore]
        public string MessageContent { get => _message; set => Set(value); }

        [JsonIgnore]
        public int MessageId { get => _messageId; set => Set(value); }

        protected override void OnEventNotification(SocketMessage message)
        {

        }
    }
}
