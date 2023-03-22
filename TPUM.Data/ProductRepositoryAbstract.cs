using System.Collections.Generic;
using System;

namespace TPUM.Data
{
    public abstract class ProductRepositoryAbstract
    {
        private static ProductRepositoryAbstract instance;

        protected ProductRepositoryAbstract()
        {
        }

        public static ProductRepositoryAbstract Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductRepository();
                }
                return instance;
            }
        }

        public abstract void Add(ProductAbstract product);

        public abstract ProductAbstract Get(Guid productGuid);

        public abstract List<ProductAbstract> GetAll();

        public abstract void Remove(Guid productGuid);
    }
}
