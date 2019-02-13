# EzSockets
Easy TCP sockets with framing for C#

## Install

Install EzSockets via NuGet:

```
Install-Package EzSockets
```

Or check it out on [nuget.org](https://www.nuget.org/packages/EzSockets/).


## What is EzSockets?

EzSockets provides a slightly improved API for C# TCP sockets:

- Listener class to easily listen on port and accept new connections.
- Framing (sending and reading whole messages with attached size).
- Events-based API.
- Async reading loops for sockets.

Basically it implements the bit of tedious work that C# sockets, which are already pretty great on their own, force you to do.

## How it works

1. Define your event listener class and implement the event handlers you want to react to.
2. On server side: create listener to accept connections from port.
3. On client side: create socket to connect to server.

### Messages Vs Data

*EzSockets* can send raw data (bytes array or string) using the Send() and Read() APIs (+ their 'Async' versions).
However, when using raw Send / Read you need to know the size of your messages on the receiving end upfront, ie you must know how many byte you're expecting.

*Messages* API allow you to send whole messages with their size attached to them. On the receiving end they will be read as a whole, without you needing to know the size.
To use Messages API use the SendMessage / ReadMessage methods.

In addition, you can call StartReadingMessages() to make sockets constantly listen to incoming messages.

Warning: don't mix raw with messages API, or you might get buffers in wrong sizes leading to exceptions.

### Usage - Server Side

The following example will listen to new sockets and react to some of their events:

```cs
// create new server with default event listener and add some events
EzSocketListener server = new EzSocketListener(new EzEventsListener()
{
	OnNewConnectionHandler = (EzSocket socket) => {
		Console.WriteLine("Connected!");
		socket.StartReadingMessages(); // <-- this will make the new socket listen to incoming framed messages.
	},
	OnConnectionClosedHandler = (EzSocket socket) => {
		Console.WriteLine("Closed!");
	},
	OnMessageReadHandler = (EzSocket socket, byte[] data) => {
		Console.WriteLine("Read!");
	},
	OnMessageSendHandler = (EzSocket socket, byte[] data) => {
		Console.WriteLine("Sent!");
	},
	OnExceptionHandler = (EzSocket socket, Exception ex) => {
		Console.WriteLine("Error!");
	}
});

// start listening on port 8080
server.ListenAsync(8080);
```

In the example above we create a server on port 8080 and listen to basic events.
By calling 'StartReadingMessages()' for every new socket, we will read messages automatically and won't need to actively call 'ReadMessage' every time.

To send a message to one of the connected sockets:

```cs
socket.SendMessage("hello world!");
```

Note that once you're using framed message you can't use the regular send / read API, as it will interfere with the buffer sizes that are automatically attached to messages.

### Usage - Client Side

Now lets create a client socket and connect to server:

```cs
// null as ip will use localhost
var socket = new EzSocket(null, 8080, new EzEventsListener()
{
	OnConnectionClosedHandler = (EzSocket sock) =>
	{
		Console.WriteLine("Closed!");
	},
	OnMessageReadHandler = (EzSocket sock, byte[] buff) =>
	{
		Console.WriteLine("Read!");
	},
	OnMessageSendHandler = (EzSocket sock, byte[] data) => {
		Console.WriteLine("Sent Data!");
	},
});

// here we also start reading messages loop
socket.StartReadingMessages();
```

### Implemeting Listener

In the examples above we used the generic `EzEventsListener` object with delegates. 
However, instead of delegates you can implement your own listener by inheriting from it and overriding the event functions that are interesting for you, or implement the base `IEzEventsListener` on your own.

## License

EzSockets is distributed with the MIT license and is free to use for any purpose.