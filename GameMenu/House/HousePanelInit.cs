using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace GameMenu.House
{
    public class HousePanelInit : SingleSceneInstance
    {
        public static HousePanelInit instance;
        public UnityAction OnHouseSizeChanged;
        public UnityAction OnHouseUpdate;
        [field: SerializeField] public GameObject houseChoosePanel { get; private set; }
        [field: SerializeField] public ItemList houseCardsChoose { get; private set; }
        [field: SerializeField] public HouseCardsHealList houseCardsHeal { get; private set; }
        [HideInInspector] public int lastHouseIndexPressed;
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            UpdateHouseLists();
            OnHouseSizeChanged?.Invoke();
        }
        public void UpdateHouseLists()
        {
            houseCardsHeal.UpdateListData();
            houseCardsChoose.UpdateListData();
        }
        public void UpdateHouseList(bool isHealPanel)
        {
            if (isHealPanel)
                houseCardsHeal.UpdateListData();
            else
                houseCardsChoose.UpdateListData();
            OnHouseUpdate?.Invoke();
        }
        public void UpdateHouseTimers()
        {
            foreach (ItemList.IListUpdater el in houseCardsHeal.items)
                if (el.rootObject.TryGetComponent(out HouseCardMenuInit cardMenuInit))
                    cardMenuInit.UpdateHouseTime();
        }
    }
}