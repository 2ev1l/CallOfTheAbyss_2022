using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public class CardTextDefenseUpdater : TextUpdater
    {
        [SerializeField] private CardMenuInit cardMenuInit;

        protected override void OnEnable()
        {
            if (!isEventsEnabled) return;
            cardMenuInit.OnValuesUpdate += SetText;
        }
        protected override void OnDisable()
        {
            if (!isEventsEnabled) return;
            cardMenuInit.OnValuesUpdate -= SetText;
        }
        public void SetText(List<CardData> cardDatas, int listPosition, int id) => SetDefaultText($":{cardDatas[listPosition].defense}");
    }
}