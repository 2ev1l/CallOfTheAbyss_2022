using Data;
using System.Collections.Generic;
using System.Linq;
using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryArtifactsList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<ArtifactData> potionData = GameDataInit.data.artifactsData.Where(x => !x.onDesk).ToList();
            UpdateListDefault(potionData, x => x.listPosition);
        }
        #endregion methods
    }
}