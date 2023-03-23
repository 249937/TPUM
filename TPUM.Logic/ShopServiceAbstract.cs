using System;
using System.Collections.Generic;
using TPUM.Data;

namespace TPUM.Logic
{
    public abstract class ShopServiceAbstract
    {
        internal class ShopService : ShopServiceAbstract
        {
            public ShopService() { }

            public void AddProduct(string name, float price)
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

                Product product = new Product(Guid.NewGuid());
                product.SetName(name);
                product.SetPrice(price);

                ProductRepositoryAbstract.Instance.Add(product);
            }

            public Product FindProduct(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                foreach (ProductAbstract product in ProductRepositoryAbstract.Instance.GetAll())
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

            public List<Product> FindProducts(string name)
            {
                if (name == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                List<Product> productsFound = new List<Product>();
                foreach (ProductAbstract product in ProductRepositoryAbstract.Instance.GetAll())
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

            public void RemoveProduct(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                ProductRepositoryAbstract.Instance.Remove(productGuid);
            }
        }
    }
}
