using jNet.RPC.Server;
using Newtonsoft.Json;
using jNet.RPCDemo;

namespace jNet.RPCDemo.Models
{
    public class Message : DtoBase, IMessage
    {
        [JsonProperty]
        public string MessageContent { get; set; }
        [JsonProperty]
        public int MessageId { get; set; }
    }
}
