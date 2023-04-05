using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TPUM.Server.Presentation
{
    public abstract class WebSocketConnection
    {
        public virtual Action<ClientServer.Communication.CommandData> OnMessage
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

    internal class Server
    {
        static void Main(string[] args)
        {
            WebSocketConnection webSocketConnectionServer = null;
            Uri uri = new Uri("ws://localhost:1337");

            Logic.ShopServiceAbstract shopService = Logic.ShopServiceAbstract.CreateShopService();

            Task serverTask = Task.Run(async () =>
                {
                    await ServerMainLoop(uri.Port, webSocketConnection =>
                        {
                            webSocketConnectionServer = webSocketConnection;
                            webSocketConnectionServer.OnMessage = (commandData) =>
                            {
                                if (ClientServer.Communication.Command.Add.Equals(commandData.command))
                                {
                                    Console.WriteLine("[COMMAND] ADD");
                                    ClientServer.Communication.Product receivedProduct;
                                    XmlSerializer serializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                                    using (MemoryStream stream = new MemoryStream(commandData.data))
                                    {
                                        receivedProduct = (ClientServer.Communication.Product)serializer.Deserialize(stream);
                                    }
                                    Console.WriteLine("Trying to add product with GUID: " + receivedProduct.guid + ", name: " + receivedProduct.name + ", price: " + receivedProduct.price);
                                    shopService.AddProduct(receivedProduct.name, receivedProduct.price);
                                }
                                else if (ClientServer.Communication.Command.Get.Equals(commandData.command))
                                {
                                    Console.WriteLine("[COMMAND] FIND");
                                    string receivedName;
                                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                                    using (MemoryStream stream = new MemoryStream(commandData.data))
                                    {
                                        receivedName = (string)serializer.Deserialize(stream);
                                    }
                                    Console.WriteLine("Trying to find product with name: " + receivedName);
                                    Logic.ProductAbstract foundProduct = shopService.FindProduct(receivedName);
                                }
                                else if (ClientServer.Communication.Command.GetAll.Equals(commandData.command))
                                {
                                    Console.WriteLine("[COMMAND] FIND");
                                    string receivedName;
                                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                                    using (MemoryStream stream = new MemoryStream(commandData.data))
                                    {
                                        receivedName = (string)serializer.Deserialize(stream);
                                    }
                                    Console.WriteLine("Trying to find all products with name: " + receivedName);
                                    List<Logic.ProductAbstract> foundProducts = shopService.FindProducts(receivedName);
                                }
                                else if (ClientServer.Communication.Command.Remove.Equals(commandData.command))
                                {
                                    Console.WriteLine("[COMMAND] REMOVE");
                                    Guid receivedGuid;
                                    XmlSerializer serializer = new XmlSerializer(typeof(Guid));
                                    using (MemoryStream stream = new MemoryStream(commandData.data))
                                    {
                                        receivedGuid = (Guid)serializer.Deserialize(stream);
                                    }
                                    Console.WriteLine("Trying to remove product with GUID: " + receivedGuid);
                                    shopService.RemoveProduct(receivedGuid);
                                }
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
