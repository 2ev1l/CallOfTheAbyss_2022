using UnityEngine;
using Data;
using Universal;
using GameMenu.Inventory.Cards;

namespace GameMenu.Shop
{
    [RequireComponent(typeof(CardMenuInit))]
    public sealed class SellCard : SellObject, ItemList.IListUpdater
    {
        #region fields
        [SerializeField] private CardMenuInit cardMenuInit;
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;
        #endregion fields

        #region methods
        public override void Init()
        {
            cardMenuInit.ChangeListPosition(listPosition);
            CardInfoSO cardInit = PrefabsData.instance.cardPrefabs[cardMenuInit.id];
            priceGold = cardInit.goldPrice;
            priceSilver = cardInit.silverPrice;
            base.Init();
        }

        public override void SellItem()
        {
            base.SellItem();
            GameDataInit.data.cardsCopyData.Add(GameDataInit.data.cardsData[listPosition].id);
            GameDataInit.RemoveCard(listPosition, CardPlaceType.Inventory);
            ShopPanelInit.instance.UpdateShopCardsData();
        }

        public void OnListUpdate(int param) => ChangeListPosition(param);
        #endregion methods
    }
}