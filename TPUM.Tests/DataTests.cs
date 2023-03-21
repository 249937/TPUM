﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TPUM.Data;

namespace TPUMTests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void ProductGuidTest()
        {
            Guid guid = Guid.NewGuid();
            IProduct product = new Product(guid);
            Assert.AreEqual(guid, product.GetGuid());
            Assert.AreEqual(guid, product.GetGuid());
        }

        [TestMethod]
        public void ProductEmptyGuidTest()
        {
            Guid guid = Guid.Empty;
            IProduct product = new Product(guid);
            Guid returnedGuid = product.GetGuid();
            Assert.AreNotEqual(guid, returnedGuid);
            Assert.AreEqual(returnedGuid, product.GetGuid());
        }

        [TestMethod]
        public void ProductNameTest()
        {
            string productName = "Test Product";
            IProduct product = new Product(Guid.NewGuid());
            product.SetName(productName);
            Assert.AreEqual(productName, product.GetName());
        }

        [TestMethod]
        public void ProductEmptyNameTest()
        {
            IProduct product = new Product(Guid.NewGuid());
            Assert.ThrowsException<ArgumentException>(() => product.SetName(""));
        }

        [TestMethod]
        public void ProductWhiteSpaceNameTest()
        {
            IProduct product = new Product(Guid.NewGuid());
            Assert.ThrowsException<ArgumentException>(() => product.SetName(" "));
        }

        [TestMethod]
        public void ProductNullNameTest()
        {
            IProduct product = new Product(Guid.NewGuid());
            Assert.ThrowsException<ArgumentNullException>(() => product.SetName(null));
        }

        [TestMethod]
        public void ProductPriceTest()
        {
            float price = 6.9f;
            IProduct product = new Product(Guid.NewGuid());
            product.SetPrice(price);
            Assert.AreEqual(price, product.GetPrice());
        }

        [TestMethod]
        public void ProductLargePriceTest()
        {
            float price = float.MaxValue;
            IProduct product = new Product(Guid.NewGuid());
            product.SetPrice(price);
            Assert.AreEqual(price, product.GetPrice());
        }

        [TestMethod]
        public void ProductSmallPriceTest()
        {
            float price = 0.01f;
            IProduct product = new Product(Guid.NewGuid());
            product.SetPrice(price);
            Assert.AreEqual(price, product.GetPrice());
        }

        [TestMethod]
        public void ProductZeroPriceTest()
        {
            IProduct product = new Product(Guid.NewGuid());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => product.SetPrice(0.0f));
        }

        [TestMethod]
        public void ProductNegativePriceTest()
        {
            IProduct product = new Product(Guid.NewGuid());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => product.SetPrice(-1.0f));
        }

        [TestMethod]
        public void ProductRepositoryEmptyTest()
        {
            IProductRepository productRepository = new ProductRepository();
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.ThrowsException<ArgumentException>(() => productRepository.Get(Guid.Empty));
            Assert.AreEqual(null, productRepository.Get(Guid.NewGuid()));
        }

        [TestMethod]
        public void ProductRepositoryAddAndGetTest()
        {
            IProductRepository productRepository = new ProductRepository();
            IProduct product1 = new Product(Guid.NewGuid());
            IProduct product2 = new Product(Guid.NewGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.AreEqual(null, productRepository.Get(product1.GetGuid()));
            productRepository.Add(product1);
            Assert.AreEqual(1, productRepository.GetAll().Count);
            Assert.AreEqual(product1, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(product1.GetGuid(), productRepository.Get(product1.GetGuid()).GetGuid());
            productRepository.Add(product1);
            Assert.AreEqual(1, productRepository.GetAll().Count);
            Assert.AreEqual(product1, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(product1.GetGuid(), productRepository.Get(product1.GetGuid()).GetGuid());
            productRepository.Add(product2);
            Assert.AreEqual(2, productRepository.GetAll().Count);
            Assert.AreEqual(product1, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(product2, productRepository.Get(product2.GetGuid()));
            Assert.AreEqual(product1.GetGuid(), productRepository.Get(product1.GetGuid()).GetGuid());
            Assert.AreEqual(product2.GetGuid(), productRepository.Get(product2.GetGuid()).GetGuid());
        }

        [TestMethod]
        public void ProductRepositoryRemoveTest()
        {
            IProductRepository productRepository = new ProductRepository();
            IProduct product1 = new Product(Guid.NewGuid());
            IProduct product2 = new Product(Guid.NewGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.AreEqual(null, productRepository.Get(product1.GetGuid()));
            productRepository.Add(product1);
            Assert.AreEqual(1, productRepository.GetAll().Count);
            Assert.AreEqual(product1, productRepository.Get(product1.GetGuid()));
            productRepository.Remove(product1.GetGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.AreEqual(null, productRepository.Get(product1.GetGuid()));
            productRepository.Add(product1);
            Assert.AreEqual(1, productRepository.GetAll().Count);
            Assert.AreEqual(product1, productRepository.Get(product1.GetGuid()));
            productRepository.Add(product2);
            Assert.AreEqual(2, productRepository.GetAll().Count);
            Assert.AreEqual(product1, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(product2, productRepository.Get(product2.GetGuid()));
            productRepository.Remove(product1.GetGuid());
            Assert.AreEqual(1, productRepository.GetAll().Count);
            Assert.AreEqual(null, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(product2, productRepository.Get(product2.GetGuid()));
            productRepository.Remove(product1.GetGuid());
            Assert.AreEqual(1, productRepository.GetAll().Count);
            Assert.AreEqual(null, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(product2, productRepository.Get(product2.GetGuid()));
            productRepository.Remove(product2.GetGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.AreEqual(null, productRepository.Get(product1.GetGuid()));
            Assert.AreEqual(null, productRepository.Get(product2.GetGuid()));
        }

        [TestMethod]
        public void ProductRepositoryEmptyRemoveTest()
        {
            IProductRepository productRepository = new ProductRepository();
            Assert.AreEqual(0, productRepository.GetAll().Count);
            productRepository.Remove(Guid.NewGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.ThrowsException<ArgumentException>(() => productRepository.Remove(Guid.Empty));
            Assert.AreEqual(0, productRepository.GetAll().Count);
        }
    }
}
