using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Inventory.Storages
{
    public sealed class InventoryCardStorage : Storage
    {
        public static InventoryCardStorage instance;
        [field: SerializeField] public Sprite groundType { get; private set; }
        [field: SerializeField] public Sprite waterType { get; private set; }
        [field: SerializeField] public Sprite skyType { get; private set; }
        [field: SerializeField] public Sprite[] inventoryCardLayoutSprites { get; private set; }
        [SerializeField] private GameObject iconDungeon;

        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void OnEnable()
        {
            InventoryPanelInit.instance.OnDeskSizeChanged += CheckDungeonAllow;
        }
        private void OnDisable()
        {
            InventoryPanelInit.instance.OnDeskSizeChanged -= CheckDungeonAllow;
        }
        private void CheckDungeonAllow()
        {
            bool allow = GameDataInit.deskCards.Count != 0;
            iconDungeon.GetComponent<ShowHelp>().id = allow ? 0 : 35;
            iconDungeon.GetComponent<Button>().enabled = allow;
            iconDungeon.GetComponent<Buttons>().enabled = allow;
        }
    }
}