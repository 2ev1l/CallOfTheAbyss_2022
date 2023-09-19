using GameMenu.Inventory.ItemLists;
using UnityEngine;
using UnityEngine.EventSystems;
using Universal;
using GameMenu.Potions;
using UnityEngine.UI;
using Data;

namespace GameMenu.Inventory.Other
{
    public class ShowPotionPrefab : ShowObject, IPointerClickHandler, ItemList.IListUpdater
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
            PotionData potion = GameDataInit.deskPotion(deskPosition);
            mainText.text = TextOutline.languageData.potionsNameData[(int)potion.effect];
            textOutline.SetAll();
            listPosition = potion.listPosition;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            RemoveFromDesk();
        }
        public override GameObject SpawnObject()
        {
            PotionInit newObject = base.SpawnObject().GetComponent<PotionInit>();
            newObject.UpdateValues(listPosition);
            newObject.gameObject.SetActive(true);
            return newObject.gameObject;
        }
        private void RemoveFromDesk()
        {
            GameDataInit.RemovePotionFromDesk(listPosition);
            ItemList rightIL = InventoryPanelInit.instance.inventoryDeskPotions;
            ItemList centerIL = InventoryPanelInit.instance.inventoryPotionsCenter;
            FindObjectOfType<InventoryPotionsList>().GetUpdatedObject(listPosition, out ItemList.IListUpdater listUpdater);
            centerIL.Add(listUpdater, true, true);
            rightIL.RemoveAtListParam(listPosition, true, true);
            DestroyChild();
            InventoryPanelInit.instance.OnPotionSizeChanged?.Invoke();
        }

        public void OnListUpdate(int param) => UpdateValues(param);
        #endregion methods
    }
}
