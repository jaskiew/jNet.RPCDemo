using jNet.RPC;
using jNet.RPC.Client;
using jNet.RPCDemo;
using System;
using System.Collections.Generic;


namespace jNet.RPCDemoClient.Proxy
{
    public class MessageManager : ProxyBase, IMessageManager
    {
        public IEnumerable<IMessage> Messages => Get<List<Message>>();

        public IMessage AddMessage(string message)
        {
            return Query<Message>(parameters: new object[] { message });
        }

        public bool DelMessage(int messageid)
        {
            return Query<bool>(parameters: new object[] { messageid });
        }

        public event EventHandler<MessageEventArgs> MessageAdded
        {
            add
            {
                EventAdd(_messageAdded);
                _messageAdded += value;
            }
            remove
            {
                _messageAdded -= value;
                EventRemove(_messageAdded);
            }
        }
        public event EventHandler<MessageEventArgs> MessageDeleted
        {
            add
            {
                EventAdd(_messageDeleted);
                _messageDeleted += value;
            }
            remove
            {
                _messageDeleted -= value;
                EventRemove(_messageDeleted);
            }
        }
       
        private event EventHandler<MessageEventArgs> _messageAdded;
        private event EventHandler<MessageEventArgs> _messageDeleted;

        protected override void OnEventNotification(SocketMessage sMessage)
        {
            switch (sMessage.MemberName)
            {
                case nameof(IMessageManager.MessageAdded):
                    _messageAdded?.Invoke(this, Deserialize<MessageEventArgs>(sMessage));
                    break;
                case nameof(IMessageManager.MessageDeleted):
                    _messageDeleted?.Invoke(this, Deserialize<MessageEventArgs>(sMessage));
                    break;
            }
        }
    }
}
