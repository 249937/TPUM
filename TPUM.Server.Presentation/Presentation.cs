using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TPUM.Server.Presentation
{
    internal class Presentation
    {
        private static void Main(string[] args)
        {
            StartServer();
        }

        internal static void StartServer()
        {
            ClientServer.Communication.WebSocketConnection webSocketConnectionServer = null;
            Uri uri = new Uri("ws://localhost:1337");

            Logic.ShopServiceAbstract shopService = Logic.ShopServiceAbstract.CreateShopService();

            Task serverTask = Task.Run(async () =>
            {
                await WebSocketServer.ServerMainLoop(uri.Port, webSocketConnection =>
                {
                    webSocketConnectionServer = webSocketConnection;
                    webSocketConnectionServer.OnMessage = (commandData) =>
                    {
                        if (ClientServer.Communication.Command.Add.Equals(commandData.command))
                        {
                            Console.WriteLine("[COMMAND] ADD");
                            ClientServer.Communication.Product receivedProduct;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream(commandData.data))
                            {
                                receivedProduct = (ClientServer.Communication.Product)productSerializer.Deserialize(stream);
                            }
                            Console.WriteLine("Trying to add product with name: " + receivedProduct.name + ", price: " + receivedProduct.price);
                            shopService.AddProduct(receivedProduct.name, receivedProduct.price);
                            ClientServer.Communication.CommandData responseData;
                            responseData.command = ClientServer.Communication.Command.Add;
                            using (MemoryStream stream = new MemoryStream())
                            {
                                productSerializer.Serialize(stream, receivedProduct);
                                responseData.data = stream.ToArray();
                            }
                            Task serverSendTask = webSocketConnectionServer.SendAsync(responseData);
                            serverSendTask.Wait(new TimeSpan(0, 0, 10));
                            Console.WriteLine("Added product with name: " + receivedProduct.name + ", price: " + receivedProduct.price);
                        }
                        else if (ClientServer.Communication.Command.Find.Equals(commandData.command))
                        {
                            Console.WriteLine("[COMMAND] FIND");
                            string receivedName;
                            XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
                            using (MemoryStream stream = new MemoryStream(commandData.data))
                            {
                                receivedName = (string)stringSerializer.Deserialize(stream);
                            }
                            Console.WriteLine("Trying to find product with name: " + receivedName);
                            Logic.ProductAbstract foundProduct = shopService.FindProduct(receivedName);
                            ClientServer.Communication.CommandData responseData;
                            responseData.command = ClientServer.Communication.Command.Find;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream())
                            {
                                if (foundProduct != null)
                                {
                                    ClientServer.Communication.Product product = new ClientServer.Communication.Product();
                                    product.guid = foundProduct.GetGuid();
                                    product.name = foundProduct.GetName();
                                    product.price = foundProduct.GetPrice();
                                    productSerializer.Serialize(stream, product);
                                    Console.WriteLine("Found product with name: " + foundProduct.GetName() + ", price: " + foundProduct.GetPrice() + ", GUID: " + foundProduct.GetGuid());
                                }
                                else
                                {
                                    productSerializer.Serialize(stream, null);
                                    Console.WriteLine("Could not find product with name: " + receivedName);
                                }
                                responseData.data = stream.ToArray();
                            }
                            Task serverSendTask = webSocketConnectionServer.SendAsync(responseData);
                            serverSendTask.Wait(new TimeSpan(0, 0, 10));
                        }
                        else if (ClientServer.Communication.Command.FindAll.Equals(commandData.command))
                        {
                            Console.WriteLine("[COMMAND] FIND ALL");
                            string receivedName;
                            XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
                            using (MemoryStream stream = new MemoryStream(commandData.data))
                            {
                                receivedName = (string)stringSerializer.Deserialize(stream);
                            }
                            Console.WriteLine("Trying to find all products with name: " + receivedName);
                            List<Logic.ProductAbstract> foundProducts = shopService.FindProducts(receivedName);
                            ClientServer.Communication.CommandData responseData;
                            responseData.command = ClientServer.Communication.Command.FindAll;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            if (foundProducts.Count > 0)
                            {
                                foreach (Logic.ProductAbstract foundProduct in foundProducts)
                                {
                                    if (foundProduct != null)
                                    {
                                        using (MemoryStream stream = new MemoryStream())
                                        {
                                            ClientServer.Communication.Product product = new ClientServer.Communication.Product();
                                            product.guid = foundProduct.GetGuid();
                                            product.name = foundProduct.GetName();
                                            product.price = foundProduct.GetPrice();
                                            productSerializer.Serialize(stream, product);
                                            responseData.data = stream.ToArray();
                                        }
                                        Task serverSendTask = webSocketConnectionServer.SendAsync(responseData);
                                        serverSendTask.Wait(new TimeSpan(0, 0, 10));
                                        Console.WriteLine("Found product with name: " + foundProduct.GetName() + ", price: " + foundProduct.GetPrice() + ", GUID: " + foundProduct.GetGuid());
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Could not find product with name: " + receivedName);
                            }
                        }
                        else if (ClientServer.Communication.Command.Remove.Equals(commandData.command))
                        {
                            Console.WriteLine("[COMMAND] REMOVE");
                            Guid receivedGuid;
                            XmlSerializer guidSerializer = new XmlSerializer(typeof(Guid));
                            using (MemoryStream stream = new MemoryStream(commandData.data))
                            {
                                receivedGuid = (Guid)guidSerializer.Deserialize(stream);
                            }
                            Console.WriteLine("Trying to remove product with GUID: " + receivedGuid);
                            Logic.ProductAbstract removedProduct = shopService.RemoveProduct(receivedGuid);
                            ClientServer.Communication.CommandData responseData;
                            responseData.command = ClientServer.Communication.Command.Remove;
                            XmlSerializer productSerializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream())
                            {
                                if (removedProduct != null)
                                {
                                    ClientServer.Communication.Product product = new ClientServer.Communication.Product();
                                    product.guid = removedProduct.GetGuid();
                                    product.name = removedProduct.GetName();
                                    product.price = removedProduct.GetPrice();
                                    productSerializer.Serialize(stream, product);
                                    Console.WriteLine("Removed product with name: " + removedProduct.GetName() + ", price: " + removedProduct.GetPrice() + ", GUID: " + removedProduct.GetGuid());
                                }
                                else
                                {
                                    productSerializer.Serialize(stream, null);
                                    Console.WriteLine("Could not remove product with GUID: " + receivedGuid);
                                }
                                responseData.data = stream.ToArray();
                            }
                            Task serverSendTask = webSocketConnectionServer.SendAsync(responseData);
                            serverSendTask.Wait(new TimeSpan(0, 0, 10));
                        }
                    };
                }
                );
                await webSocketConnectionServer?.DisconnectAsync();
            }
            );

            serverTask.Wait();
        }
    }
}
