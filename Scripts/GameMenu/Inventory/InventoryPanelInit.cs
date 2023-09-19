using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace GameMenu.Inventory
{
    public class InventoryPanelInit : SingleSceneInstance
    {
        public static InventoryPanelInit instance { get; private set; } = null;

        public UnityAction OnDeskSizeChanged;
        public UnityAction OnInventorySizeChanged;
        public UnityAction OnPotionSizeChanged;

        [SerializeField] private InventoryStateMachine inventoryStateMachine;
        [field: SerializeField] public ItemList inventoryCardsCenter { get; private set; }
        [field: SerializeField] public ItemList inventoryDeskCards { get; private set; }
        [field: SerializeField] public ItemList inventoryCardsTrash { get; private set; }
        [field: SerializeField] public ItemList inventoryPreviewCardsCenter { get; private set; }
        [field: SerializeField] public ItemList inventoryChests { get; private set; }
        [field: SerializeField] public ItemList inventoryPotionsCenter { get; private set; }
        [field: SerializeField] public ItemList inventoryDeskPotions { get; private set; }
        [field: SerializeField] public ItemList inventoryArtifactsCenter { get; private set; }
        [field: SerializeField] public ItemList inventoryDeskArtifacts { get; private set; }
        public static string[] cardsNameData;

        private IEnumerator Start()
        {
            cardsNameData = GameDataInit.GetCardsName();
            yield return CustomMath.WaitAFrame();
            UpdatePanelsDefault();
            OnDeskSizeChanged?.Invoke();
        }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        public void UpdatePanelsDefault()
        {
            inventoryStateMachine.ApplyDefaultState();
            SetInventoryStatesAvailability();
            GameDataInit.instance.OnTrashCardsChanged?.Invoke();
            OnDeskSizeChanged?.Invoke();
        }
        public void SetInventoryStatesAvailability()=> inventoryStateMachine.SetStatesAvailability();
    }
}