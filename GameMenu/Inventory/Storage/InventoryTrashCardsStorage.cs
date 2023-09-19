using UnityEngine;
using Universal;

namespace GameMenu.Inventory.Storages
{
    public sealed class InventoryTrashCardsStorage : Storage
    {
        public static InventoryTrashCardsStorage instance;
        [SerializeField] private GameObject[] trashAlertObjects;
        [SerializeField] private GameObject trashInventoryIcon;

        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void OnEnable()
        {
            GameDataInit.instance.OnTrashCardsChanged += UpdateTrashAlertObjects;
        }
        private void OnDisable()
        {
            GameDataInit.instance.OnTrashCardsChanged -= UpdateTrashAlertObjects;
        }
        private void UpdateTrashAlertObjects()
        {
            bool isTrashNotNull = GameDataInit.data.cardsOnTrash.Count != 0;
            foreach (var el in trashAlertObjects)
                el.SetActive(isTrashNotNull);
            if (isTrashNotNull)
                trashInventoryIcon.SetActive(true);
        }
    }
}