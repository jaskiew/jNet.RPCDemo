using jNet.RPC.Server;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace jNet.RPCDemo
{
    class Program
    {
        static void ConfigureLogger()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);
            LogManager.Configuration = config;
        }

        static void Main()
        {
            ConfigureLogger();
            var host = new ServerHost() { ListenPort = 9999 };
            host.Initialize(jNet.RPCDemo.Models.MessageManager.Current, jNet.RPC.Server.PrincipalProvider.Default);
            while (true)
            {
                {
                    var message = jNet.RPCDemo.Models.MessageManager.Current;
                    while (true)
                    {
                        var line = Console.ReadLine();
                        var lineParts = line?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (lineParts?.Length >= 1)
                        {
                            switch (lineParts[0])
                            {
                                case "list":
                                    {
                                        var messageslist = message.Messages;
                                        foreach (var players in messageslist)
                                        {
                                            Console.Write("MessageID: ");
                                            Console.WriteLine(players.MessageId);
                                            Console.Write("Message: ");
                                            Console.WriteLine(players.MessageContent);
                                            Console.WriteLine();

                                        }
                                        break;
                                    }
                                case "quit":
                                    return;
                                case "add":
                                    message.AddMessage(lineParts[1]);
                                    break;
                                case "del":
                                    message.DelMessage(int.Parse(lineParts[1]));
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
    }
}
