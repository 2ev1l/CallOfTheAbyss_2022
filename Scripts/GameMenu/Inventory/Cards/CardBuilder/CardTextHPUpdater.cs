using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public class CardTextHPUpdater : TextUpdater
    {
        [SerializeField] private CardMenuInit cardMenuInit;

        protected override void OnEnable()
        {
            cardMenuInit.OnValuesUpdate += SetText;
        }
        protected override void OnDisable()
        {
            cardMenuInit.OnValuesUpdate -= SetText;
        }
        public void SetText(List<CardData> cardDatas, int listPosition, int id) => SetDefaultText($":{cardDatas[listPosition].hp}");

    }
}