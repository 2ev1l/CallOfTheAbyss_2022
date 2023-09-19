using System.Collections.Generic;
using System.Linq;
using Data;
using Universal;

namespace GameMenu.House
{
    public class HouseCardsChooseList : ItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.data.cardsData.Where(card => !card.onDesk && !card.onHeal &&
            (card.hp < card.maxHP || card.damage < card.maxDamage || card.defense < card.maxDefense)).ToList();
            UpdateListDefault(cardsData, x => x.listPosition);
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            UpdateCurrentPage();
        }
    }
}