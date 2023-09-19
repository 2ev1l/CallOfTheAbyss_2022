using System.Collections.Generic;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopCardMenuInit : SellCardMenuInit
    {
        protected override void UpdateValues() => UpdateValues(new List<CardData>() { GetCardDataFromPrefab() });
        private CardData GetCardDataFromPrefab()
        {
            CardInfoSO cardInit = PrefabsData.instance.cardPrefabs[id];
            CardData cardData = new CardData();
            cardData.id = id;
            cardData.hp = cardInit.hp;
            cardData.damage = cardInit.damage;
            cardData.defense = cardInit.defense;
            cardData.listPosition = listPosition;
            return cardData;
        }
    }
}