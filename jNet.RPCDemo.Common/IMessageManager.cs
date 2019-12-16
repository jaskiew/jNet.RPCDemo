using jNet.RPCDemoClient;
using System;
using System.Collections.Generic;

namespace jNet.RPCDemo
{
    public interface IMessageManager
    {
        IEnumerable<IMessage> Messages { get; }

        IMessage AddMessage(string message);
        bool DelMessage(int messageid);
        event EventHandler<MessageEventArgs> MessageAdded;
        event EventHandler<MessageEventArgs> MessageDeleted;
    }
}