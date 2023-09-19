using GameMenu.Inventory.Cards;
using UnityEngine;
using Universal;

namespace GameMenu.Inventory.Storages
{
    public class InventoryPreviewCardsStorage : Storage
    {
        #region fields & properties
        public static InventoryPreviewCardsStorage instance { get; private set; }
        [field: SerializeField] public Sprite groundCreationPanelSprite;
        [field: SerializeField] public Sprite flyingCreationPanelSprite;
        [field: SerializeField] public Sprite underwaterCreationPanelSprite;
        [field: SerializeField] public GameObject mainPreviewCard;
        [field: SerializeField] public Sprite[] cardLayoutSprites;
        #endregion fields & properties;

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        #endregion methods
    }
}