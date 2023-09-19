using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopUnknownLoad : ShopLoad
    {
        public override void UpdateTab()
        {
            List<ShopData> list = GameDataInit.data.shopData.Where(x => x.itemID != 0 && x.upgradeType != UpgradeType.None).ToList();
            DefaultTabs(list);
        }
    }
}