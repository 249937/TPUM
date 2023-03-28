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
            MainModelAbstract.Instance.Clear();

            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => MainModelAbstract.Instance.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => MainModelAbstract.Instance.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            MainModelAbstract.Instance.Clear();

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => MainModelAbstract.Instance.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => MainModelAbstract.Instance.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void InvalidFindTest()
        {
            MainModelAbstract.Instance.Clear();

            string nullName = null;
            string emptyName = "";
            string fullName = "Test Product";

            Assert.ThrowsException<ArgumentNullException>(() => MainModelAbstract.Instance.FindProduct(nullName));
            Assert.ThrowsException<ArgumentException>(() => MainModelAbstract.Instance.FindProduct(emptyName));
            ProductAbstract product = MainModelAbstract.Instance.FindProduct(fullName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void AddFindRemoveTest()
        {
            string productName = "Test Product";
            float productPrice = 5.25f;

            ProductAbstract product = MainModelAbstract.Instance.FindProduct(productName);
            Assert.IsNull(product);

            MainModelAbstract.Instance.AddProduct(productName, productPrice);
            product = MainModelAbstract.Instance.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            product = MainModelAbstract.Instance.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            MainModelAbstract.Instance.RemoveProduct(product.GetGuid());
            product = MainModelAbstract.Instance.FindProduct(productName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void FindProductsTest()
        {
            string product1Name = "Test Product 1";
            float product1Price = 5.25f;
            string product2Name = "Test Product 2";
            float product2Price = 6.9f;

            List<ProductAbstract> products = MainModelAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(0, products.Count);
            products = MainModelAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            MainModelAbstract.Instance.AddProduct(product1Name, product1Price);
            products = MainModelAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = MainModelAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            MainModelAbstract.Instance.AddProduct(product2Name, product2Price);
            products = MainModelAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = MainModelAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);

            MainModelAbstract.Instance.AddProduct(product1Name, product1Price);
            products = MainModelAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(2, products.Count);
            products = MainModelAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);
        }
    }
}
