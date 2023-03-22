using System;
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
