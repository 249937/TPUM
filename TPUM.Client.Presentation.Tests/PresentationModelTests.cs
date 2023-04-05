using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TPUM.Client.Presentation.Tests
{
    [TestClass]
    public class PresentationModelTests
    {
        private class Product : Logic.ProductAbstract
        {
            private Guid guid;
            private string name;
            private float price;

            public Product(Guid guid, string name, float price) : base(guid)
            {
                this.guid = guid;
                SetName(name);
                SetPrice(price);
            }

            public override Guid GetGuid()
            {
                return guid;
            }

            public override string GetName()
            {
                return name;
            }

            public override float GetPrice()
            {
                return price;
            }

            public override void SetName(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }
                this.name = name;
            }

            public override void SetPrice(float price)
            {
                if (price <= 0.0f)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.price = price;
            }
        }

        private class ShopService : Logic.ShopServiceAbstract
        {
            public override event Action<Logic.ProductAbstract> OnProductAdded;
            public override event Action<Logic.ProductAbstract> OnProductRemoved;

            private List<Logic.ProductAbstract> products;

            public ShopService()
            {
                products = new List<Logic.ProductAbstract>();
            }

            public override void AddProduct(string name, float price)
            {
                Product product = new Product(Guid.NewGuid(), name, price);
                if (product == null)
                {
                    throw new ArgumentNullException();
                }

                foreach (Logic.ProductAbstract existingProduct in products)
                {
                    if (existingProduct.GetGuid() == product.GetGuid())
                    {
                        return;
                    }
                }
                products.Add(product);
                OnProductAdded?.Invoke(product);
            }

            public override Logic.ProductAbstract FindProduct(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                foreach (Logic.ProductAbstract product in products)
                {
                    if (name.Equals(product.GetName()))
                    {
                        return new Product(product.GetGuid(), product.GetName(), product.GetPrice());
                    }
                }
                return null;
            }

            public override List<Logic.ProductAbstract> FindProducts(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                List<Logic.ProductAbstract> productsFound = new List<Logic.ProductAbstract>();
                foreach (Logic.ProductAbstract product in products)
                {
                    if (name.Equals(product.GetName()))
                    {
                        productsFound.Add(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
                    }
                }
                return productsFound;
            }

            public override void RemoveProduct(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }
                for (int i = products.Count - 1; i >= 0; --i)
                {
                    if (productGuid.Equals(products[i].GetGuid()))
                    {
                        Logic.ProductAbstract product = products[i];
                        products.RemoveAt(i);
                        OnProductRemoved?.Invoke(product);
                    }
                }
            }
        }

        [TestMethod]
        public void AddProductWithInvalidNameTest()
        {
            Model.MainModelAbstract model = Model.MainModelAbstract.CreateModel(new ShopService());

            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => model.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => model.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            Model.MainModelAbstract model = Model.MainModelAbstract.CreateModel(new ShopService());

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => model.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => model.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void InvalidFindTest()
        {
            Model.MainModelAbstract model = Model.MainModelAbstract.CreateModel(new ShopService());

            string nullName = null;
            string emptyName = "";
            string fullName = "Test Product";

            Assert.ThrowsException<ArgumentNullException>(() => model.FindProduct(nullName));
            Assert.ThrowsException<ArgumentException>(() => model.FindProduct(emptyName));
            Model.ProductAbstract product = model.FindProduct(fullName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void AddFindRemoveTest()
        {
            Model.MainModelAbstract model = Model.MainModelAbstract.CreateModel(new ShopService());

            string productName = "Test Product";
            float productPrice = 5.25f;

            Model.ProductAbstract product = model.FindProduct(productName);
            Assert.IsNull(product);

            model.AddProduct(productName, productPrice);
            product = model.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            product = model.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            model.RemoveProduct(product.GetGuid());
            product = model.FindProduct(productName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void FindProductsTest()
        {
            Model.MainModelAbstract model = Model.MainModelAbstract.CreateModel(new ShopService());

            string product1Name = "Test Product 1";
            float product1Price = 5.25f;
            string product2Name = "Test Product 2";
            float product2Price = 6.9f;

            List<Model.ProductAbstract> products = model.FindProducts(product1Name);
            Assert.AreEqual(0, products.Count);
            products = model.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            model.AddProduct(product1Name, product1Price);
            products = model.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = model.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            model.AddProduct(product2Name, product2Price);
            products = model.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = model.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);

            model.AddProduct(product1Name, product1Price);
            products = model.FindProducts(product1Name);
            Assert.AreEqual(2, products.Count);
            products = model.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);
        }
    }
}
