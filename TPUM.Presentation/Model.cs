using System;
using System.Collections.Generic;
using TPUM.Logic;

namespace TPUM.Presentation.Model
{
    public abstract class MainModelAbstract
    {
        private class MainModel : MainModelAbstract
        {
            public MainModel()
            {
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

                ShopServiceAbstract.Instance.AddProduct(name, price);
            }

            public override void Clear()
            {
                ShopServiceAbstract.Instance.Clear();
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

                Logic.ProductAbstract productFound = ShopServiceAbstract.Instance.FindProduct(name);
                if (productFound == null) return null;
                Presentation.Model.ProductAbstract product = new Presentation.Model.Product(productFound.GetGuid());

                product.SetName(productFound.GetName());
                product.SetPrice(productFound.GetPrice());
                return product;
            }

            public override List<ProductAbstract> FindProducts(string name)
            {
                List<ProductAbstract> productsFound = new List<ProductAbstract>();
                List<Logic.ProductAbstract> productsFoundInLogicLayer = ShopServiceAbstract.Instance.FindProducts(name);
                foreach (Logic.ProductAbstract product in productsFoundInLogicLayer)
                {
                    if (name.Equals(product.GetName()))
                    {
                        ProductAbstract productFound = new Product(product.GetGuid());
                        productFound.SetName(product.GetName());
                        productFound.SetPrice(product.GetPrice());
                        productsFound.Add(productFound);
                    }
                }  
                return productsFound;
            }

            public override void RemoveProduct(Guid productGuid)
            {
                ShopServiceAbstract.Instance.RemoveProduct(productGuid);
            }
        }

        private static MainModelAbstract instance;

        protected MainModelAbstract()
        {
        }

        public static MainModelAbstract Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainModel();
                }
                return instance;
            }
        }

        public abstract void AddProduct(string name, float price);

        public abstract ProductAbstract FindProduct(string name);

        public abstract List<ProductAbstract> FindProducts(string name);

        public abstract void RemoveProduct(Guid productGuid);

        public abstract void Clear();

    }
}
