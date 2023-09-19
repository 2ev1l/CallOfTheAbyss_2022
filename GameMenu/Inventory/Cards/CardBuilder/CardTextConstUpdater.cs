using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public class CardTextConstUpdater : TextUpdater
    {
        #region fields
        [SerializeField] private CardMenuInit cardMenuInit;
        [SerializeField] private Text atkPriorityText;
        [SerializeField] private Text defPriorityText;
        #endregion fields

        #region methods
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
        protected virtual void SetText(List<CardData> cardDatas, int listPosition, int id)
        {
            atkPriorityText.text = $"{cardMenuInit.cardInfo.attackPriority}:";
            defPriorityText.text = $":{cardMenuInit.cardInfo.defensePriority}";
        }
        #endregion methods
    }
}