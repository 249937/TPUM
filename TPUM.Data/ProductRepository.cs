using System;
using System.Collections.Generic;

namespace TPUM.Data
{
    internal class ProductRepository : IProductRepository
    {
        private List<ProductAbstract> products;

        public ProductRepository()
        {
            products = new List<ProductAbstract>();
        }

        public void Add(ProductAbstract product)
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

        public ProductAbstract Get(Guid productGuid)
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

        public List<ProductAbstract> GetAll()
        {
            return products;
        }

        public void Remove(Guid productGuid)
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
    }
}
