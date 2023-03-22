using System;
using System.Collections.Generic;

namespace TPUM.Data
{
    public interface IProductRepository
    {
        void Add(ProductAbstract product);
        ProductAbstract Get(Guid productGuid);
        List<ProductAbstract> GetAll();
        void Remove(Guid productGuid);
    }
}
