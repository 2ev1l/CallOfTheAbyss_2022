using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;
using GameMenu.Inventory.Cards;

namespace GameMenu.House
{
    public class HouseCardEventTextUpdater : DefaultUpdater
    {
        [SerializeField] private CardMenuInit cardMenuInit;
        [SerializeField] private Text eventText;
        [SerializeField] private LanguageLoad eventLanguage;
        protected override void OnEnable()
        {
            HousePanelInit.instance.OnHouseSizeChanged += SetTextColor;
            cardMenuInit.OnValuesUpdate += SetTextColor;
        }
        protected override void OnDisable()
        {
            HousePanelInit.instance.OnHouseSizeChanged -= SetTextColor;
            cardMenuInit.OnValuesUpdate -= SetTextColor;
        }
        private void SetTextColor(List<CardData> cardsData, int listPosition, int id)
        {
            SetTextColor();
        }
        private void SetTextColor()
        {
            bool isHouseFull = GameDataInit.data.maxHouseSize <= GameDataInit.data.cardsData.Where(card => card.onHeal).Count();
            eventText.color = !isHouseFull ? new Color(1f, 241 / 255f, 220 / 255f) : new Color(1f, 220 / 255f, 230 / 255f);
            eventText.raycastTarget = !isHouseFull;
            eventLanguage.ChangeID(!isHouseFull ? 11 : 47);
        }
    }
}