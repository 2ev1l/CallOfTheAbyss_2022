using Data;
using GameMenu.Inventory.Cards.Builder;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Blacksmith
{
    public class BlacksmithCardTextUpdater : CardTextConstUpdater
    {
        #region fields
        [SerializeField] private BlacksmithCardMenuInit bCardMenuInit;
        [SerializeField] private Text eventTextHP;
        [SerializeField] private LanguageLoad languageTextHP;
        [SerializeField] private Text eventTextDMG;
        [SerializeField] private LanguageLoad languageTextDMG;
        [SerializeField] private Text eventTextDEF;
        [SerializeField] private LanguageLoad languageTextDEF;
        #endregion fields

        #region methods
        protected override void SetText(List<CardData> cardDatas, int listPosition, int id)
        {
            base.SetText(cardDatas, listPosition, id);
            CardData cardData = GameDataInit.data.cardsData[listPosition];
            languageTextHP.Load();
            languageTextDMG.Load();
            languageTextDEF.Load();
            int pricePerHPSilver = -1;
            int pricePerHPGold = -1;
            int pricePerDMGSilver = -1;
            int pricePerDMGGold = -1;
            int pricePerDEFSilver = -1;
            int pricePerDEFGold = -1;

            eventTextHP.enabled = (BlacksmithInit.instance.TryGetCardPricePerHP(cardData, out pricePerHPSilver, out pricePerHPGold));
            if (eventTextHP.enabled)
                InsertPrices(eventTextHP, pricePerHPSilver, pricePerHPGold);

            eventTextDEF.enabled = (BlacksmithInit.instance.TryGetCardPricePerDEF(cardData, out pricePerDEFSilver, out pricePerDEFGold));
            if (eventTextDEF.enabled)
                InsertPrices(eventTextDEF, pricePerDEFSilver, pricePerDEFGold);

            eventTextDMG.enabled = (BlacksmithInit.instance.TryGetCardPricePerDMG(cardData, out pricePerDMGSilver, out pricePerDMGGold));
            if (eventTextDMG.enabled)
                InsertPrices(eventTextDMG, pricePerDMGSilver, pricePerDMGGold);

            bCardMenuInit.SetPricePerHP(pricePerHPSilver, pricePerHPGold);
            bCardMenuInit.SetPricePerDMG(pricePerDMGSilver, pricePerDMGGold);
            bCardMenuInit.SetPricePerDEF(pricePerDEFSilver, pricePerDEFGold);
        }
        private void InsertPrices(Text txt, int priceSilver, int priceGold)
        {
            txt.text += ": ";
            Shop.PriceParser.InsertSilverPrice(txt, priceSilver, true);
            txt.text += " ";
            Shop.PriceParser.InsertGoldPrice(txt, priceGold, true);
        }
        #endregion methods
    }
}