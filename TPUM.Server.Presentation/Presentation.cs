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
                            XmlSerializer serializer = new XmlSerializer(typeof(ClientServer.Communication.Product));
                            using (MemoryStream stream = new MemoryStream(commandData.data))
                            {
                                receivedProduct = (ClientServer.Communication.Product)serializer.Deserialize(stream);
                            }
                            Console.WriteLine("Trying to add product with GUID: " + receivedProduct.guid + ", name: " + receivedProduct.name + ", price: " + receivedProduct.price);
                            shopService.AddProduct(receivedProduct.name, receivedProduct.price);
                        }
                        else if (ClientServer.Communication.Command.Find.Equals(commandData.command))
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
                        else if (ClientServer.Communication.Command.FindAll.Equals(commandData.command))
                        {
                            Console.WriteLine("[COMMAND] FIND ALL");
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
    }
}
