using System;
using System.Collections.Generic;

namespace TPUM.Server.Data
{
    public abstract class ProductRepositoryAbstract
    {
        private class ProductRepository : ProductRepositoryAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            private List<ProductAbstract> products;

            public ProductRepository()
            {
                products = new List<ProductAbstract>();
            }

            public override void Add(ProductAbstract product)
            {
                if (product == null)
                {
                    throw new ArgumentNullException();
                }
                foreach (ProductAbstract existingProduct in products)
                {
                    if (existingProduct.GetGuid() == product.GetGuid())
                    {
                        return;
                    }
                }
                products.Add(product);
                OnProductAdded?.Invoke(product);
            }

            public override ProductAbstract Get(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }
                foreach (ProductAbstract product in products)
                {
                    if (product.GetGuid() == productGuid)
                    {
                        return product;
                    }
                }
                return null;
            }

            public override List<ProductAbstract> GetAll()
            {
                return products;
            }

            public override ProductAbstract Remove(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }
                for (int i = products.Count - 1; i >= 0; --i)
                {
                    if (productGuid.Equals(products[i].GetGuid()))
                    {
                        ProductAbstract product = products[i];
                        products.RemoveAt(i);
                        OnProductRemoved?.Invoke(product);
                        return product;
                    }
                }
                return null;
            }
        }

        public abstract event Action<ProductAbstract> OnProductAdded;
        public abstract event Action<ProductAbstract> OnProductRemoved;

        public abstract void Add(ProductAbstract product);

        public abstract ProductAbstract Get(Guid productGuid);

        public abstract List<ProductAbstract> GetAll();

        public abstract ProductAbstract Remove(Guid productGuid);

        public static ProductRepositoryAbstract CreateProductRepository()
        {
            return new ProductRepository();

        }
    }
}
