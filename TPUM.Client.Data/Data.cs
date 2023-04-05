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
                
                try
                {
                    Task addTask = Task.Run(async () =>
                    {
                        ClientServer.Communication.Product webSocketProduct = new ClientServer.Communication.Product();
                        webSocketProduct.guid = product.GetGuid();
                        webSocketProduct.name = product.GetName();
                        webSocketProduct.price = product.GetPrice();
                        byte[] serializedData;
                        XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            productSerializer.Serialize(stream, webSocketProduct);
                            serializedData = stream.ToArray();
                        }
                        ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                        commandData.command = ClientServer.Communication.Command.Add;
                        commandData.data = serializedData;
                        ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        webSocketConnectionClient.OnMessage = (receivedCommandData) =>
                        {
                            ClientServer.Communication.Product receivedProduct;
                            using (MemoryStream stream = new MemoryStream(receivedCommandData.data))
                            {
                                receivedProduct = (ClientServer.Communication.Product)productSerializer.Deserialize(stream);
                            }
                            if (receivedProduct != null)
                            {
                                OnProductAdded?.Invoke(new Product(receivedProduct.guid, receivedProduct.name, receivedProduct.price));
                            }
                        };
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                    );
                    addTask.Wait();
                }
                catch (Exception)
                {
                }
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

                ProductAbstract foundProduct = null;

                try
                {
                    Task findTask = Task.Run(async () =>
                    {
                        byte[] serializedData;
                        XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            stringSerializer.Serialize(stream, productName);
                            serializedData = stream.ToArray();
                        }
                        ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                        commandData.command = ClientServer.Communication.Command.Find;
                        commandData.data = serializedData;
                        ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        webSocketConnectionClient.OnMessage = (receivedCommandData) =>
                        {
                            ClientServer.Communication.Product receivedProduct;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream(receivedCommandData.data))
                            {
                                receivedProduct = (ClientServer.Communication.Product)productSerializer.Deserialize(stream);
                            }
                            if (receivedProduct != null)
                            {
                                foundProduct = new Product(receivedProduct.guid, receivedProduct.name, receivedProduct.price);
                            }
                        };
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                    );
                    findTask.Wait();
                }
                catch (Exception)
                {
                }

                return foundProduct;
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

                List<ProductAbstract> foundProducts = new List<ProductAbstract>();

                try
                {
                    Task findAllTask = Task.Run(async () =>
                    {
                        byte[] serializedData;
                        XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            stringSerializer.Serialize(stream, productName);
                            serializedData = stream.ToArray();
                        }
                        ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                        commandData.command = ClientServer.Communication.Command.FindAll;
                        commandData.data = serializedData;
                        ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        webSocketConnectionClient.OnMessage = (receivedCommandData) =>
                        {
                            ClientServer.Communication.Product receivedProduct;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream(receivedCommandData.data))
                            {
                                receivedProduct = (ClientServer.Communication.Product)productSerializer.Deserialize(stream);
                            }
                            if (receivedProduct != null)
                            {
                                foundProducts.Add(new Product(receivedProduct.guid, receivedProduct.name, receivedProduct.price));
                            }
                        };
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                    );
                    findAllTask.Wait();
                }
                catch (Exception)
                {
                }

                return foundProducts;
            }

            public override void Remove(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                try
                {
                    Task removeTask = Task.Run(async () =>
                    {
                        byte[] serializedData;
                        XmlSerializer guidSerializer = new XmlSerializer(typeof(Guid));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            guidSerializer.Serialize(stream, productGuid);
                            serializedData = stream.ToArray();
                        }
                        ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                        commandData.command = ClientServer.Communication.Command.Remove;
                        commandData.data = serializedData;
                        ClientServer.Communication.WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        webSocketConnectionClient.OnMessage = (receivedCommandData) =>
                        {
                            ClientServer.Communication.Product receivedProduct;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream(receivedCommandData.data))
                            {
                                receivedProduct = (ClientServer.Communication.Product)productSerializer.Deserialize(stream);
                            }
                            if (receivedProduct != null)
                            {
                                OnProductRemoved?.Invoke(new Product(receivedProduct.guid, receivedProduct.name, receivedProduct.price));
                            }
                        };
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                    );
                    removeTask.Wait();
                }
                catch (Exception)
                {
                }
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
