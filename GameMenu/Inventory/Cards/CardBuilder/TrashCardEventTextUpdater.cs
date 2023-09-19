using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public class TrashCardEventTextUpdater : DefaultUpdater
    {
        [SerializeField] private CardMenuInit cardMenuInit;
        [SerializeField] private Text eventText;
        [SerializeField] private LanguageLoad eventLanguage;

        protected override void OnEnable()
        {
            InventoryPanelInit.instance.OnInventorySizeChanged += SetTextColor;
            cardMenuInit.OnValuesUpdate += SetTextColor;
        }

        protected override void OnDisable()
        {
            InventoryPanelInit.instance.OnInventorySizeChanged -= SetTextColor;
            cardMenuInit.OnValuesUpdate -= SetTextColor;
        }
        private void SetTextColor(List<CardData> cardsData, int listPosition, int id)
        {
            SetTextColor();
        }
        private void SetTextColor()
        {
            bool isInventoryFull = GameDataInit.data.maxInventorySize <= GameDataInit.data.cardsData.Count;
            eventText.color = !isInventoryFull ? new Color(225 / 255f, 1f, 220 / 255f) : new Color(1f, 220 / 255f, 230 / 255f);
            eventText.raycastTarget = !isInventoryFull;
            eventLanguage.ChangeID(!isInventoryFull ? 9 : 46);
        }
    }
}