# EzSockets
Easy TCP sockets with message framing for C#

## Install

Install EzSockets via NuGet:

```
Install-Package EzSockets
```

Or check it out on [nuget.org](https://www.nuget.org/packages/EzSockets/).


## What is EzSockets?

EzSockets provides a slightly improved API for C# TCP sockets:

- Listener class to listen on port and accept new connections.
- Framing (sending and reading messages as whole, with size attached).
- Events-driven API.
- Async reading loops to automatically read messages.

Basically it implements the bit of tedious work that C# sockets (which are already pretty great on their own) force you to do.

## How you use it

1. Implement event listeners for client and server side (where you put your logic).
2. On server side: create listener to accept connections from port.
3. On client side: create sockets to connect to server.

That's pretty much it. You can use *EzSockets* just like you would with regular TCP client, but the real fun is using them from the events API, sort of like with socket.io.

### Framed Messages Vs Raw Data

Raw API is the basic sockets API that allow you to send streams of bytes to the other side (from byte array or string). 
The problem with this method is that it has no framing and if you send two different messages in a row (for example "hello " and "world") the receiving side might get it in a single read, as "hello world", which is not always desired..

The *Framed Messages* API allow you to send and read whole messages without knowing their size. With this API if the remote device sent you "hello " and then "world", you will get two messages containing "hello " and "world" separately.
Messages API comes with another feature, StartReadingMessages(), which will make a socket listen to incoming traffic and generate 'Message read' events for every new message arrived.

Note: mixing raw data with framed messages is not recommended and can cause unexpected behavior, if you don't read your raw data properly on the receiving end. 


### Usage - Server Side Example

The following is a simple 'echo' server that also send "hello!" on connection:

```cs
// create new server with default event listener and add some events
EzSocketListener server = new EzSocketListener(new EzEventsListener()
{
	OnNewConnectionHandler = (EzSocket socket) => 
	{
		Console.WriteLine("Connected!");
		socket.SendMessage("hello!");
		socket.StartReadingMessages(); // <-- this will make the new socket listen to incoming messages and trigger events.
	},
	OnConnectionClosedHandler = (EzSocket socket) => 
	{
		Console.WriteLine("Connection Closed!");
	},
	OnMessageReadHandler = (EzSocket socket, byte[] data) => 
	{
		Console.WriteLine("Read message!");
		socket.SendMessage(data);
	},
	OnMessageSendHandler = (EzSocket socket, byte[] data) => 
	{
		Console.WriteLine("Sent message!");
	},
	OnExceptionHandler = (EzSocket socket, Exception ex) => 
	{
		Console.WriteLine("Error! " + ex.ToString());
		return ExceptionHandlerResponse.CloseSocket;
	}
});

// start listening on port 8080
server.ListenAsync(8080);
```


### Usage - Client Side Example

Now lets create a client socket and connect to our echo server:

```cs
// null as ip will use localhost
var socket = new EzSocket(null, 8080, new EzEventsListener()
{
	OnConnectionClosedHandler = (EzSocket sock) =>
	{
		Console.WriteLine("Connection Closed!");
	},
	OnMessageReadHandler = (EzSocket sock, byte[] buff) =>
	{
		Console.WriteLine("Read message!");
	},
	OnMessageSendHandler = (EzSocket sock, byte[] data) => 
	{
		Console.WriteLine("Sent Data!");
	},
});

// here we also start reading messages loop
socket.StartReadingMessages();

// send data to server
socket.SendMessage("How are you today?");
```

### Implemeting Events Listener As Class

In the examples above we used the generic `EzEventsListener` object with attached functions. 

If you want to have a 'cleaner' solution with your own server manager class you can create your own listener by implementing the ```IEzEventsListener``` API, or you can inherit from ```EzEventsListener``` and override the handlers that are interesting for you (in the second option you can enjoy both callbacks you can attach and class methods to handle events).

### Change Encoding

By default when converting between string and bytes array *EzSockets* will use UTF-8. To change this behavior, you can override the following static methods:

```cs
EzSocket.BytesToString = (byte[] bytes) => { /* your implementation here */ };
EzSocket.StringToBytes = (string str) => { /* your implementation here */ };
```

## License

EzSockets is distributed with the MIT license and is free to use for any purpose.