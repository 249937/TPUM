using System;
using System.Net.WebSockets;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace TPUM.Server.Presentation
{
    internal class WebSocketServer
    {
        internal static async Task ServerMainLoop(int port, Action<ClientServer.Communication.WebSocketConnection> OnConnection)
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

        internal class ServerWebSocketConnection : ClientServer.Communication.WebSocketConnection
        {
            private WebSocket webSocket = null;

            public ServerWebSocketConnection(WebSocket webSocket)
            {
                this.webSocket = webSocket;
                Task.Factory.StartNew(() => ServerMessageLoop());
            }

            protected override Task SendTask(ClientServer.Communication.CommandData commandData)
            {
                byte[] serializedData;
                XmlSerializer serializer = new XmlSerializer(typeof(ClientServer.Communication.CommandData));
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, commandData);
                    serializedData = stream.ToArray();
                }
                return webSocket.SendAsync(new ArraySegment<byte>(serializedData), WebSocketMessageType.Text, true, CancellationToken.None);
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
                    ClientServer.Communication.CommandData receivedCommandData = new ClientServer.Communication.CommandData();
                    XmlSerializer serializer = new XmlSerializer(typeof(ClientServer.Communication.CommandData));
                    using (MemoryStream stream = new MemoryStream(buffer))
                    {
                        receivedCommandData = (ClientServer.Communication.CommandData)serializer.Deserialize(stream);
                    }
                    OnMessage?.Invoke(receivedCommandData);
                }
            }
        }
    }
}
