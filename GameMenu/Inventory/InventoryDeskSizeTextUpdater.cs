using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Inventory
{
    public class InventoryDeskSizeTextUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] private InventoryPanelInit inventoryPanelInit;
        [SerializeField] private Text cardsText;
        [SerializeField] private Text potionsText;
        [SerializeField] private Text artifactsText;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            inventoryPanelInit.OnDeskSizeChanged += DeskCardsText;
            inventoryPanelInit.OnPotionSizeChanged += DeskPotionsText;
            GameDataInit.instance.OnArtifactEffectsChanged += DeskArtifactsText;
        }

        protected override void OnDisable()
        {
            inventoryPanelInit.OnDeskSizeChanged -= DeskCardsText;
            inventoryPanelInit.OnPotionSizeChanged -= DeskPotionsText;
            GameDataInit.instance.OnArtifactEffectsChanged -= DeskArtifactsText;
        }
        private void DeskCardsText()
        {
            cardsText.text = $"{GameDataInit.deskCards.Count}/{GameDataInit.data.maxDeskSize}";
        }
        private void DeskPotionsText()
        {
            potionsText.text = $"{GameDataInit.deskPotions.Count}/{GameDataInit.data.maxPotionSize}";
        }
        private void DeskArtifactsText()
        {
            artifactsText.text = $"{GameDataInit.deskArtifacts.Count}/{GameDataInit.data.maxArtifactSize}";
        }
        #endregion methods
    }
}