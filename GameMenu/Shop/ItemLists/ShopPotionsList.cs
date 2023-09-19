using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace GameMenu.Shop
{
    public class ShopPotionsList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<PotionData> potionsData = GameDataInit.data.potionsData.Where(x => !x.onDesk).ToList();
            UpdateListDefault(potionsData, x => x.listPosition);
        }
        #endregion methods
    }
}
