using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryDeskArtifactsList : ItemList
    {
        public override void UpdateListData()
        {
            UpdateListDefault(GameDataInit.deskArtifacts, x => x.deskPosition);
        }
    }
}