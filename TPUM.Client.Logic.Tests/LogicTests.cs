using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TPUM.Client.Logic.Tests
{
    [TestClass]
    public class LogicTests
    {
        private class Data : Client.Data.DataAbstract
        {
            public override event Action<Client.Data.ProductAbstract> OnProductAdded;
            public override event Action<Client.Data.ProductAbstract> OnProductRemoved;

            private List<Client.Data.ProductAbstract> products;

            public Data()
            {
                products = new List<Client.Data.ProductAbstract>();
            }

            public override void Add(Client.Data.ProductAbstract product)
            {
                if (product == null)
                {
                    throw new ArgumentNullException();
                }

                foreach (Client.Data.ProductAbstract existingProduct in products)
                {
                    if (existingProduct.GetGuid() == product.GetGuid())
                    {
                        return;
                    }
                }

                products.Add(product);
                OnProductAdded?.Invoke(product);
            }

            public override Client.Data.ProductAbstract Find(string productName)
            {
                if (productName == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    throw new ArgumentException();
                }

                foreach (Client.Data.ProductAbstract product in products)
                {
                    if (product.GetName() == productName)
                    {
                        return product;
                    }
                }
                return null;
            }

            public override List<Client.Data.ProductAbstract> FindAll(string productName)
            {
                if (productName == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    throw new ArgumentException();
                }

                List<Client.Data.ProductAbstract> productsFound = new List<Client.Data.ProductAbstract>();
                foreach (Client.Data.ProductAbstract product in products)
                {
                    if (product.GetName() == productName)
                    {
                        productsFound.Add(product);
                    }
                }

                return productsFound;
            }

            public override void Remove(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                for (int i = products.Count - 1; i >= 0; --i)
                {
                    if (productGuid.Equals(products[i].GetGuid()))
                    {
                        Client.Data.ProductAbstract product = products[i];
                        products.RemoveAt(i);
                        OnProductRemoved?.Invoke(product);
                    }
                }
            }
        }

        [TestMethod]
        public void AddProductWithInvalidNameTest()
        {
            ShopServiceAbstract shopService = ShopServiceAbstract.CreateShopService(new Data());

            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => shopService.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => shopService.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            ShopServiceAbstract shopService = ShopServiceAbstract.CreateShopService(new Data());

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => shopService.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => shopService.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void AddFindRemoveTest()
        {
            ShopServiceAbstract shopService = ShopServiceAbstract.CreateShopService(new Data());

            string productName = "Test Product";
            float productPrice = 5.25f;

            ProductAbstract product = shopService.FindProduct(productName);
            Assert.IsNull(product);

            shopService.AddProduct(productName, productPrice);
            product = shopService.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            product = shopService.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            shopService.RemoveProduct(product.GetGuid());
            product = shopService.FindProduct(productName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void FindProductsTest()
        {
            ShopServiceAbstract shopService = ShopServiceAbstract.CreateShopService(new Data());

            string product1Name = "Test Product 1";
            float product1Price = 5.25f;
            string product2Name = "Test Product 2";
            float product2Price = 6.9f;

            List<ProductAbstract> products = shopService.FindProducts(product1Name);
            Assert.AreEqual(0, products.Count);
            products = shopService.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            shopService.AddProduct(product1Name, product1Price);
            products = shopService.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = shopService.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            shopService.AddProduct(product2Name, product2Price);
            products = shopService.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = shopService.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);

            shopService.AddProduct(product1Name, product1Price);
            products = shopService.FindProducts(product1Name);
            Assert.AreEqual(2, products.Count);
            products = shopService.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);
        }
    }
}
