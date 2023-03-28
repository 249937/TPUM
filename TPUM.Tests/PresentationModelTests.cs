using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TPUM.Presentation.Model;

namespace TPUM.Tests
{
    [TestClass]
    public class PresentationModelTests
    {
        [TestMethod]
        public void AddProductWithInvalidNameTest()
        {
            MainModelAbstract model = MainModelAbstract.CreateModel();

            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => model.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => model.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            MainModelAbstract model = MainModelAbstract.CreateModel();

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => model.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => model.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void InvalidFindTest()
        {
            MainModelAbstract model = MainModelAbstract.CreateModel();

            string nullName = null;
            string emptyName = "";
            string fullName = "Test Product";

            Assert.ThrowsException<ArgumentNullException>(() => model.FindProduct(nullName));
            Assert.ThrowsException<ArgumentException>(() => model.FindProduct(emptyName));
            ProductAbstract product = model.FindProduct(fullName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void AddFindRemoveTest()
        {
            MainModelAbstract model = MainModelAbstract.CreateModel();

            string productName = "Test Product";
            float productPrice = 5.25f;

            ProductAbstract product = model.FindProduct(productName);
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
            MainModelAbstract model = MainModelAbstract.CreateModel();

            string product1Name = "Test Product 1";
            float product1Price = 5.25f;
            string product2Name = "Test Product 2";
            float product2Price = 6.9f;

            List<ProductAbstract> products = model.FindProducts(product1Name);
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
