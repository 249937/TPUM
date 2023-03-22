using System;
using TPUM.Data;

namespace TPUM.Logic
{
    public class ShopService: IShopService
    {
        private readonly ProductRepositoryAbstract _productRepository;

        public ShopService(ProductRepositoryAbstract productRepository)
        {
            _productRepository = productRepository;
        }

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

            ProductAbstract product = null;
            product.SetName(name);
            product.SetPrice(price);

            _productRepository.Add(product);
        }

        public void RemoveProduct(Guid productGuid)
        {
            if (Guid.Empty.Equals(productGuid))
            {
                throw new ArgumentException();
            }
            _productRepository.Remove(productGuid);
        }
    }
}
