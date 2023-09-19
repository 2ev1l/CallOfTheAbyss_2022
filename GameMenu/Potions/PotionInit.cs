using Data;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace GameMenu.Potions
{
    public class PotionInit : MonoBehaviour, ItemList.IListUpdater
    {
        #region fields & properties;
        public PotionInfoSO potionInfo { get; private set; }
        public UnityAction OnValuesUpdate;
        public int listPosition { get; private set; } = -1;

        public GameObject rootObject => gameObject;

        public int listParam => listPosition;
        #endregion fields & properties;

        public void UpdateValues(int listPosition)
        {
            potionInfo = PrefabsData.instance.potionPrefabs[GameDataInit.data.potionsData[listPosition].id];
            this.listPosition = listPosition;
            OnValuesUpdate?.Invoke();
        }
        public void UpdateValuesById(int id)
        {
            potionInfo = PrefabsData.instance.potionPrefabs[id];
            OnValuesUpdate?.Invoke();
        }

        public void OnListUpdate(int param) => UpdateValues(param);
    }
}
