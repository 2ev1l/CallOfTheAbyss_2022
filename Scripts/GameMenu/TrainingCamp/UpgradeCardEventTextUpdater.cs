using Data;
using GameMenu.Inventory.Cards;
using GameMenu.Shop;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.TrainingCamp
{
    public class UpgradeCardEventTextUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] private CardMenuInit cardMenuInit;
        [SerializeField] private Text eventText;
        [SerializeField] private Text priceText;
        [SerializeField] private LanguageLoad eventTextLanguage;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            cardMenuInit.OnValuesUpdate += delegate { UpdateEventText(); };
            GameDataInit.instance.OnCoinsChanged += UpdateEventText;
        }
        protected override void OnDisable()
        {
            cardMenuInit.OnValuesUpdate -= delegate { UpdateEventText(); };
            GameDataInit.instance.OnCoinsChanged -= UpdateEventText;
        }
        public void UpdateEventText()
        {
            bool canBuy = CheckPrice();
            if (eventText == null) return;
            Color defaultColor = new Color(1f, 251 / 255f, 220 / 255f);
            Color badColor = new Color(1f, 220 / 255f, 226 / 255f);
            eventText.color = canBuy ? defaultColor : badColor;
            eventTextLanguage.ChangeID(canBuy ? 62 : 63);
            eventText.raycastTarget = canBuy;
        }
        private bool CheckPrice()
        {
            if (priceText == null) return false;
            CardInfoSO cardInfo = cardMenuInit.cardInfo;
            priceText.text = "";
            PriceParser.InsertSilverPrice(priceText, cardInfo.upgradeSilverPrice, true);
            if (priceText.text != "")
                priceText.text += " ";
            PriceParser.InsertGoldPrice(priceText, cardInfo.upgradeGoldPrice, true);
            if (priceText.text != "")
                priceText.text += " ";
            int copiesCount = GameDataInit.CopiesCount(cardMenuInit.id);
            string copyPriceColor = cardInfo.upgradeDuplicatePrice > copiesCount ? "B24149" : "E2E3AA";
            priceText.text += $"<color=#{copyPriceColor}>{cardInfo.upgradeDuplicatePrice}</color>{TextOutline.languageData.interfaceData[55]}";
            if (cardInfo.upgradeSilverPrice > GameDataInit.data.coinsSilver) return false;
            if (cardInfo.upgradeGoldPrice > GameDataInit.data.coinsGold) return false;
            if (cardInfo.upgradeDuplicatePrice > copiesCount) return false;
            return true;
        }
        #endregion methods
    }
}