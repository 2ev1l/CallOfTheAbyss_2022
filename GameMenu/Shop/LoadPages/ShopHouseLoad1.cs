using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopHouseLoad1 : ShopLoad
    {
        #region methods
        public override void UpdateTab()
        {
            ShopData pos = GameDataInit.data.shopData.Find(x => x.sizeType == SizeType.House);
            DefaultTab(pos, 0);
        }
        #endregion methods

    }
}