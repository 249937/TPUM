﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TPUM.Server.Data.Tests
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
            ProductRepositoryAbstract productRepository = ProductRepositoryAbstract.CreateProductRepository();
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.ThrowsException<ArgumentException>(() => productRepository.Get(Guid.Empty));
            Assert.AreEqual(null, productRepository.Get(Guid.NewGuid()));
        }

        [TestMethod]
        public void ProductRepositoryAddAndGetTest()
        {
            ProductRepositoryAbstract productRepository = ProductRepositoryAbstract.CreateProductRepository();
            ProductAbstract product1 = new Product(Guid.NewGuid(), "Product1", 1.0f);
            ProductAbstract product2 = new Product(Guid.NewGuid(), "Product1", 2.0f);
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
            ProductRepositoryAbstract productRepository = ProductRepositoryAbstract.CreateProductRepository();
            ProductAbstract product1 = new Product(Guid.NewGuid(), "Product1", 1.0f);
            ProductAbstract product2 = new Product(Guid.NewGuid(), "Product1", 2.0f);
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
            ProductRepositoryAbstract productRepository = ProductRepositoryAbstract.CreateProductRepository();
            Assert.AreEqual(0, productRepository.GetAll().Count);
            productRepository.Remove(Guid.NewGuid());
            Assert.AreEqual(0, productRepository.GetAll().Count);
            Assert.ThrowsException<ArgumentException>(() => productRepository.Remove(Guid.Empty));
            Assert.AreEqual(0, productRepository.GetAll().Count);
        }
    }
}
