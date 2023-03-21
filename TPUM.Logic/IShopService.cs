using System;

namespace TPUM.Logic
{
    public interface IShopService
    {
        void AddProduct(string name, float price);
        void RemoveProduct(Guid productGuid);
    }
}
