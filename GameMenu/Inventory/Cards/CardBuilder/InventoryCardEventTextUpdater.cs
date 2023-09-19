using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public class InventoryCardEventTextUpdater : DefaultUpdater
    {
        [SerializeField] private CardMenuInit cardMenuInit;
        [SerializeField] private Text eventText;
        [SerializeField] private LanguageLoad eventLanguage;
        [SerializeField] private Text nameText;
        private Color defaultEventColor => new Color(225 / 255f, 1f, 220 / 255f, 1f);
        private Color badEventColor => new Color(1f, 0.1f, 0.2f, 1f);

        protected override void OnEnable()
        {
            InventoryPanelInit.instance.OnDeskSizeChanged += SetTextColor;
            cardMenuInit.OnValuesUpdate += SetTextColor;
        }

        protected override void OnDisable()
        {
            InventoryPanelInit.instance.OnDeskSizeChanged -= SetTextColor;
            cardMenuInit.OnValuesUpdate -= SetTextColor;
        }
        private void SetTextColor(List<CardData> cardsData, int listPosition, int id)
        {
            SetTextColor();
        }
        private void SetTextColor()
        {
            bool isHPZero = cardMenuInit.isHPZero;
            SetTextByHP(isHPZero);
            if (!isHPZero)
            {
                SetTextByDeskSize();
                return;
            }
        }
        private void SetTextByDeskSize()
        {
            bool isDeskSizeFree = GameDataInit.deskCards.Count < GameDataInit.data.maxDeskSize;
            eventText.color = isDeskSizeFree ? defaultEventColor : badEventColor;
            eventText.raycastTarget = isDeskSizeFree;
            eventLanguage.ChangeID(isDeskSizeFree ? 6 : 51);
        }
        private void SetTextByHP(bool isHPZero)
        {
            eventText.color = isHPZero ? badEventColor : defaultEventColor;
            nameText.color = isHPZero ? badEventColor : Color.white;
            eventText.raycastTarget = !isHPZero;
            eventLanguage.ChangeID(isHPZero ? 4 : 6);
        }
    }
}