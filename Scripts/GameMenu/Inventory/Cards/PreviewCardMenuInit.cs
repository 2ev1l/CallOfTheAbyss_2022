using GameMenu.Inventory.Storages;
using UnityEngine.Events;

namespace GameMenu.Inventory.Cards
{
    public class PreviewCardMenuInit : CardMenuInit
    {
        #region fields
        public static UnityAction OnChoosedCardChange;
        public static PreviewCardMenuInit choosedCardToPreview;
        #endregion fields

        #region methods
        public void ChangeCardPreivew()
        {
            choosedCardToPreview = this;
            InventoryPreviewCardsStorage.instance.mainPreviewCard.SetActive(true);
            OnChoosedCardChange?.Invoke();
        }
        #endregion methods
    }
}
