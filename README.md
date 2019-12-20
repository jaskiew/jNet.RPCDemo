# jNet.RPC
This project was created as easy to use Remote Procedure Call (RPC) in client-server object environment.

There are plenty of communication means and frameworks, but almost all of them requires us to communicate with server through keyhole. This library is response to my personal need to have a tool that allow me to have, at client, reflection of all obtained server objects. And use them in a way as if they were local, through network.
So it's possible to:
 - transfer an object from server to client,
 - execute its methods,
 - get and set its properties,
 - subscribe to its events.
 
 Sounds pretty easy?
 Under the hood it's little less, but still relatively simple.
 
 # History
The framework was created as part of [TVPlay](https://github.com/jaskie/PlayoutAutomation) project, a TV play-out automation system. Therefore it does not have a big usage base. But its versatility made me to extract it to separate library, to use it in other projects as well. 

# Limitations and dependencies
Currently it works over TCP connections only (however initially was using WebSockets). It heavily relies on Newtonsoft Json and uses NLog. It was tested only on Windows .NET Framework.

# Usage
To use it, it is reqired that all your persistent server classes were based on `DtoBase`. I know, it's huge claim, but the class makes lot of work for us. It, for example, can notify all clients about changing its property values (using built-in `INotifyPropertyChanged` interface). Such a server class should also implement its interface (i.e. used by application GUI). The same interface must be implemented on client side. 

Corresponding client classes currently should be written manually, but they are generally straightforward, simple implementations, and may be generated automatically in near future. Client classes are derived from `ProxyBase`. 

We'll need also a server `ServerHost` and client `RemoteClient` classes to maintain communication. And mapping between server and client classes, in a class that implements `Newtonsoft.Json.Serialization.ISerializationBinder`. That's all.

An example of usage may be found in [CollaborativeEditor](https://github.com/jaskie/CollaborativeEditor) project.

# jNet.RPC Tutorial

### You can find here informations about:

- setting up jNet.RPC
- subscribing events
- executing methods
- getting and setting properties


## Setting Up

### NuGet Package
You can find package on [www.nuget.org](https://www.nuget.org/packages/jNet.RPC) an check current version.

Installation

- Packgage Manager:

	`PM> Install-Package jNet.RPC -Version 0.1.7283.22640`

- via VS [Package Managment window](https://docs.microsoft.com/pl-pl/nuget/consume-packages/install-use-packages-visual-studio) 

- .NET CLI:

	`> dotnet add package jNet.RPC --version 0.1.7283.22640`

- PackageReference:

	`<PackageReference Include="jNet.RPC" Version="0.1.7283.22640" /`>

- Paket CLI:

	`> paket add jNet.RPC --version 0.1.7283.22640`


### Server
Start with *using directive* and create instance of ***ServerHost()*** declaring *ListenPort* at once.
```C#
    using jNet.RPC.Server;	

	//...
    var host = new ServerHost() { ListenPort = 9999 };
```
Initialize ***ServerHost()*** passing in parameters:

- DtoBase -> Root data model
- Principal -> Enter principal or use Default from jNet.RPC.Server.PrincipalProvider

for exapmle:
```C#
    host.Initialize(Project.Models.Model.Current, jNet.RPC.Server.PrincipalProvider.Default);
```
### Client

Start with *using directive*

```C#
    using jNet.RPC.Client;
```
Create instace of ***RemoteHost*** pointing at **adress** of *ServerHost* and it's **port**, it is convinient to get *rootObject* right away.
```C#
    RemoteClient client = new RemoteClient("127.0.0.1:9999") { Binder = new ClientTypeNameBinder() };
    var rootObject = client.GetRootObject<Proxy.Model>();
```
To recive data from server ***ClientTypeNameBinder*** class will be needed to resolve which datatype is requied.
```C#
	using Newtonsoft.Json.Serialization;
	using System;
	
	namespace Project
	{
	    class ClientTypeNameBinder : ISerializationBinder
	    {
	        public Type BindToType(string assemblyName, string typeName)
	        {
	            switch (typeName)
	            {
	                case "Project.Models.Model":
	                    return typeof(Proxy.Model);
	                case "Project.Models.AnotherModel":
	                    return typeof(Proxy.AnotherModel);
					//.....
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
```
### Subscribing Events

You can recive and send notfications between Client and Server.
Subscribe exent in `Proxy.Model`
```C#
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
```
On Server add event
```C#
	public event EventHandler<ModelEventArgs> ModelAdded;
```
and send notification on change of object/variable on Your client

```C#
	public Model AddModel(int _parameter)
	{
		Model exampleModel = new Model();
		exampleModel.PropertyOne = _parameter;
		ModelAdded?.Invoke(this, new ModelEventArgs(item));
		//.....
		return exampleModel;
	}
```
### Executing Methods

There is two ways od executing methods:
 - Query -> for functions that return values
 - Invoke -> if function is void

```C#
public IMessage AddMessage(string message)
        {
            return Query<Message>(parameters: new object[] { message });
        }
        public bool DelMessage(int messageid)
        {
            return Query<bool>(parameters: new object[] { messageid });
        }
    
```

### Getting and setting properties

Set is ProxyBase method that sets and send immidiatly EventNotofication. 
Attribute `JsonProperty(...)` and eventually`JsonIgnore` decide which data send and which store only locally.


```C#
 	public class Message : ProxyBase, IMessage
	    {
	
	         [JsonProperty(nameof(MessageContent))]
	        public string MessageContent { get; set; }
	
	        [JsonProperty(nameof(MessageId))]
	        public int MessageId { get; set; }
	
	        protected override void OnEventNotification(SocketMessage message)
	        {
				//Must exist because of ProxyBase
	        }
	    }
```