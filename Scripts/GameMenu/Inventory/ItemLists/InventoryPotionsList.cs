using Data;
using GameMenu.Potions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryPotionsList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<PotionData> potionData = GameDataInit.data.potionsData.Where(x => !x.onDesk).ToList();
            UpdateListDefault(potionData, x => x.listPosition);
        }
        #endregion methods
    }
}