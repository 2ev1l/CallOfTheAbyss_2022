using GameMenu.Potions;
using UnityEngine.EventSystems;
using Universal;

namespace GameMenu.Inventory.Other
{
    public class InventoryPotion : PotionInit, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region methods
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!CanChangeCursor() || eventData.button != PointerEventData.InputButton.Left) return;
            CursorSettings.instance.DoClickSound();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanChangeCursor()) return;
            CursorSettings.instance.SetPointCursor();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            CursorSettings.instance.SetDefaultCursor();
        }
        public void AddToDesk()
        {
            if (GameDataInit.deskPotions.Count >= GameDataInit.data.maxPotionSize) return;
            GameDataInit.data.potionsData[listPosition].onDesk = true;
            GameDataInit.data.potionsData[listPosition].deskPosition = GameDataInit.MaxDeskPotionPosition() + 1;
            ItemList centerIL = InventoryPanelInit.instance.inventoryPotionsCenter;
            ItemList rightIL = InventoryPanelInit.instance.inventoryDeskPotions;
            centerIL.RemoveAtListParam(listParam, true, true);
            rightIL.GetUpdatedObject(GameDataInit.data.potionsData[listPosition].deskPosition, out ItemList.IListUpdater listUpdater);
            rightIL.Add(listUpdater, true, true);
            InventoryPanelInit.instance.OnPotionSizeChanged?.Invoke();
        }
        private bool CanChangeCursor() => GameDataInit.data.maxPotionSize > GameDataInit.deskPotions.Count;
        #endregion methods
    }
}