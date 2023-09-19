using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryCardsList : CardItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.data.cardsData.Where(x => !x.onDesk && !x.onHeal).ToList();
            UpdateListDefault(cardsData, x => x.listPosition);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            foreach(var el in currentPositions)
                el.OnListUpdate(el.listParam);
        }
    }
}