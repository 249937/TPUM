using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TPUM.Data;
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
        public void AddAndRemoveTest()
        {
            ShopServiceAbstract.ShopService shopService = new ShopServiceAbstract.ShopService();

            ProductRepositoryAbstract productRepository = new ProductRepository();

            string productName = "Test Product";
            float productPrice = 5.25f;

            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            shopService.AddProduct(productName, productPrice);
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            
            shopService.RemoveProduct(ProductRepositoryAbstract.Instance.GetAll()[0].GetGuid());
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
        }
    }
}
