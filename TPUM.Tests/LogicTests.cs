using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TPUM.Logic;

namespace TPUM.Tests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void AddProductWithInvalidNameTest()
        {
            ShopServiceAbstract.Instance.Clear();

            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => ShopServiceAbstract.Instance.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => ShopServiceAbstract.Instance.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            ShopServiceAbstract.Instance.Clear();

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ShopServiceAbstract.Instance.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ShopServiceAbstract.Instance.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void AddFindRemoveTest()
        {
            ShopServiceAbstract.Instance.Clear();

            string productName = "Test Product";
            float productPrice = 5.25f;

            ProductAbstract product = ShopServiceAbstract.Instance.FindProduct(productName);
            Assert.IsNull(product);

            ShopServiceAbstract.Instance.AddProduct(productName, productPrice);
            product = ShopServiceAbstract.Instance.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            product = ShopServiceAbstract.Instance.FindProduct(productName);
            Assert.IsNotNull(product);
            Assert.AreEqual(productName, product.GetName());
            Assert.AreEqual(productPrice, product.GetPrice());

            ShopServiceAbstract.Instance.RemoveProduct(product.GetGuid());
            product = ShopServiceAbstract.Instance.FindProduct(productName);
            Assert.IsNull(product);
        }

        [TestMethod]
        public void FindProductsTest()
        {
            ShopServiceAbstract.Instance.Clear();

            string product1Name = "Test Product 1";
            float product1Price = 5.25f;
            string product2Name = "Test Product 2";
            float product2Price = 6.9f;

            List<ProductAbstract> products = ShopServiceAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(0, products.Count);
            products = ShopServiceAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            ShopServiceAbstract.Instance.AddProduct(product1Name, product1Price);
            products = ShopServiceAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = ShopServiceAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(0, products.Count);

            ShopServiceAbstract.Instance.AddProduct(product2Name, product2Price);
            products = ShopServiceAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(1, products.Count);
            products = ShopServiceAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);

            ShopServiceAbstract.Instance.AddProduct(product1Name, product1Price);
            products = ShopServiceAbstract.Instance.FindProducts(product1Name);
            Assert.AreEqual(2, products.Count);
            products = ShopServiceAbstract.Instance.FindProducts(product2Name);
            Assert.AreEqual(1, products.Count);
        }
    }
}
