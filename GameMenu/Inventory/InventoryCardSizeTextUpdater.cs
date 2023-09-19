using UnityEngine;
using Universal;

namespace GameMenu.Inventory
{
    public class InventoryCardSizeTextUpdater : TextUpdater
    {
        [SerializeField] private InventoryPanelInit inventoryPanelInit;

        protected override void OnEnable()
        {
            inventoryPanelInit.OnInventorySizeChanged += SetText;
        }
        protected override void OnDisable()
        {
            inventoryPanelInit.OnInventorySizeChanged -= SetText;
        }
        private void SetText()
        {
            txt.text = $"{GameDataInit.data.cardsData.Count}/{GameDataInit.data.maxInventorySize}";
        }
    }
}