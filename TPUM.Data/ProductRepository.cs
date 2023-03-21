using System;
using System.Collections.Generic;

namespace TPUM.Data
{
    public class ProductRepository : IProductRepository
    {
        private List<IProduct> products;

        public ProductRepository()
        {
            products = new List<IProduct>();
        }

        public void Add(IProduct product)
        {
            if (product == null)
            {
                throw new ArgumentNullException();
            }
            foreach (IProduct existingProduct in products)
            {
                if (existingProduct.GetGuid() == product.GetGuid())
                {
                    return;
                }
            }
            products.Add(product);
        }

        public IProduct Get(Guid productGuid)
        {
            if (Guid.Empty.Equals(productGuid))
            {
                throw new ArgumentException();
            }
            foreach (IProduct product in products)
            {
                if (product.GetGuid() == productGuid)
                {
                    return product;
                }
            }
            return null;
        }

        public List<IProduct> GetAll()
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
