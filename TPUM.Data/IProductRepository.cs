using System;
using System.Collections.Generic;

namespace TPUM.Data
{
    public interface IProductRepository
    {
        void Add(IProduct product);
        IProduct Get(Guid productGuid);
        List<IProduct> GetAll();
        void Remove(Guid productGuid);
    }
}
