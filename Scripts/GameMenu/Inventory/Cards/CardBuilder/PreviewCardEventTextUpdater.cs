using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public class PreviewCardEventTextUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] private PreviewCardMenuInit cardMenuInit;
        [SerializeField] private Text eventText;
        [SerializeField] private LanguageLoad eventLanguage;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            cardMenuInit.OnValuesUpdate += UpdateEvent;
            PreviewCardMenuInit.OnChoosedCardChange += UpdateEvent;
        }

        protected override void OnDisable()
        {
            cardMenuInit.OnValuesUpdate -= UpdateEvent;
            PreviewCardMenuInit.OnChoosedCardChange -= UpdateEvent;
        }
        public void UpdateEvent()
        {
            bool isChoosedCardThis = PreviewCardMenuInit.choosedCardToPreview == cardMenuInit;
            eventText.raycastTarget = !isChoosedCardThis;
            eventText.color = !isChoosedCardThis ? new Color(225 / 255f, 1f, 220 / 255f) : new Color(220 / 255f, 1f, 1f);
            eventLanguage.ChangeID(!isChoosedCardThis ? 49 : 50);
        }
        private void UpdateEvent(List<CardData> cardsData, int id, int listPosition) => UpdateEvent();
        #endregion methods
    }
}