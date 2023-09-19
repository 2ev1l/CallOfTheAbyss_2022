using UnityEngine.EventSystems;
using Universal;
using GameMenu.Inventory.Chests;

namespace GameMenu.Inventory.Cards
{
    public class ChestCardMenuInit : CardMenuInit
    {
        private CardPlaceType cardPlaceType;
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (ChestLootGenerator.isAnimation) return;
            base.OnPointerClick(eventData);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            if (!CheckChestAnimation(true)) return;
            CursorChangeEvent(eventData, true);
            onSelect.SetActive(true);
            OnCardEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            if (!CheckChestAnimation(false)) return;
            CursorChangeEvent(eventData, false);
            HideSelectedPanels();
            OnCardExit?.Invoke();
        }
        protected bool CheckChestAnimation(bool afterEnable)
        {
            if (ChestLootGenerator.isAnimation)
            {
                if (HasDescription())
                    onSelect.SetActive(afterEnable);
                else
                    statsPanel.SetActive(afterEnable);
                return false;
            }
            else return true;
        }
        public void ChangeListPosition(int listPosition, CardPlaceType cardPlaceType)
        {
            if (listPosition < 0)
                gameObject.SetActive(false);
            this.cardPlaceType = cardPlaceType;
            this.listPosition = listPosition;
            UpdateValues();
        }
        protected override void UpdateValues()
        {
            switch (cardPlaceType)
            {
                case CardPlaceType.Inventory: UpdateValues(GameDataInit.data.cardsData); break;
                case CardPlaceType.Trash: UpdateValues(GameDataInit.data.cardsOnTrash); break;
                default: throw new System.NotImplementedException();
            }
        }
    }
}