using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;
using GameMenu.Inventory.Cards;

namespace GameMenu.Inventory.ItemLists
{
    public class InventoryTrashCardsList : CardItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.data.cardsOnTrash;
            UpdateListDefault(cardsData, x => x.listPosition);
        }
    }
}