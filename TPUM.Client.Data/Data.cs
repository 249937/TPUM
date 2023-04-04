using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPUM.Client.Data
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

    internal static class WebSocketClient
    {
        public static async Task<WebSocketConnection> Connect(Uri uri)
        {
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(uri, CancellationToken.None);
            switch (clientWebSocket.State)
            {
                case WebSocketState.Open:
                    return new ClientWebSocketConnection(clientWebSocket);
                default:
                    throw new WebSocketException();
            }
        }

        private class ClientWebSocketConnection : WebSocketConnection
        {
            private ClientWebSocket clientWebSocket = null;

            public ClientWebSocketConnection(ClientWebSocket clientWebSocket)
            {
                this.clientWebSocket = clientWebSocket;
                Task.Factory.StartNew(() => ClientMessageLoop());
            }

            protected override Task SendTask(string message)
            {
                return clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            public override Task DisconnectAsync()
            {
                OnClose?.Invoke();
                return clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client Disconnect", CancellationToken.None);
            }

            private void ClientMessageLoop()
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                        WebSocketReceiveResult receiveResult = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                        if (receiveResult.MessageType == WebSocketMessageType.Close)
                        {
                            DisconnectAsync().Wait();
                            return;
                        }
                        int count = receiveResult.Count;
                        while (!receiveResult.EndOfMessage)
                        {
                            if (count >= buffer.Length)
                            {
                                OnClose?.Invoke();
                                clientWebSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Buffer Overflow", CancellationToken.None).Wait();
                                return;
                            }
                            segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                            receiveResult = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
                            count += receiveResult.Count;
                        }
                        OnMessage?.Invoke(Encoding.UTF8.GetString(buffer, 0, count));
                    }
                }
                catch (Exception)
                {
                    OnClose?.Invoke();
                    clientWebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None).Wait();
                }
            }
        }
    }
}
