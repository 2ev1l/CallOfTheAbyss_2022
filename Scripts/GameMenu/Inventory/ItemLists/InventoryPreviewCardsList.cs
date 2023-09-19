using Data;
using GameMenu.Inventory.Cards;
using GameMenu.Inventory.Cards.Builder;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryPreviewCardsList : ItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cards = GameDataInit.data.cardsData;
            UpdateListDefault(cards, card => card.listPosition);
        }

        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            foreach (IListUpdater el in currentPositions)
                if (el.rootObject.TryGetComponent(out PreviewCardEventTextUpdater eventTextUpdater))
                    eventTextUpdater.UpdateEvent();
        }
    }
}
