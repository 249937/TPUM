using TPUM.Logic;

namespace TPUM.Presentation
{
    internal class Model
    {
        private readonly ShopServiceAbstract _shopService;

        internal Model(ShopServiceAbstract shopservice = null)
        {
            _shopService = shopservice;
        }
    }
}
