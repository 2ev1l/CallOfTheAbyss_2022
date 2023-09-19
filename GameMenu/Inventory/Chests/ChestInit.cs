using Data;
using GameMenu.Inventory.Storages;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace GameMenu.Inventory.Chests
{
    public class ChestInit : MonoBehaviour,  ItemList.IListUpdater
    {
        #region fields & properties
        public UnityAction OnInfoChange;
        
        [SerializeField] private LanguageLoad nameText;
        public ChestInfo chestInfo { get; private set; }

        public GameObject rootObject => this.gameObject;
        public int listParam => chestInfo.id;
        #endregion fields & properties;
        public void ChangeInfo(ChestInfo chestInfo)
        {
            this.chestInfo = chestInfo;
            OnInfoChange?.Invoke();
        }
        private void OnEnable()
        {
            OnInfoChange += UpdateValues;
        }
        protected void OnDisable()
        {
            OnInfoChange -= UpdateValues;
        }
        private void UpdateValues()
        {
            nameText.ChangeID((int)chestInfo.chestType);
            nameText.Load();
        }
        public void OnListUpdate(int param) => ChangeInfo(InventoryChestStorage.instance.chestPrefabs[param]);
    }
}