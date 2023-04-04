using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPUM.Server.Presentation
{
    public abstract class WebSocketConnection
    {
        public virtual Action<string> OnMessage
        {
            set;
            protected get;
        } = message => { };

        public virtual Action OnClose
        {
            set;
            protected get;
        } = () => { };

        public virtual Action OnError
        {
            set;
            protected get;
        } = () => { };
        
        public async Task SendAsync(string message)
        {
            await SendTask(message);
        }
        
        public abstract Task DisconnectAsync();

        protected abstract Task SendTask(string message);
    }

    internal class ServerWebSocketConnection : WebSocketConnection
    {
        private WebSocket webSocket = null;

        public ServerWebSocketConnection(WebSocket webSocket)
        {
            this.webSocket = webSocket;
            Task.Factory.StartNew(() => ServerMessageLoop());
        }

        protected override Task SendTask(string message)
        {
            return webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public override Task DisconnectAsync()
        {
            OnClose?.Invoke();
            return webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server Disconnect", CancellationToken.None);
        }

        private void ServerMessageLoop()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                ArraySegment<byte> segments = new ArraySegment<byte>(buffer);
                WebSocketReceiveResult receiveResult = webSocket.ReceiveAsync(segments, CancellationToken.None).Result;
                if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    DisconnectAsync();
                    return;
                }
                int count = receiveResult.Count;
                while (!receiveResult.EndOfMessage)
                {
                    if (count >= buffer.Length)
                    {
                        OnClose?.Invoke();
                        webSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Buffer Overflow", CancellationToken.None);
                        return;
                    }
                    segments = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                    receiveResult = webSocket.ReceiveAsync(segments, CancellationToken.None).Result;
                    count += receiveResult.Count;
                }
                OnMessage?.Invoke(Encoding.UTF8.GetString(buffer, 0, count));
            }
        }
    }

    internal class Server
    {
        static void Main(string[] args)
        {
            WebSocketConnection webSocketConnectionServer = null;
            Uri uri = new Uri("ws://localhost:1337");

            Task serverTask = Task.Run(async () =>
                {
                    await ServerMainLoop(uri.Port, webSocketConnection =>
                        {
                            webSocketConnectionServer = webSocketConnection;
                            webSocketConnectionServer.OnMessage = (data) =>
                            {
                                Console.WriteLine(data);
                            };
                        }
                    );
                    await webSocketConnectionServer?.DisconnectAsync();
                }
            );

            serverTask.Wait();
        }

        private static async Task ServerMainLoop(int port, Action<WebSocketConnection> OnConnection)
        {
            Uri uri = new Uri($@"http://localhost:{port}/");
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(uri.ToString());
            httpListener.Start();
            while (true)
            {
                HttpListenerContext httpListenerContext = await httpListener.GetContextAsync();
                if (!httpListenerContext.Request.IsWebSocketRequest)
                {
                    httpListenerContext.Response.StatusCode = 400;
                    httpListenerContext.Response.Close();
                }
                HttpListenerWebSocketContext httpListenerWebSocketContext = await httpListenerContext.AcceptWebSocketAsync(null);
                OnConnection?.Invoke(new ServerWebSocketConnection(httpListenerWebSocketContext.WebSocket));
            }
        }
    }
}
