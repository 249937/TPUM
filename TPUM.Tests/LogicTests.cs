using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TPUM.Data;
using TPUM.Logic;

namespace TPUMTests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void AddProductWithInvalidNameTest()
        {
            IProductRepository productRepository = new ProductRepository();
            IShopService shopService = new ShopService(productRepository);

            string productName1 = "";
            string productName2 = null;
            float productPrice = 5.25f;

            Assert.ThrowsException<ArgumentException>(() => shopService.AddProduct(productName1, productPrice));
            Assert.ThrowsException<ArgumentNullException>(() => shopService.AddProduct(productName2, productPrice));
        }

        [TestMethod]
        public void AddProductWithInvalidPrice()
        {
            IProductRepository productRepository = new ProductRepository();
            IShopService shopService = new ShopService(productRepository);

            string productName = "Test Product";
            float productPrice1 = 0.00f;
            float productPrice2 = -1.25f;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => shopService.AddProduct(productName, productPrice1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => shopService.AddProduct(productName, productPrice2));
        }

        [TestMethod]
        public void AddAndRemoveTest()
        {
            IProductRepository productRepository = new ProductRepository();
            IShopService shopService = new ShopService(productRepository);

            string productName = "Test Product";
            float productPrice = 5.25f;

            Assert.AreEqual(0, productRepository.GetAll().Count);
            shopService.AddProduct(productName, productPrice);
            Assert.AreEqual(1, productRepository.GetAll().Count);
            
            shopService.RemoveProduct(productRepository.GetAll()[0].GetGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
        }
    }
}
