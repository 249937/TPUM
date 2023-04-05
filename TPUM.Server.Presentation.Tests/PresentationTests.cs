using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TPUM.Server.Presentation.Tests
{
    [TestClass]
    public class PresentationTests
    {
        private class Product : Logic.ProductAbstract
        {
            private Guid guid;
            private string name;
            private float price;

            public Product(Guid guid, string name, float price) : base(guid)
            {
                this.guid = guid;
                SetName(name);
                SetPrice(price);
            }

            public override Guid GetGuid()
            {
                return guid;
            }

            public override string GetName()
            {
                return name;
            }

            public override float GetPrice()
            {
                return price;
            }

            public override void SetName(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }
                this.name = name;
            }

            public override void SetPrice(float price)
            {
                if (price <= 0.0f)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.price = price;
            }
        }

        private class ShopService : Logic.ShopServiceAbstract
        {
            public override event Action<Logic.ProductAbstract> OnProductAdded;
            public override event Action<Logic.ProductAbstract> OnProductRemoved;

            private List<Logic.ProductAbstract> products;

            public ShopService()
            {
                products = new List<Logic.ProductAbstract>();
            }

            public override void AddProduct(string name, float price)
            {
                Product product = new Product(Guid.NewGuid(), name, price);
                if (product == null)
                {
                    throw new ArgumentNullException();
                }

                foreach (Logic.ProductAbstract existingProduct in products)
                {
                    if (existingProduct.GetGuid() == product.GetGuid())
                    {
                        return;
                    }
                }
                products.Add(product);
                OnProductAdded?.Invoke(product);
            }

            public override Logic.ProductAbstract FindProduct(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                foreach (Logic.ProductAbstract product in products)
                {
                    if (name.Equals(product.GetName()))
                    {
                        return new Product(product.GetGuid(), product.GetName(), product.GetPrice());
                    }
                }
                return null;
            }

            public override List<Logic.ProductAbstract> FindProducts(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                List<Logic.ProductAbstract> productsFound = new List<Logic.ProductAbstract>();
                foreach (Logic.ProductAbstract product in products)
                {
                    if (name.Equals(product.GetName()))
                    {
                        productsFound.Add(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
                    }
                }
                return productsFound;
            }

            public override Logic.ProductAbstract RemoveProduct(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }
                for (int i = products.Count - 1; i >= 0; --i)
                {
                    if (productGuid.Equals(products[i].GetGuid()))
                    {
                        Logic.ProductAbstract product = products[i];
                        products.RemoveAt(i);
                        OnProductRemoved?.Invoke(product);
                        return product;
                    }
                }
                return null;
            }
        }
    }
}
