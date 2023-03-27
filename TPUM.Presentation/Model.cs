using TPUM.Logic;

namespace TPUM.Presentation.Model
{
    internal class MainModel
    {
        private readonly ShopServiceAbstract shopService;

        internal MainModel(ShopServiceAbstract shopService = null)
        {
            this.shopService = shopService;
        }
    }
}
