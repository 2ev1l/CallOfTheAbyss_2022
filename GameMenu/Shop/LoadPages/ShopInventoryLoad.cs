using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopInventoryLoad : ShopLoad
    {
        public override void UpdateTab()
        {
            List<ShopData> list = new List<ShopData>();
            list = GameDataInit.data.shopData.Where(x => x.sizeType != SizeType.None && x.sizeType != SizeType.House).ToList();
            DefaultTabs(list);
        }
    }
}