using Data;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace GameMenu.Artifacts
{
    public class ArtifactInit : MonoBehaviour, ItemList.IListUpdater
    {
        #region fields & properties;
        public ArtifactInfoSO artifactInfo { get; private set; }
        public UnityAction OnValuesUpdate;
        public int listPosition { get; private set; } = -1;
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;
        #endregion fields & properties;

        public void UpdateValues(int listPosition)
        {
            artifactInfo = PrefabsData.instance.artifactPrefabs[GameDataInit.data.artifactsData[listPosition].id];
            this.listPosition = listPosition;
            OnValuesUpdate?.Invoke();
        }
        public void UpdateValuesById(int id)
        {
            artifactInfo = PrefabsData.instance.artifactPrefabs[id];
            OnValuesUpdate?.Invoke();
        }

        public void OnListUpdate(int param) => UpdateValues(param);
    }
}