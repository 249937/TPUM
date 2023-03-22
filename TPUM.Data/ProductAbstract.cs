using System;

namespace TPUM.Data
{
    public abstract class ProductAbstract
    {
        public ProductAbstract(Guid guid)
        {
        }

        public abstract Guid GetGuid();

        public abstract string GetName();

        public abstract void SetName(string name);

        public abstract float GetPrice();

        public abstract void SetPrice(float price);
    }
}
