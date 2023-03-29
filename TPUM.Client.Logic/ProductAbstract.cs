using System;

namespace TPUM.Client.Logic
{
    public abstract class ProductAbstract
    {
        public ProductAbstract(Guid guid)
        {
        }

        public abstract Guid GetGuid();

        public abstract string GetName();

        public abstract float GetPrice();

        public abstract void SetName(string name);

        public abstract void SetPrice(float price);
    }
}
