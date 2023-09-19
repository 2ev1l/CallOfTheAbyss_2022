using Universal;
using GameMenu.Potions;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu.Shop
{
    public class ShopPotion : ShopObject
    {
        #region fields
        [SerializeField] private PotionInit potionInit;
        [SerializeField] private Text potionText;
        #endregion fields

        #region methods
        public override void Init()
        {
            base.Init();
            potionInit.UpdateValuesById(shopData.itemID);
        }
        public override void BuyItem()
        {
            if (!canBuy) return;
            base.BuyItem();
            GameDataInit.AddPotion(shopData.itemID, false);
        }
        protected override void ActivateObject(bool f)
        {
            base.ActivateObject(f);
            potionText.enabled = f;
        }
        #endregion methods
    }
}