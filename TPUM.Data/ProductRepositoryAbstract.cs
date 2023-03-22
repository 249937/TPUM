using System.Collections.Generic;
using System;

namespace TPUM.Data
{
    public abstract class ProductRepositoryAbstract
    {
        public abstract void Add(ProductAbstract product);

        public abstract ProductAbstract Get(Guid productGuid);

        public abstract List<ProductAbstract> GetAll();

        public abstract void Remove(Guid productGuid);
    }
}
