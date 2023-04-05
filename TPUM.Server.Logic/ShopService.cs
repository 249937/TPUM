using System;
using System.Collections.Generic;
using TPUM.Server.Data;

namespace TPUM.Server.Logic
{
    public abstract class ShopServiceAbstract
    {
        private class ShopService : ShopServiceAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            private ProductRepositoryAbstract productRepository = null;

            public ShopService(ProductRepositoryAbstract productRepository)
            {
                this.productRepository = productRepository;

                this.productRepository.OnProductAdded += HandleProductAdded;
                this.productRepository.OnProductRemoved += HandleProductRemoved;
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

                productRepository.Add(product);
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

                foreach (ProductData product in productRepository.GetAll())
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
                foreach (ProductData product in productRepository.GetAll())
                {
                    if (name.Equals(product.GetName()))
                    {
                        productsFound.Add(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
                    }
                }
                return productsFound;
            }

            public override ProductAbstract RemoveProduct(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                Data.ProductAbstract removedProductData = productRepository.Remove(productGuid);
                if (removedProductData != null)
                {
                    return new Product(removedProductData.GetGuid(), removedProductData.GetName(), removedProductData.GetPrice());
                }
                return null;
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

        public abstract void AddProduct(string name, float price);

        public abstract ProductAbstract FindProduct(string name);

        public abstract List<ProductAbstract> FindProducts(string name);

        public abstract ProductAbstract RemoveProduct(Guid productGuid);

        public static ShopServiceAbstract CreateShopService()
        {
            return new ShopService(ProductRepositoryAbstract.CreateProductRepository());
        }

        internal static ShopServiceAbstract CreateShopService(ProductRepositoryAbstract productRepository)
        {
            if (productRepository == null)
            {
                return CreateShopService();
            }
            return new ShopService(productRepository);
        }
    }
}
