using jNet.RPC.Server;
using jNet.RPCDemoClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jNet.RPCDemo.Models
{
    [Serializable]
    public class MessageManager : DtoBase, IMessageManager
    {
        public const string SerializationFileName = "PlayersConfig.xml";
        public const string Folder = "PlayerMgr";

        public static MessageManager Current { get; } = new MessageManager();

        public List<Message> _messages { get; set; } = new List<Message>();

        public IEnumerable<IMessage> Messages { get => _messages; }

        public IMessage AddMessage(string _message)
        {
            Message message = new Message();
            message.MessageContent = _message;
            message.MessageId = (_messages.Count > 0 ? _messages.Max(p => p.MessageId) : 0) + 1;
            _messages.Add(message);
            MessageAdded?.Invoke(this, new MessageEventArgs(message));
            return message;
        }

        public bool DelMessage(int _messageid)
        {
            var item = _messages.FirstOrDefault(p => p.MessageId == _messageid);
            if (item == null)
                return false;
            _messages.Remove(item);
            MessageDeleted?.Invoke(this, new MessageEventArgs(item));
            return true;
        }

        public event EventHandler<MessageEventArgs> MessageAdded;
        public event EventHandler<MessageEventArgs> MessageDeleted;

    }
}
