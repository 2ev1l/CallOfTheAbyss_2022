using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;
using System.Linq;

namespace GameMenu.Inventory.Cards
{
    public class TrashCardMenuInit : CardMenuInit
    {
        private int trashStartTime;
        protected override void Start()
        {
            Invoke(nameof(UpdateTrashTime), 0f);
            base.Start();
        }
        protected override void UpdateValues() => UpdateValues(GameDataInit.data.cardsOnTrash);
        protected override void UpdateValues(List<CardData> cardDatas)
        {
            if (id == -1) return;
            base.UpdateValues(cardDatas);
            trashStartTime = cardDatas[listPosition].trashStartTime;
        }
        private void UpdateTrashTime()
        {
            int maxHoldTime = 600;
            int remainingTime = Mathf.Clamp(maxHoldTime - (GameDataInit.data.trashTime - trashStartTime), 0, maxHoldTime);
            int min = remainingTime / 60;
            int sec = remainingTime % 60;
            Transform trashTimer = gameObject.transform.Find("TrashTimer");
            if (trashTimer != null)
            {
                Text trashTimerText = trashTimer.GetComponent<Text>();
                trashTimerText.text = min.ToString("00") + ":" + sec.ToString("00");
                trashTimerText.color = new Color(1f, ((float)remainingTime / maxHoldTime), ((float)remainingTime / maxHoldTime), 1f);
            }
            else return;
            if (remainingTime <= 1)
                DeleteCard();
            else
                Invoke(nameof(UpdateTrashTime), 1f);
        }
        public void DeleteCard()
        {
            ItemList il = InventoryPanelInit.instance.inventoryCardsTrash;
            il.RemoveAtListParam(listParam, true, true);
            GameDataInit.RemoveCard(listPosition, CardPlaceType.Trash);
            il.UpdateListData();
        }
        public void AddToInventory()
        {
            if (GameDataInit.data.cardsData.Count >= GameDataInit.data.maxInventorySize) return;
            ItemList il = InventoryPanelInit.instance.inventoryCardsTrash;
            il.RemoveAtListParam(listParam, true, true);

            GameDataInit.AddCard(id, false);
            GameDataInit.RemoveCard(listPosition, CardPlaceType.Trash);
            il.UpdateListData();
            InventoryPanelInit.instance.OnInventorySizeChanged?.Invoke();
        }
    }
}