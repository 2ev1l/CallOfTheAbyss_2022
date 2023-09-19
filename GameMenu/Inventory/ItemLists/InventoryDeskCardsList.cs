using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryDeskCardsList : CardItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.deskCards;
            List<CardData> sortedCards = cardsData.OrderBy(x => x.deskPosition).ToList();
            UpdateListDefault(sortedCards, x => x.deskPosition);
        }
    }
}