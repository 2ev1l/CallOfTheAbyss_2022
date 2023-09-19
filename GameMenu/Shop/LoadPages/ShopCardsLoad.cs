using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public sealed class ShopCardsLoad : ShopLoad
    {
        #region methods
        public override void UpdateTab()
        {
            List<ShopData> list = new List<ShopData>();
            list = GameDataInit.data.shopData.Where(x => x.lootType == LootType.Card).ToList();
            if (list.Find(x => x.discount > 0) != null)
                DefaultTab(list.Find(x => x.discount > 0), 0);
            else
                DefaultTab(new ShopData(), 0);

            list = list.Where(x => x.discount == 0).ToList();
            int c = 1;
            foreach (ShopData el in list)
            {
                DefaultTab(el, c);
                c++;
            }
            RemoveShopData(c);
        }
        #endregion methods
    }
}