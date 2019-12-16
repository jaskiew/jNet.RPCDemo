using Newtonsoft.Json.Serialization;
using System;

namespace jNet.RPCDemoClient
{
    class ClientTypeNameBinder : ISerializationBinder
    {
        public Type BindToType(string assemblyName, string typeName)
        {
            switch (typeName)
            {
                case "jNet.RPCDemo.Models.MessageManager":
                    return typeof(Proxy.MessageManager);
                case "jNet.RPCDemo.Models.Message":
                    return typeof(Proxy.Message);
                default:
                    return Type.GetType($"{typeName}, {assemblyName}", true);
            }
        }
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            typeName = serializedType.FullName;
            assemblyName = serializedType.Assembly.FullName;
        }
    }
}
