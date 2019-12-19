using jNet.RPC;
using jNet.RPC.Client;
using Newtonsoft.Json;
using jNet.RPCDemo;

namespace jNet.RPCDemoClient.Proxy
{
    public class Message : ProxyBase, IMessage
    {

        [JsonProperty(nameof(MessageContent))]
        public string MessageContent { get; set; }

        [JsonProperty(nameof(MessageId))]
        public int MessageId { get; set; }

        protected override void OnEventNotification(SocketMessage message)
        {

        }
    }
}
