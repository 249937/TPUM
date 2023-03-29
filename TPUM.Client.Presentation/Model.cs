using System;
using System.Collections.Generic;
using TPUM.Client.Logic;

namespace TPUM.Client.Presentation.Model
{
    public abstract class MainModelAbstract
    {
        private class MainModel : MainModelAbstract
        {
            public override event Action<ProductAbstract> OnProductAdded;
            public override event Action<ProductAbstract> OnProductRemoved;

            private ShopServiceAbstract shopService = null;

            public MainModel(ShopServiceAbstract shopService)
            {
                this.shopService = shopService;

                this.shopService.OnProductAdded += HandleProductAdded;
                this.shopService.OnProductRemoved += HandleProductRemoved;
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

                shopService.AddProduct(name, price);
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

                Logic.ProductAbstract productFound = shopService.FindProduct(name);
                if (productFound == null)
                {
                    return null;
                }
                return new Product(productFound.GetGuid(), productFound.GetName(), productFound.GetPrice()); ;
            }

            public override List<ProductAbstract> FindProducts(string name)
            {
                List<ProductAbstract> productsFound = new List<ProductAbstract>();
                List<Logic.ProductAbstract> productsFoundInLogicLayer = shopService.FindProducts(name);
                foreach (Logic.ProductAbstract product in productsFoundInLogicLayer)
                {
                    if (name.Equals(product.GetName()))
                    {
                        productsFound.Add(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
                    }
                }  
                return productsFound;
            }

            public override void RemoveProduct(Guid productGuid)
            {
                shopService.RemoveProduct(productGuid);
            }

            public void HandleProductAdded(Logic.ProductAbstract product)
            {
                OnProductAdded?.Invoke(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
            }

            public void HandleProductRemoved(Logic.ProductAbstract product)
            {
                OnProductRemoved?.Invoke(new Product(product.GetGuid(), product.GetName(), product.GetPrice()));
            }
        }

        public abstract event Action<ProductAbstract> OnProductAdded;
        public abstract event Action<ProductAbstract> OnProductRemoved;

        public abstract void AddProduct(string name, float price);

        public abstract ProductAbstract FindProduct(string name);

        public abstract List<ProductAbstract> FindProducts(string name);

        public abstract void RemoveProduct(Guid productGuid);

        public static MainModelAbstract CreateModel()
        {
            return new MainModel(ShopServiceAbstract.CreateShopService());
        }

        internal static MainModelAbstract CreateModel(ShopServiceAbstract shopService)
        {
            if (shopService == null)
            {
                return CreateModel();
            }
            return new MainModel(shopService);
        }
    }
}
