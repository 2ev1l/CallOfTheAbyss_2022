using UnityEngine;
using UnityEngine.UI;
using Universal;
using GameMenu.Inventory.Cards;

namespace GameMenu.Shop
{
    [RequireComponent(typeof(CardMenuInit))]
    public class ShopCard : ShopObject
    {
        #region fields
        [SerializeField] private Image layout;
        [SerializeField] private CardMenuInit cardMenuInit;
        #endregion fields

        #region methods
        public override void Init()
        {
            base.Init();
            mainImage.sprite = PrefabsData.instance.cardPrefabs[shopData.itemID].cardBG;
            cardMenuInit.ChangeID(shopData.itemID);
        }

        protected override void ActivateObject(bool f)
        {
            base.ActivateObject(f);
            mainImage.transform.GetChild(0).GetComponent<Text>().enabled = f;
            layout.enabled = f;
            CardMenuInit cardMenuInit = GetComponent<CardMenuInit>();
            cardMenuInit.HideSelectedPanels();
            cardMenuInit.enabled = f;
        }
        public override void BuyItem()
        {
            if (!canBuy) return;
            base.BuyItem();
            GameDataInit.AddCard(shopData.itemID, false);
        }
        #endregion methods
    }
}