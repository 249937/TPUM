using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace TPUM.ClientServer.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void AddFindRemoveProductTest()
        {
            try
            {
                Task serverTask = Task.Run(async () => await Server.Presentation.Presentation.StartServer());

                Client.Presentation.Model.MainModelAbstract client = Client.Presentation.Model.MainModelAbstract.CreateModel();

                string productName = "Test Product";
                float productPrice = 13.37f;

                Thread.Sleep(1000);

                Client.Presentation.Model.ProductAbstract product = client.FindProduct(productName);
                Assert.AreEqual(0, client.FindProducts(productName).Count);
                Assert.IsNull(product);

                client.AddProduct(productName, productPrice);
                product = client.FindProduct(productName);
                Assert.AreEqual(1, client.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());

                product = client.FindProduct(productName);
                Assert.AreEqual(1, client.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());

                client.RemoveProduct(product.GetGuid());
                product = client.FindProduct(productName);
                Assert.AreEqual(0, client.FindProducts(productName).Count);
                Assert.IsNull(product);

                Thread.Sleep(4000);
                Server.Presentation.Presentation.StopServer();
                Thread.Sleep(4000);
                Assert.IsTrue(serverTask.IsCompleted);
            }
            finally
            {
                Server.Presentation.Presentation.StopServer();
                Thread.Sleep(4000);
            }
        }

        [TestMethod]
        public void MultipleClientsTest()
        {
            Thread.Sleep(5000);
            try
            {
                Task serverTask = Task.Run(async () => await Server.Presentation.Presentation.StartServer());

                Client.Presentation.Model.MainModelAbstract client0 = Client.Presentation.Model.MainModelAbstract.CreateModel();
                Client.Presentation.Model.MainModelAbstract client1 = Client.Presentation.Model.MainModelAbstract.CreateModel();
                Client.Presentation.Model.MainModelAbstract client2 = Client.Presentation.Model.MainModelAbstract.CreateModel();
                Client.Presentation.Model.MainModelAbstract client3 = Client.Presentation.Model.MainModelAbstract.CreateModel();

                string productName = "Test Product";
                float productPrice = 13.37f;

                Thread.Sleep(1000);

                Client.Presentation.Model.ProductAbstract product = client0.FindProduct(productName);
                Assert.AreEqual(0, client0.FindProducts(productName).Count);
                Assert.IsNull(product);
                product = client1.FindProduct(productName);
                Assert.AreEqual(0, client1.FindProducts(productName).Count);
                Assert.IsNull(product);
                product = client2.FindProduct(productName);
                Assert.AreEqual(0, client2.FindProducts(productName).Count);
                Assert.IsNull(product);
                product = client3.FindProduct(productName);
                Assert.AreEqual(0, client3.FindProducts(productName).Count);
                Assert.IsNull(product);

                client0.AddProduct(productName, productPrice);
                product = client0.FindProduct(productName);
                Assert.AreEqual(1, client0.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client1.FindProduct(productName);
                Assert.AreEqual(1, client1.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client2.FindProduct(productName);
                Assert.AreEqual(1, client2.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client3.FindProduct(productName);
                Assert.AreEqual(1, client3.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                client0.RemoveProduct(product.GetGuid());
                product = client0.FindProduct(productName);
                Assert.AreEqual(0, client0.FindProducts(productName).Count);
                Assert.IsNull(product);
                product = client1.FindProduct(productName);
                Assert.AreEqual(0, client1.FindProducts(productName).Count);
                Assert.IsNull(product);
                product = client2.FindProduct(productName);
                Assert.AreEqual(0, client2.FindProducts(productName).Count);
                Assert.IsNull(product);
                product = client3.FindProduct(productName);
                Assert.AreEqual(0, client3.FindProducts(productName).Count);
                Assert.IsNull(product);

                client0.AddProduct(productName, productPrice);
                client1.AddProduct(productName, productPrice);
                client2.AddProduct(productName, productPrice);
                client3.AddProduct(productName, productPrice);
                product = client0.FindProduct(productName);
                Assert.AreEqual(4, client0.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client1.FindProduct(productName);
                Assert.AreEqual(4, client1.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client2.FindProduct(productName);
                Assert.AreEqual(4, client2.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client3.FindProduct(productName);
                Assert.AreEqual(4, client3.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                client0.RemoveProduct(product.GetGuid());
                product = client0.FindProduct(productName);
                Assert.AreEqual(3, client0.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client1.FindProduct(productName);
                Assert.AreEqual(3, client1.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client2.FindProduct(productName);
                Assert.AreEqual(3, client2.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client3.FindProduct(productName);
                Assert.AreEqual(3, client3.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                client0.RemoveProduct(product.GetGuid());
                client1.RemoveProduct(product.GetGuid());
                client2.RemoveProduct(product.GetGuid());
                client3.RemoveProduct(product.GetGuid());
                product = client0.FindProduct(productName);
                Assert.AreEqual(2, client0.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client1.FindProduct(productName);
                Assert.AreEqual(2, client1.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client2.FindProduct(productName);
                Assert.AreEqual(2, client2.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());
                product = client3.FindProduct(productName);
                Assert.AreEqual(2, client3.FindProducts(productName).Count);
                Assert.IsNotNull(product);
                Assert.AreEqual(productName, product.GetName());
                Assert.AreEqual(productPrice, product.GetPrice());

                Thread.Sleep(4000);
                Server.Presentation.Presentation.StopServer();
                Thread.Sleep(4000);
                Assert.IsTrue(serverTask.IsCompleted);
            }
            finally
            {
                Server.Presentation.Presentation.StopServer();
                Thread.Sleep(4000);
            }
        }
    }
}
