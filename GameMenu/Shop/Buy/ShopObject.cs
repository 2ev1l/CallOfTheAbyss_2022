using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopObject : MonoBehaviour
    {
        #region fields
        [SerializeField] protected Image mainImage;
        [SerializeField] private Text priceText;
        [HideInInspector] public ShopData shopData = new ShopData();
        [HideInInspector] public int indexPosition;
        protected bool canBuy;
        #endregion fields

        #region methods
        public virtual void Start()
        {
            shopData ??= new ShopData();
            if (shopData.itemID == -1) return;
            if (CanInit())
                Init();
            CheckPrice();
        }
        public virtual void Init()
        {
            ActivateObject(true);
            SetPriceColor();
            if (shopData.discount != 0)
            {
                priceText.text += $"\n (-{shopData.discount}%)";
            }
            CheckPrice();
            CheckShowHelp();
        }
        public virtual bool CanInit()
        {
            if (shopData == null || shopData.owned || shopData.itemID < 0)
            {
                ActivateObject(false);
                return false;
            }
            return true;
        }
        protected virtual void ActivateObject(bool f)
        {
            if (!f) canBuy = false;
            mainImage.enabled = f;
            mainImage.gameObject.GetComponent<Button>().enabled = f;
            priceText.enabled = f;
        }
        protected virtual void ActivateOnPrice(bool f)
        {
            canBuy = f;
            mainImage.gameObject.GetComponent<Button>().enabled = f;
            SetPriceColor();
            CheckShowHelp();
        }
        private void CheckShowHelp()
        {
            if (!mainImage.TryGetComponent(out ShowHelp shelp)) return;
            shelp.id = canBuy ? 54 : 55;
        }
        public virtual void BuyItem()
        {
            if (!canBuy) return;
            GameDataInit.AddSilver(-GetDiscountPrice(shopData.priceSilver, shopData.discount), false);
            GameDataInit.AddGold(-GetDiscountPrice(shopData.priceGold, shopData.discount), false);
            shopData.owned = true;
            GameDataInit.data.shopData[indexPosition] = shopData;
            ActivateObject(false);
        }
        public void CheckPrice()
        {
            ActivateOnPrice(GameDataInit.data.coinsSilver >= GetDiscountPrice(shopData.priceSilver, shopData.discount) && GameDataInit.data.coinsGold >= GetDiscountPrice(shopData.priceGold, shopData.discount));
        }
        private void SetPriceColor()
        {
            try
            {
                priceText.text = "";
                int finalSilverPrice = GetDiscountPrice(shopData.priceSilver, shopData.discount);
                int finalGoldPrice = GetDiscountPrice(shopData.priceGold, shopData.discount);
                PriceParser.InsertSilverPrice(priceText, finalSilverPrice, true);
                if (finalSilverPrice > 0 && finalGoldPrice > 0)
                    priceText.text += " ";
                PriceParser.InsertGoldPrice(priceText, finalGoldPrice, true);
                if (shopData.discount > 0)
                    priceText.text += $"\r\n-{shopData.discount}%";
            }
            catch { print("e"); }
        }
        private int GetDiscountPrice(int defaultPrice, int discount) => (int)(defaultPrice * ((100 - discount) / 100f));
        #endregion methods
    }
}