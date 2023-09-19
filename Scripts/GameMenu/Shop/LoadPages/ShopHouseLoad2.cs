using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopHouseLoad2 : ShopLoad
    {
        #region methods
        public override void UpdateTab()
        {
            ShopData pos = GameDataInit.data.shopData.Find(x => x.itemID == 0 && x.upgradeType == UpgradeType.Other);
            DefaultTab(pos, 0);
        }
        #endregion methods

    }
}