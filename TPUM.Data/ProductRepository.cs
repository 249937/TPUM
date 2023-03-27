using System;
using System.Collections.Generic;

namespace TPUM.Data
{
    public abstract class ProductRepositoryAbstract
    {
        private class ProductRepository : ProductRepositoryAbstract
        {
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

            public override void Remove(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }
                for (int i = products.Count - 1; i >= 0; --i)
                {
                    if (productGuid.Equals(products[i].GetGuid()))
                    {
                        products.RemoveAt(i);
                    }
                }
            }

            public override void Clear()
            {
                products.Clear();
            }
        }

        private static ProductRepositoryAbstract instance;

        protected ProductRepositoryAbstract()
        {
        }

        public static ProductRepositoryAbstract Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductRepository();
                }
                return instance;
            }
        }

        public abstract void Add(ProductAbstract product);

        public abstract ProductAbstract Get(Guid productGuid);

        public abstract List<ProductAbstract> GetAll();

        public abstract void Remove(Guid productGuid);

        public abstract void Clear();
    }
}
