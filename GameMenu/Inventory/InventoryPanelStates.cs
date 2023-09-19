using GameMenu.Inventory.Cards;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Inventory
{
    public sealed class InventoryPanelStates : StateChange
    {
        [SerializeField] private Sprite iconActive;
        [SerializeField] private Sprite iconUnActive;
        [SerializeField] private Image mainImage;
        [SerializeField] private GameObject panel;
        public string stateNameNormalized => gameObject.name.Remove(gameObject.name.Length - 8);

        public override void SetActive(bool active)
        {
            mainImage.sprite = active ? iconActive : iconUnActive;
            mainImage.raycastTarget = !active;
            panel.SetActive(active);
            if (!active) return;
            InventoryPanelInit inventoryPanelInit = InventoryPanelInit.instance;
            switch (stateNameNormalized)
            {
                case "Cards":
                    inventoryPanelInit.inventoryCardsCenter.UpdateListData();
                    inventoryPanelInit.inventoryDeskCards.UpdateListData();
                    inventoryPanelInit.OnDeskSizeChanged?.Invoke();
                    break;
                case "Trash":
                    inventoryPanelInit.inventoryCardsTrash.UpdateListData();
                    inventoryPanelInit.OnInventorySizeChanged?.Invoke();
                    break;
                case "Chests":
                    inventoryPanelInit.inventoryChests.UpdateListData();
                    break;
                case "Preview":
                    inventoryPanelInit.inventoryPreviewCardsCenter.UpdateListData();
                    if (PreviewCardMenuInit.choosedCardToPreview != null && PreviewCardMenuInit.choosedCardToPreview.listPosition < GameDataInit.data.cardsData.Count)
                        PreviewCardMenuInit.OnChoosedCardChange?.Invoke();
                    else if (MainPreviewCardMenuInit.instance != null)
                        MainPreviewCardMenuInit.instance.gameObject.SetActive(false);
                    inventoryPanelInit.OnInventorySizeChanged?.Invoke();
                    break;
                case "Potions":
                    inventoryPanelInit.inventoryPotionsCenter.UpdateListData();
                    inventoryPanelInit.inventoryDeskPotions.UpdateListData();
                    inventoryPanelInit.OnPotionSizeChanged?.Invoke();
                    break;
                case "Artifacts":
                    inventoryPanelInit.inventoryArtifactsCenter.UpdateListData();
                    inventoryPanelInit.inventoryDeskArtifacts.UpdateListData();
                    GameDataInit.instance.OnArtifactEffectsChanged?.Invoke();
                    break;
                default:
                    throw new System.NotImplementedException();
            };
        }
    }
}