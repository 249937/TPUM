using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TPUM.Client.Data
{
    public abstract class ProductRepositoryAbstract
    {
        private class ProductRepository : ProductRepositoryAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            private List<ProductAbstract> products;

            public ProductRepository()
            {
                products = new List<ProductAbstract>();
            }

            public override void Add(ProductAbstract product)
            {
                if (product == null)
                {
                    throw new ArgumentNullException();
                }
                foreach (ProductAbstract existingProduct in products)
                {
                    if (existingProduct.GetGuid() == product.GetGuid())
                    {
                        return;
                    }
                }
                products.Add(product);
                OnProductAdded?.Invoke(product);

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
                        WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                );
                addTask.Wait();
            }

            public override ProductAbstract Get(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                Task getTask = Task.Run(async () =>
                    {
                        byte[] serializedData;
                        XmlSerializer serializer = new XmlSerializer(typeof(Guid));
                        using (MemoryStream stream = new MemoryStream())
                        {
                            serializer.Serialize(stream, productGuid);
                            serializedData = stream.ToArray();
                        }
                        ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                        commandData.command = ClientServer.Communication.Command.Get;
                        commandData.data = serializedData;
                        WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                );
                getTask.Wait();

                foreach (ProductAbstract product in products)
                {
                    if (product.GetGuid() == productGuid)
                    {
                        return product;
                    }
                }
                return null;
            }

            public override List<ProductAbstract> GetAll()
            {
                Task removeTask = Task.Run(async () =>
                    {
                        ClientServer.Communication.CommandData commandData = new ClientServer.Communication.CommandData();
                        commandData.command = ClientServer.Communication.Command.GetAll;
                        commandData.data = null;
                        WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                );
                removeTask.Wait();

                return products;
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
                        WebSocketConnection webSocketConnectionClient = await WebSocketClient.Connect(new Uri("ws://localhost:1337"));
                        Task clientSendTask = webSocketConnectionClient.SendAsync(commandData);
                        clientSendTask.Wait(new TimeSpan(0, 0, 10));
                        await webSocketConnectionClient?.DisconnectAsync();
                    }
                );
                removeTask.Wait();

                for (int i = products.Count - 1; i >= 0; --i)
                {
                    if (productGuid.Equals(products[i].GetGuid()))
                    {
                        ProductAbstract product = products[i];
                        products.RemoveAt(i);
                        OnProductRemoved?.Invoke(product);
                    }
                }
            }
        }

        public abstract event Action<ProductAbstract> OnProductAdded;
        public abstract event Action<ProductAbstract> OnProductRemoved;

        public abstract void Add(ProductAbstract product);

        public abstract ProductAbstract Get(Guid productGuid);

        public abstract List<ProductAbstract> GetAll();

        public abstract void Remove(Guid productGuid);

        public static ProductRepositoryAbstract CreateProductRepository()
        {
            return new ProductRepository();
        }
    }
}
