using Data;
using GameMenu.Inventory.Cards;
using GameMenu.Inventory.ItemLists;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace GameMenu.TrainingCamp
{
    public class TrainingCampItemList : CardItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.data.cardsData.Where(x => !x.onDesk && !x.onHeal && PrefabsData.instance.cardPrefabs[x.id].upgradedCardID > 0).ToList();
            UpdateListDefault(cardsData, x => x.listPosition);
        }

        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            foreach (IListUpdater el in currentPositions)
                if (el.rootObject.TryGetComponent(out UpgradeCardEventTextUpdater eventTextUpdater))
                    eventTextUpdater.UpdateEventText();
        }
    }
}
