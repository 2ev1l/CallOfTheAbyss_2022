using GameMenu.Inventory.Storages;
using UnityEngine;
using UnityEngine.UI;

namespace GameMenu.Inventory.Cards.Builder
{
    public class PreviewCardImagesUpdater : CardImagesUpdater
    {
        #region fields
        [SerializeField] private Image panelImage;
        #endregion fields

        #region methods
        protected override void ChangeLayoutImage()
        {
            layoutImage.sprite = InventoryPreviewCardsStorage.instance.cardLayoutSprites[cardMenuInit.cardInfo.rareTier];
        }
        protected override void ChangeGroundCreationValues()
        {
            base.ChangeGroundCreationValues();
            panelImage.sprite = InventoryPreviewCardsStorage.instance.groundCreationPanelSprite;
        }
        protected override void ChangeUnderwaterCreationValues()
        {
            base.ChangeUnderwaterCreationValues();
            panelImage.sprite = InventoryPreviewCardsStorage.instance.underwaterCreationPanelSprite;
        }
        protected override void ChangeFlyingCreationValues()
        {
            base.ChangeFlyingCreationValues();
            panelImage.sprite = InventoryPreviewCardsStorage.instance.flyingCreationPanelSprite;
        }
        #endregion methods
    }
}
