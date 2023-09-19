using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryChestList : ItemList
    {
        public override void UpdateListData()
        {
            UpdateListDefault(GameDataInit.data.chestsData, x => x);
        }
    }
}