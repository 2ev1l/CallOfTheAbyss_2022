using GameMenu.Potions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Universal;

namespace GameFight.Equipment
{
    public class FightPotion : Buttons, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ItemList.IListUpdater
    {
        #region fields & properties
        public static UnityAction<FightPotion> OnPotionChoosed;
        public static UnityAction<FightPotion> OnPotionDeselect;

        [SerializeField] private PotionInit potionInit;
        public ShortPotionInfo potionInfo => new ShortPotionInfo(potionInit.potionInfo.effect, potionInit.potionInfo.value);

        public static FightPotion choosedPotion;
        public static bool isPotionChoosed => choosedPotion != null;
        public GameObject rootObject => gameObject;
        public int listParam => potionInit.listParam;
        #endregion fields & properties

        #region methods
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            ChoosePotion(eventData);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (isPotionChoosed && choosedPotion != this) return;
            base.OnPointerEnter(eventData);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (isPotionChoosed && choosedPotion != this) return;
            base.OnPointerExit(eventData);
        }
        private void ChoosePotion(PointerEventData eventData)
        {
            if (isPotionChoosed)
            {
                if (choosedPotion == this)
                {
                    choosedPotion = null;
                    OnPotionDeselect?.Invoke(this);
                    base.OnPointerClick(eventData);
                }
            }
            else
            {
                choosedPotion = this;
                OnPotionChoosed?.Invoke(this);
                base.OnPointerClick(eventData);
            }
        }
        public static void TryDeselectPotion()
        {
            if (choosedPotion == null) return;
            OnPotionDeselect?.Invoke(choosedPotion);
            choosedPotion = null;
        }
        public static void RemoveUsedPotion()
        {
            OnPotionDeselect?.Invoke(choosedPotion);
            GameDataInit.RemovePotionFromDesk(choosedPotion.potionInit.listPosition);
            choosedPotion = null;
            EquipmentPanelInit.instance.UpdatePotionsList();
            EquipmentPanelInit.instance.CheckPanelAvailability();
        }
        public void UpdateValues(int listPosition) => potionInit.UpdateValues(listPosition);
        public void OnListUpdate(int param)=>UpdateValues(param);
        #endregion methods
    }
}
