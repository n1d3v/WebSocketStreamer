# WebSocketStreamer
WebSockets for Windows Phone 8.1 (WinRT), built from the ground up and based on Windows.Networking.Sockets.
# Why would you make this?
I wanted to have a simple WebSocket wrapper system for my project and I didn't want to gatekeep this only to that project, so instead I built a DLL file that does the wrapping for me and now I can use it everywhere.
# How do I use this?
You will need to put these 4 variables at the top of your class:

```cs
using WebSocketStreamer;

private WebSocketStreamerClient _client; // The main WebSocket client
private WebSocketStreamerSend _sender; // The sender for the WebSocket client, so you can send data
private WebSocketStreamerReceive _receiver; // The receiver for the WebSocket client, so you can receive data
private WebSocketStreamerErrorHandler _errorHandler; // An error handler for the WebSocket client, not really important
```

And then you can use the WebSocket client like this (pseudo-code):

```cs
using System.Diagnostics;

// Creates a WebSocket client with a specific URL.
_client = new WebSocketStreamerClient("wss://example.com");

// Adds a header when connecting, so you can add origins, etc.
// Remember to always do this before the WebSocket connects!
_client.AddHeader("Origin", "https://example.com");

// This is to receive data and use it later in a function.
// In this case, the function is HandleMessage.
_client.MessageReceived += HandleMessage;

// Connects the WebSocket client to the URL you used earlier.
await _client.Connect();

// Attaches both the error handler and the sender to the client for use later.
_errorHandler = new WebSocketStreamerErrorHandler(_client);
_sender = new WebSocketStreamerSend(_client.Socket);

Debug.WriteLine("Connected to the WebSocket and all handlers have been attached.");
```

To send data over the connection, you can use this:

```cs
await _sender.SendText("Hello from WebSocketStreamer!");
```

To attempt reconnecting to the WebSocket URL, you can use this:

```cs
// _client is your WebSocket client, obviously
// maxRetries controls how many reconnection attempts you should have
// baseDelayMS controls the initial delay of reconnecting in miliseconds
var reconnectionHandler = new WebSocketStreamerReconnection(_client, maxRetries: 5, baseDelayMs: 2000);
```

If you need anymore information than that, you can take a look through the source code.

Also, this is my first time writing something like this so if you have any suggestions, please submit a pull request with your changes!