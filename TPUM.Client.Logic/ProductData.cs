using System;

namespace TPUM.Client.Logic
{
    internal class ProductData : Data.ProductAbstract
    {
        private Guid guid;
        private string name;
        private float price;

        public ProductData(Guid guid) : base(guid)
        {
            this.guid = guid;
        }

        public override Guid GetGuid()
        {
            return guid;
        }

        public override string GetName()
        {
            return name;
        }

        public override float GetPrice()
        {
            return price;
        }

        public override void SetName(string name)
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

        public override void SetPrice(float price)
        {
            if (price <= 0.0f)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.price = price;
        }
    }
}
