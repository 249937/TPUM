using System;

namespace TPUM.Data
{
    public interface IProduct
    {
        Guid GetGuid();
        string GetName();
        void SetName(string name);
        float GetPrice();
        void SetPrice(float price);
    }
}
