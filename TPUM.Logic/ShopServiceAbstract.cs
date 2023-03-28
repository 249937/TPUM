using System;
using System.Collections.Generic;
using TPUM.Data;

namespace TPUM.Logic
{
    public abstract class ShopServiceAbstract
    {
        private class ShopService : ShopServiceAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            public ShopService() 
            {
                ProductRepositoryAbstract.Instance.OnProductAdded += HandleProductAdded;
                ProductRepositoryAbstract.Instance.OnProductRemoved += HandleProductRemoved;
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
                        return new Product(product.GetGuid(), product.GetName(), product.GetPrice());
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
                        productsFound.Add(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
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

            public void HandleProductAdded(Data.ProductAbstract product)
            {
                OnProductAdded?.Invoke(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
            }

            public void HandleProductRemoved(Data.ProductAbstract product)
            {
                OnProductRemoved?.Invoke(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
            }
        }

        public abstract event Action<ProductAbstract> OnProductAdded;
        public abstract event Action<ProductAbstract> OnProductRemoved;

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
