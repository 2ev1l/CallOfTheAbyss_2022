using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public sealed class LanguageLoadsCardUpdater : DefaultUpdater
    {
        [SerializeField] private CardMenuInit cardMenuInit;
        [SerializeField] private LanguageLoad onSelectLanguage;
        [SerializeField] private Text onSelectText;
        [SerializeField] private LanguageLoad nameLanguage;

        protected override void OnEnable()
        {
            cardMenuInit.OnValuesUpdate += Init;
            cardMenuInit.OnCardClick += DisableNameText;
            cardMenuInit.OnCardEnter += DisableNameText;
            cardMenuInit.OnCardExit += EnableNameText;
        }
        protected override void OnDisable()
        {
            cardMenuInit.OnValuesUpdate -= Init;
            cardMenuInit.OnCardClick -= DisableNameText;
            cardMenuInit.OnCardEnter -= DisableNameText;
            cardMenuInit.OnCardExit -= EnableNameText;
        }
        private void EnableNameText() => nameLanguage.gameObject.SetActive(true);
        private void DisableNameText() => nameLanguage.gameObject.SetActive(false);
        private void Init(List<CardData> cardDatas, int listPosition, int id)
        {
            bool isCardHasDescription = cardMenuInit.HasDescription();
            if (isCardHasDescription)
                onSelectLanguage.ChangeID(id);
            onSelectText.enabled = isCardHasDescription;
            nameLanguage.ChangeID(id);
        }
    }
}