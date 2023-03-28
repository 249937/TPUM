using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TPUM.Data;

namespace TPUM.Tests
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

        [TestMethod]
        public void ProductRepositoryEmptyTest()
        {
            ProductRepositoryAbstract.Instance.Clear();
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.ThrowsException<ArgumentException>(() => ProductRepositoryAbstract.Instance.Get(Guid.Empty));
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(Guid.NewGuid()));
        }

        [TestMethod]
        public void ProductRepositoryAddAndGetTest()
        {
            ProductRepositoryAbstract.Instance.Clear();
            ProductAbstract product1 = new Product(Guid.NewGuid(), "Product1", 1.0f);
            ProductAbstract product2 = new Product(Guid.NewGuid(), "Product1", 2.0f);
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            ProductRepositoryAbstract.Instance.Add(product1);
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(product1, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(product1.GetGuid(), ProductRepositoryAbstract.Instance.Get(product1.GetGuid()).GetGuid());
            ProductRepositoryAbstract.Instance.Add(product1);
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(product1, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(product1.GetGuid(), ProductRepositoryAbstract.Instance.Get(product1.GetGuid()).GetGuid());
            ProductRepositoryAbstract.Instance.Add(product2);
            Assert.AreEqual(2, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(product1, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(product2, ProductRepositoryAbstract.Instance.Get(product2.GetGuid()));
            Assert.AreEqual(product1.GetGuid(), ProductRepositoryAbstract.Instance.Get(product1.GetGuid()).GetGuid());
            Assert.AreEqual(product2.GetGuid(), ProductRepositoryAbstract.Instance.Get(product2.GetGuid()).GetGuid());
        }

        [TestMethod]
        public void ProductRepositoryRemoveTest()
        {
            ProductRepositoryAbstract.Instance.Clear();
            ProductAbstract product1 = new Product(Guid.NewGuid(), "Product1", 1.0f);
            ProductAbstract product2 = new Product(Guid.NewGuid(), "Product1", 2.0f);
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            ProductRepositoryAbstract.Instance.Add(product1);
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(product1, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            ProductRepositoryAbstract.Instance.Remove(product1.GetGuid());
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            ProductRepositoryAbstract.Instance.Add(product1);
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(product1, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            ProductRepositoryAbstract.Instance.Add(product2);
            Assert.AreEqual(2, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(product1, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(product2, ProductRepositoryAbstract.Instance.Get(product2.GetGuid()));
            ProductRepositoryAbstract.Instance.Remove(product1.GetGuid());
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(product2, ProductRepositoryAbstract.Instance.Get(product2.GetGuid()));
            ProductRepositoryAbstract.Instance.Remove(product1.GetGuid());
            Assert.AreEqual(1, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(product2, ProductRepositoryAbstract.Instance.Get(product2.GetGuid()));
            ProductRepositoryAbstract.Instance.Remove(product2.GetGuid());
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product1.GetGuid()));
            Assert.AreEqual(null, ProductRepositoryAbstract.Instance.Get(product2.GetGuid()));
        }

        [TestMethod]
        public void ProductRepositoryEmptyRemoveTest()
        {
            ProductRepositoryAbstract.Instance.Clear();
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            ProductRepositoryAbstract.Instance.Remove(Guid.NewGuid());
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
            Assert.ThrowsException<ArgumentException>(() => ProductRepositoryAbstract.Instance.Remove(Guid.Empty));
            Assert.AreEqual(0, ProductRepositoryAbstract.Instance.GetAll().Count);
        }
    }
}
