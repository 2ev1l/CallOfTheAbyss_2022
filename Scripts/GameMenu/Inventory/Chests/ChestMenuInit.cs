using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Data;
using Universal;
using GameMenu.Inventory.Cards;
using GameMenu.Inventory.Storages;

namespace GameMenu.Inventory.Chests
{
    public class ChestMenuInit : ChestInit, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region fields & properties
        [SerializeField] private ChestLootGenerator chestLootGenerator;
        #endregion fields & properties

        #region methods
        public void OnPointerClick(PointerEventData eventData)
        {
            if (ChestLootGenerator.isAnimation ||
                eventData.button != PointerEventData.InputButton.Left ||
                !eventData.pointerCurrentRaycast.gameObject.name.Equals("Picture")) return;
            CursorSettings.instance.DoClickSound();
            chestLootGenerator.OnChestOpened?.Invoke();
            CursorSettings.instance.SetDefaultCursor();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SceneLoader.IsBlackScreenFade() || ChestLootGenerator.isAnimation)
                return;
            CursorSettings.instance.SetPointCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorSettings.instance.SetDefaultCursor();
        }
        #endregion methods
    }

    [System.Serializable]
    public class ChestSpawned
    {
        public GameObject obj;
        public LootType lootType = LootType.None;
        public CardPlaceType cardPlaceType = CardPlaceType.Inventory;
        public int id = 0;
        public int listPosition = 0;
    }
    [System.Serializable]
    public class ChestLoot
    {
        public int id = 0;
        public LootType type = LootType.None;

        //DO NOT MODIFY MANUALLY
        [HideInInspector] public int location = 0;
        [HideInInspector] public int rare = 0;
        [HideInInspector] public int tier = 0;
    }
    [System.Serializable]
    public class ChestChances
    {
        [Range(0, 100)]
        public int rareChance = 0;
        [Range(0, 100)]
        public List<int> tiersChance = new List<int>();
    }
}