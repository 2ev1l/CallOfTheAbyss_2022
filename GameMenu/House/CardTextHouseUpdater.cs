using System.Collections.Generic;
using UnityEngine;
using Data;
using GameMenu.Inventory.Cards;
using GameMenu.Inventory.Cards.Builder;
using Universal;

namespace GameMenu.House
{
    public class CardTextHouseUpdater : DefaultUpdater
    {
        [SerializeField] protected CardMenuInit cardMenuInit;
        [SerializeField] protected CardTextHPUpdater cardTextHPUpdater;
        [SerializeField] protected CardTextDamageUpdater cardTextDamageUpdater;
        [SerializeField] protected CardTextDefenseUpdater cardTextDefenseUpdater;

        protected override void OnEnable()
        {
            cardMenuInit.OnValuesUpdate += SetText;
        }
        protected override void OnDisable()
        {
            cardMenuInit.OnValuesUpdate -= SetText;
        }

        private void SetText(List<CardData> cardDatas, int listPosition, int id)
        {
            cardTextHPUpdater.SetText(cardDatas, listPosition, id);
            cardTextDamageUpdater.SetText(cardDatas, listPosition, id);
            cardTextDefenseUpdater.SetText(cardDatas, listPosition, id);
            CardData cardData = cardDatas[listPosition];
            int maxHP = cardData.maxHP;
            int maxDamage = cardData.maxDamage;
            int maxDefense = cardData.maxDefense;
            SetMaxTextValues(cardDatas[listPosition], maxHP, maxDamage, maxDefense);
        }
        protected virtual void SetMaxTextValues(CardData cardData, int maxHP, int maxDamage, int maxDefense)
        {
            if (cardData.hp != maxHP)
                cardTextHPUpdater.AddText($"\n({maxHP})");

            if (cardData.damage != maxDamage)
                cardTextDamageUpdater.AddText($"\n({maxDamage})");

            if (cardData.defense != maxDefense)
                cardTextDefenseUpdater.AddText($"\n({maxDefense})");
        }
    }
}