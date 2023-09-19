using Data;
using System.Linq;
using System.Collections.Generic;
using Universal;

namespace GameMenu.Shop
{
    public class ShopPotionsLoad : ShopLoad
    {
        #region methods
        public override void UpdateTab()
        {
            List<ShopData> list = GameDataInit.data.shopData.Where(x => x.lootType == LootType.Potion).ToList();
            DefaultTabs(list);
        }
        #endregion methods
    }
}