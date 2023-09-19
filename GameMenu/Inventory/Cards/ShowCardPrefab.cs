using UnityEngine;
using UnityEngine.EventSystems;
using Universal;
using UnityEngine.UI;
using Data;

namespace GameMenu.Inventory.Cards
{
    public class ShowCardPrefab : ShowObject, IPointerClickHandler, ItemList.IListUpdater
    {
        #region fields
        private int listPosition;
        [SerializeField] private TextOutline textOutline;
        [SerializeField] private Text mainText;
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;
        #endregion fields

        #region methods
        public void UpdateValues(int deskPosition)
        {
            CardData deskCard = GameDataInit.deskCard(deskPosition);
            listPosition = deskCard.listPosition;
            mainText.text = InventoryPanelInit.cardsNameData[deskCard.id];
            textOutline.SetAll();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            RemoveFromDesk();
        }
        public override GameObject SpawnObject()
        {
            CardMenuInit newObject = base.SpawnObject().GetComponent<CardMenuInit>();
            newObject.ChangeListPosition(listPosition);
            newObject.gameObject.SetActive(true);
            return newObject.gameObject;
        }
        private void RemoveFromDesk()
        {
            GameDataInit.RemoveCardFromDesk(listPosition);
            ItemList rightIL = InventoryPanelInit.instance.inventoryDeskCards;
            ItemList centerIL = InventoryPanelInit.instance.inventoryCardsCenter;
            centerIL.GetUpdatedObject(listPosition, out ItemList.IListUpdater listUpdater);
            centerIL.Add(listUpdater, true, true);
            rightIL.RemoveAtListParam(listParam, true, true);
            DestroyChild();
            InventoryPanelInit.instance.OnDeskSizeChanged?.Invoke();
        }

        public void OnListUpdate(int param) => UpdateValues(param);
        #endregion methods
    }
}