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
            ShopServiceAbstract.ShopService shopService = new ShopServiceAbstract.ShopService();
            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => shopService.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => shopService.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            ShopServiceAbstract.ShopService shopService = new ShopServiceAbstract.ShopService();

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => shopService.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => shopService.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void AddFindRemoveTest()
        {
            ShopServiceAbstract.ShopService shopService = new ShopServiceAbstract.ShopService();

            string productName = "Test Product";
            float productPrice = 5.25f;

            Product product = shopService.FindProduct(productName);
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
            ShopServiceAbstract.ShopService shopService = new ShopServiceAbstract.ShopService();

            string product1Name = "Test Product 1";
            float product1Price = 5.25f;
            string product2Name = "Test Product 2";
            float product2Price = 6.9f;

            List<Product> products = shopService.FindProducts(product1Name);
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
