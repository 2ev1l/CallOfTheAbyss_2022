using GameMenu.Artifacts;
using UnityEngine.EventSystems;
using Universal;

namespace GameMenu.Inventory.Other
{
    public class InventoryArtifact : ArtifactInit, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
            if (GameDataInit.deskArtifacts.Count >= GameDataInit.data.maxArtifactSize) return;
            GameDataInit.data.artifactsData[listPosition].onDesk = true;
            GameDataInit.data.artifactsData[listPosition].deskPosition = GameDataInit.MaxDeskArtifactPosition() + 1;
            ItemList centerIL = InventoryPanelInit.instance.inventoryArtifactsCenter;
            ItemList rightIL = InventoryPanelInit.instance.inventoryDeskArtifacts;
            centerIL.RemoveAtListParam(listParam, true, true);
            rightIL.GetUpdatedObject(GameDataInit.data.artifactsData[listPosition].deskPosition, out ItemList.IListUpdater listUpdater);
            rightIL.Add(listUpdater, true, true);
            GameDataInit.instance.OnArtifactEffectsChanged?.Invoke();
        }
        private bool CanChangeCursor() => GameDataInit.data.maxArtifactSize > GameDataInit.deskArtifacts.Count;
        #endregion methods
    }
}