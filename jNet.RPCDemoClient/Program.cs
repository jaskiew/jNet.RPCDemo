using jNet.RPC.Client;
using jNet.RPCDemo;
using System;
using System.Collections.Generic;

namespace jNet.RPCDemoClient
{
    class Program
    {
        static void Main()
        {

            RemoteClient client = new RemoteClient("127.0.0.1:9999") { Binder = new ClientTypeNameBinder() };
            var manager = client.GetRootObject<Proxy.MessageManager>();

            while (true)
            {
                var line = Console.ReadLine();
                var lineParts = line?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (lineParts?.Length >= 1)
                {
                    switch (lineParts[0])
                    {
                        case "list":

                            IEnumerable<IMessage> messages = manager.Messages;
                            foreach (var message in messages)
                            {
                                Console.Write("MessageID: ");
                                Console.WriteLine(message.MessageId);
                                Console.Write("Message: ");
                                Console.WriteLine(message.MessageContent);
                                Console.WriteLine();

                            }
                            break;
                        case "quit":
                            return;
                        case "add":
                            manager.AddMessage(lineParts[1]);
                            break;
                        case "del":
                            manager.DelMessage(int.Parse(lineParts[1]));
                            break;
                        default:
                            Console.WriteLine("\n Couldn't recognize command");
                            Console.WriteLine("\n ? - help");
                            break;
                    }
                }
            }
        }
    }
}
