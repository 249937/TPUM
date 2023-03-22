using System;

namespace TPUM.Data
{
    internal class Product : IProduct
    {
        private Guid guid;
        private string name;
        private float price;

        public Product(Guid guid)
        {
            if (!Guid.Empty.Equals(guid))
            {
                this.guid = guid;
            }
            else
            {
                this.guid = Guid.NewGuid();
            }
        }

        public Guid GetGuid()
        {
            return guid;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException();
            }
            this.name = name;
        }

        public float GetPrice()
        {
            return price;
        }

        public void SetPrice(float price)
        {
            if (price <= 0.0f)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.price = price;
        }
    }
}
