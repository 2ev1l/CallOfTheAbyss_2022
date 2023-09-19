using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Universal;
using GameMenu.Inventory.ItemLists;

namespace GameMenu.Shop
{
    public class ShopCardsList : CardItemList
    {
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.data.cardsData.Where(card => !card.onDesk && !card.onHeal).ToList();
            UpdateListDefault(cardsData, x => x.listPosition);
        }
    }
}