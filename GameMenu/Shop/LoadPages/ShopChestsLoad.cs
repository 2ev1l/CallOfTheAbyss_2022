using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopChestsLoad : ShopLoad
    {
        public override void UpdateTab()
        {
            List<ShopData> list = new List<ShopData>();
            list = GameDataInit.data.shopData.Where(x => x.lootType == LootType.Chest).ToList();
            DefaultTabs(list);
        }
    }
}