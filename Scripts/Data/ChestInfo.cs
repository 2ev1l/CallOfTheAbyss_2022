using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;
using GameMenu.Inventory.Storages;
using GameMenu.Inventory.Chests;

namespace Data
{
    [CreateAssetMenu(fileName = "ChestInfo", menuName = "ScriptableObjects/ChestInfo")]
    public class ChestInfo : ScriptableObject
    {
        [field: SerializeField] public int id { get; private set; }
        [field: SerializeField] public ChestType chestType { get; private set; }
        [field: SerializeField] public int silverPrice { get; private set; }
        [field: SerializeField] public int goldPrice { get; private set; }
        [field: SerializeField] public int maxLootCount { get; private set; }
        [field: SerializeField] public int chestLocation { get; private set; }
        [field: SerializeField] public bool visibleInShop { get; private set; }
        [field: SerializeField] public List<ChestLoot> chestLoot { get; private set; }
        [field: SerializeField] public List<ChestChances> chestChances { get; private set; }

        private void AddX(int location)
        {
            foreach (var el in PrefabsData.instance.cardPrefabs.Where(card => card.cardLocation == location))
            {
                ChestLoot loot = new ChestLoot();
                loot.type = LootType.Card;
                loot.id = el.id;
                chestLoot.Add(loot);
            }
        }
        [ContextMenu("Add all cards equals chest location")]
        private void Add1() => AddX(chestLocation);
        [ContextMenu("Add all cards equals chest location - 1")]
        private void Add2() => AddX(chestLocation - 1);
        [ContextMenu("Add all cards equals chest location + 1")]
        private void Add3() => AddX(chestLocation + 1);

        [ContextMenu("Add all potions equals chest location")]
        private void AddX1()
        {
            foreach (var el in PrefabsData.instance.potionPrefabs.Where(potion => potion.potionLocation == chestLocation))
            {
                ChestLoot loot = new ChestLoot();
                loot.type = LootType.Potion;
                loot.id = el.id;
                chestLoot.Add(loot);
            }
        }
        [ContextMenu("Add all artifacts equals chest location")]
        private void AddX2()
        {
            foreach (var el in PrefabsData.instance.artifactPrefabs.Where(art => art.artifactLocation == chestLocation))
            {
                ChestLoot loot = new ChestLoot();
                loot.type = LootType.Artifact;
                loot.id = el.id;
                chestLoot.Add(loot);
            }
        }
        [ContextMenu("Add all previous chests")]
        private void AddX3()
        {
            foreach (var el in InventoryChestStorage.instance.chestPrefabs.Where(x => x.id < id))
            {
                ChestLoot loot = new ChestLoot();
                loot.type = LootType.Chest;
                loot.id = el.id;
                chestLoot.Add(loot);
            }
        }
        [ContextMenu("Add chest to inventory")]
        private void Add4() => GameDataInit.AddChest(id, false);
    }
    public enum ChestType { Wooden, Bronze, Silver, Gold, Water, Bloody, Magic, Cloud, Hidden, Old, Ancient, Luxury, Azure, Royal, Black, God }
}