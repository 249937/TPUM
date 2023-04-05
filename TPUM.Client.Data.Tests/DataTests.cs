using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TPUM.Client.Data.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void ProductGuidTest()
        {
            Guid guid = Guid.NewGuid();
            ProductAbstract product = new Product(guid, "Product", 1.0f);
            Assert.AreEqual(guid, product.GetGuid());
            Assert.AreEqual(guid, product.GetGuid());
        }

        [TestMethod]
        public void ProductEmptyGuidTest()
        {
            Guid guid = Guid.Empty;
            ProductAbstract product = new Product(guid, "Product", 1.0f);
            Guid returnedGuid = product.GetGuid();
            Assert.AreNotEqual(guid, returnedGuid);
            Assert.AreEqual(returnedGuid, product.GetGuid());
        }

        [TestMethod]
        public void ProductNameTest()
        {
            string productName = "Test Product";
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            product.SetName(productName);
            Assert.AreEqual(productName, product.GetName());
        }

        [TestMethod]
        public void ProductEmptyNameTest()
        {
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            Assert.ThrowsException<ArgumentException>(() => product.SetName(""));
        }

        [TestMethod]
        public void ProductWhiteSpaceNameTest()
        {
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            Assert.ThrowsException<ArgumentException>(() => product.SetName(" "));
        }

        [TestMethod]
        public void ProductNullNameTest()
        {
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            Assert.ThrowsException<ArgumentNullException>(() => product.SetName(null));
        }

        [TestMethod]
        public void ProductPriceTest()
        {
            float price = 6.9f;
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            product.SetPrice(price);
            Assert.AreEqual(price, product.GetPrice());
        }

        [TestMethod]
        public void ProductLargePriceTest()
        {
            float price = float.MaxValue;
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            product.SetPrice(price);
            Assert.AreEqual(price, product.GetPrice());
        }

        [TestMethod]
        public void ProductSmallPriceTest()
        {
            float price = 0.01f;
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            product.SetPrice(price);
            Assert.AreEqual(price, product.GetPrice());
        }

        [TestMethod]
        public void ProductZeroPriceTest()
        {
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => product.SetPrice(0.0f));
        }

        [TestMethod]
        public void ProductNegativePriceTest()
        {
            ProductAbstract product = new Product(Guid.NewGuid(), "Product", 1.0f);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => product.SetPrice(-1.0f));
        }
    }
}
