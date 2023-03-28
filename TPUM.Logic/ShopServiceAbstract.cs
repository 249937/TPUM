using System;
using System.Collections.Generic;
using TPUM.Data;

namespace TPUM.Logic
{
    public abstract class ShopServiceAbstract
    {
        private class ShopService : ShopServiceAbstract
        {
            public ShopService() 
            {
            }

            public override void AddProduct(string name, float price)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }
                if (price <= 0.0f)
                {
                    throw new ArgumentOutOfRangeException();
                }

                ProductData product = new ProductData(Guid.NewGuid());
                product.SetName(name);
                product.SetPrice(price);

                ProductRepositoryAbstract.Instance.Add(product);
            }

            public override ProductAbstract FindProduct(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                foreach (ProductData product in ProductRepositoryAbstract.Instance.GetAll())
                {
                    if (name.Equals(product.GetName()))
                    {
                        Product productFound = new Product(product.GetGuid());
                        productFound.SetName(product.GetName());
                        productFound.SetPrice(product.GetPrice());
                        return productFound;
                    }
                }
                return null;
            }

            public override List<ProductAbstract> FindProducts(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                List<ProductAbstract> productsFound = new List<ProductAbstract>();
                foreach (ProductData product in ProductRepositoryAbstract.Instance.GetAll())
                {
                    if (name.Equals(product.GetName()))
                    {
                        Product productFound = new Product(product.GetGuid());
                        productFound.SetName(product.GetName());
                        productFound.SetPrice(product.GetPrice());
                        productsFound.Add(productFound);
                    }
                }
                return productsFound;
            }

            public override void RemoveProduct(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                ProductRepositoryAbstract.Instance.Remove(productGuid);
            }

            public override void Clear()
            {
                ProductRepositoryAbstract.Instance.Clear();
            }
        }

        private static ShopServiceAbstract instance;

        protected ShopServiceAbstract()
        {
        }

        public static ShopServiceAbstract Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShopService();
                }
                return instance;
            }
        }

        public abstract void AddProduct(string name, float price);

        public abstract ProductAbstract FindProduct(string name);

        public abstract List<ProductAbstract> FindProducts(string name);

        public abstract void RemoveProduct(Guid productGuid);

        public abstract void Clear();
    }
}
