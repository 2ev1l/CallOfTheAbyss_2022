using Data;
using GameMenu.House;
using GameMenu.Inventory.Cards.Builder;
using System;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Inventory.Cards
{
    public class MainPreviewCardStatsUpdater : CardTextHouseUpdater
    {
        #region fields
        [SerializeField] private Text stateText;
        [SerializeField] private Text copiesText;
        [SerializeField] private Text rareText;
        [SerializeField] private ShowHelp[] valuesWithMaxParam;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            base.OnEnable();
            PreviewCardMenuInit.OnChoosedCardChange += UpdateAfterPreviewChange;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PreviewCardMenuInit.OnChoosedCardChange -= UpdateAfterPreviewChange;
        }
        private void UpdateAfterPreviewChange()
        {
            UpdateState();
            UpdateCopies();
            UpdateRare();
        }
        private void UpdateState()
        {
            CardData cardData = GameDataInit.data.cardsData[cardMenuInit.listPosition];
            stateText.text = "";
            if (cardData.onDesk)
            {
                AddInterfaceText(stateText, 53);
                return;
            }

            if (cardData.onHeal)
            {
                AddInterfaceText(stateText, 54);
                return;
            }

            AddInterfaceText(stateText, 52);
        }
        private void UpdateCopies()
        {
            copiesText.text = "";
            AddInterfaceText(copiesText, 55);
            copiesText.text += $": {GameDataInit.CopiesCount(cardMenuInit.id)}";
        }
        private void UpdateRare()
        {
            CardInfoSO cardInfo = PrefabsData.instance.cardPrefabs[cardMenuInit.id];
            rareText.text = "";
            int textId = 0;
            Color col = Color.white;
            switch (cardInfo.rareTier)
            {
                case 0:
                    textId = 56;
                    col = Color.white;
                    break;
                case 1:
                    textId = 57;
                    col = Color.yellow;
                    break;
                case 2:
                    textId = 58;
                    col = Color.green;
                    break;
                case 3:
                    textId = 59;
                    col = Color.cyan;
                    break;
                default: throw new System.NotImplementedException();
            }
            AddInterfaceText(rareText, textId);
            rareText.color = col;
        }
        private void AddInterfaceText(Text txt, int id) => txt.text += TextOutline.languageData.interfaceData[id];
        protected override void SetMaxTextValues(CardData cardData, int maxHP, int maxDamage, int maxDefense)
        {
            cardTextHPUpdater.AddText($"\n/{maxHP}");
            cardTextDamageUpdater.AddText($"\n/{maxDamage}");
            cardTextDefenseUpdater.AddText($"\n/{maxDefense}");
            foreach (ShowHelp showHelp in valuesWithMaxParam)
                showHelp.additionalText = $" / {TextOutline.languageData.interfaceData[60]}";
        }
        #endregion methods

    }
}