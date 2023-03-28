﻿using System;

namespace TPUM.Data
{
    internal class Product : ProductAbstract
    {
        private Guid guid;
        private string name;
        private float price;

        public Product(Guid guid, string name, float price) : base(guid)
        {
            if (!Guid.Empty.Equals(guid))
            {
                this.guid = guid;
            }
            else
            {
                this.guid = Guid.NewGuid();
            }

            SetName(name);
            SetPrice(price);
        }

        public override Guid GetGuid()
        {
            return guid;
        }

        public override string GetName()
        {
            return name;
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

        public override float GetPrice()
        {
            return price;
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
