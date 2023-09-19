using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;

namespace GameMenu.Inventory.Storages
{
    public sealed class InventoryChestStorage : Storage
    {
        #region fields & properties
        public static InventoryChestStorage instance;
        [field: SerializeField] public GameObject[] objectsToOff { get; private set; }
        [field: SerializeField] public ChestInfo[] chestPrefabs { get; private set; }
        [field: SerializeField] public List<ChestTypeSprite> chestTypeSprites { get; private set; } = new List<ChestTypeSprite>();
        [field: SerializeField] public List<LootTypePrefab> lootTypePrefabs { get; private set; } = new List<LootTypePrefab>();
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        public Sprite GetChestSprite(int chestID) => FindChestSprite(instance.chestPrefabs[chestID]);
        public Sprite GetChestSprite(ChestInfo chestInfo) => FindChestSprite(chestInfo);
        private Sprite FindChestSprite(ChestInfo chestInfo) => instance.chestTypeSprites.Find(x => x.chestType == chestInfo.chestType).sprite;
        #endregion methods
    }

    [System.Serializable]
    public class LootTypePrefab
    {
        public GameObject prefab;
        public LootType lootType;
    }

    [System.Serializable]
    public class ChestTypeSprite
    {
        public Sprite sprite;
        public ChestType chestType;
    }
}