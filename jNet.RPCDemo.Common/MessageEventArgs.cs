using jNet.RPCDemo;
using System;

namespace jNet.RPCDemoClient
{
    public class MessageEventArgs : EventArgs
    {
        public IMessage Message { get; }
        public MessageEventArgs() { }
        public MessageEventArgs(IMessage message)
        {
            Message = message;
        }
    }
}
