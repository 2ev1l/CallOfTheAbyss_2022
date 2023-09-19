using UnityEngine.EventSystems;
using GameMenu.Inventory.Cards;

namespace GameMenu.Shop
{
    public class SellCardMenuInit : CardMenuInit
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            CursorChangeEvent(eventData, true);
            statsPanel.SetActive(false);
            onSelect.SetActive(true);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            CursorChangeEvent(eventData, false);
            statsPanel.SetActive(true);
            onSelect.SetActive(false);
        }
    }
}