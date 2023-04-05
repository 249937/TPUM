using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TPUM.Client.Data
{
    public abstract class DataAbstract
    {
        private class Data : DataAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            public Data()
            {
            }

            public override void Add(ProductAbstract product)
            {
                if (product == null)
                {
                    throw new ArgumentNullException();
                }
                
                Task addTask = Task.Run(async () =>
                {
                    ClientServer.Communication.Product webSocketProduct = new ClientServer.Communication.Product();
                    webSocketProduct.guid = product.GetGuid();
                    webSocketProduct.name = product.GetName();
                    webSocketProduct.price = product.GetPrice();
                    byte[] serializedData;
                    XmlSerializer serializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, webSocketProduct);
                        serializedData = stream.ToArray();
                    }
                    ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                    commandData.command = ClientServer.Communication.Command.Add;
                    commandData.data = serializedData;
                    ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                    Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                    clientSendTask.Wait(new TimeSpan(0, 0, 10));
                    await webSocketConnectionClient?.DisconnectAsync();
                }
                );
                addTask.Wait();

                OnProductAdded?.Invoke(product);
            }

            public override ProductAbstract Find(string productName)
            {
                if (productName == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    throw new ArgumentException();
                }

                Task getTask = Task.Run(async () =>
                {
                    byte[] serializedData;
                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, productName);
                        serializedData = stream.ToArray();
                    }
                    ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                    commandData.command = ClientServer.Communication.Command.Find;
                    commandData.data = serializedData;
                    ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                    Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                    clientSendTask.Wait(new TimeSpan(0, 0, 10));
                    await webSocketConnectionClient?.DisconnectAsync();
                }
                );
                getTask.Wait();

                return null;
            }

            public override List<ProductAbstract> FindAll(string productName)
            {
                if (productName == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    throw new ArgumentException();
                }

                Task getAllTask = Task.Run(async () =>
                {
                    byte[] serializedData;
                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, productName);
                        serializedData = stream.ToArray();
                    }
                    ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                    commandData.command = ClientServer.Communication.Command.FindAll;
                    commandData.data = serializedData;
                    ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                    Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                    clientSendTask.Wait(new TimeSpan(0, 0, 10));
                    await webSocketConnectionClient?.DisconnectAsync();
                }
                );
                getAllTask.Wait();

                return new List<ProductAbstract>();
            }

            public override void Remove(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                Task removeTask = Task.Run(async () =>
                {
                    byte[] serializedData;
                    XmlSerializer serializer = new XmlSerializer(typeof(Guid));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, productGuid);
                        serializedData = stream.ToArray();
                    }
                    ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                    commandData.command = ClientServer.Communication.Command.Remove;
                    commandData.data = serializedData;
                    ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                    Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                    clientSendTask.Wait(new TimeSpan(0, 0, 10));
                    await webSocketConnectionClient?.DisconnectAsync();
                }
                );
                removeTask.Wait();
            }
        }

        public abstract event Action<ProductAbstract> OnProductAdded;
        public abstract event Action<ProductAbstract> OnProductRemoved;

        public abstract void Add(ProductAbstract product);

        public abstract ProductAbstract Find(string productName);

        public abstract List<ProductAbstract> FindAll(string productName);

        public abstract void Remove(Guid productGuid);

        public static DataAbstract CreateData()
        {
            return new Data();
        }
    }
}
