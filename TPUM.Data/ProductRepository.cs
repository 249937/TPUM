using System;
using System.Collections.Generic;

namespace TPUM.Data
{
    internal class ProductRepository : ProductRepositoryAbstract
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
    }
}
