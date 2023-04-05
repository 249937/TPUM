using System;
using System.Collections.Generic;

namespace TPUM.Client.Logic
{
    public abstract class ShopServiceAbstract
    {
        private class ShopService : ShopServiceAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            private Data.DataAbstract data = null;

            public ShopService(Data.DataAbstract data) 
            {
                this.data = data;

                this.data.OnProductAdded += HandleProductAdded;
                this.data.OnProductRemoved += HandleProductRemoved;
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

                data.Add(product);
            }

            public override ProductAbstract FindProduct(string productName)
            {
                if (productName == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    throw new ArgumentException();
                }

                Data.ProductAbstract productData = data.Find(productName);
                if (productData == null)
                {
                    return null;
                }
                return new Product(productData.GetGuid(), productData.GetName(), productData.GetPrice());
            }

            public override List<ProductAbstract> FindProducts(string productName)
            {
                if (productName == null)
                {
                    throw new ArgumentNullException();
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    throw new ArgumentException();
                }

                List<Data.ProductAbstract> productsData = data.FindAll(productName);
                List<ProductAbstract> products = new List<ProductAbstract>();
                foreach (Data.ProductAbstract productData in productsData)
                {
                    products.Add(new Product(productData.GetGuid(), productData.GetName(), productData.GetPrice()));
                }
                return products;
            }

            public override void RemoveProduct(Guid productGuid)
            {
                if (Guid.Empty.Equals(productGuid))
                {
                    throw new ArgumentException();
                }

                data.Remove(productGuid);
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

        public abstract void AddProduct(string productName, float productPrice);

        public abstract ProductAbstract FindProduct(string productName);

        public abstract List<ProductAbstract> FindProducts(string productName);

        public abstract void RemoveProduct(Guid productGuid);

        public static ShopServiceAbstract CreateShopService()
        {
            return new ShopService(Data.DataAbstract.CreateData());
        }

        internal static ShopServiceAbstract CreateShopService(Data.DataAbstract data)
        {
            if (data == null)
            {
                return CreateShopService();
            }
            return new ShopService(data);
        }
    }
}
