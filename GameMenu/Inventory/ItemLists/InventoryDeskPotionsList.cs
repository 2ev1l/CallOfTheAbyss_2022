using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryDeskPotionsList : ItemList
    {
        public override void UpdateListData()
        {
            UpdateListDefault(GameDataInit.deskPotions, x => x.deskPosition);
        }
    }
}