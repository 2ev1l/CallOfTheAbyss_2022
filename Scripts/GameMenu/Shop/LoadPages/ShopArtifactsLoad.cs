using Data;
using System.Linq;
using System.Collections.Generic;
using Universal;

namespace GameMenu.Shop
{
    public class ShopArtifactsLoad : ShopLoad
    {
        #region methods
        public override void UpdateTab()
        {
            List<ShopData> list = GameDataInit.data.shopData.Where(x => x.lootType == LootType.Artifact).ToList();
            DefaultTabs(list);
        }
        #endregion methods
    }
}