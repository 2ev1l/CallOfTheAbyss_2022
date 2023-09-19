using Data;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameEvent
{
    public class GameEventInit : SingleSceneInstance
    {
        #region fields & properties
        [SerializeField] private int eventIDToLoad;
        public static GameEventInit instance { get; private set; }
        [SerializeField] private GameObject[] onStartEnable;
        [SerializeField] private List<EventInfo> eventsInfo;
        [field: SerializeField] public List<VariationLanguageInfo> answersLanguageInfo { get; private set; } = new List<VariationLanguageInfo>();
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
            RemoveEventPoint();
            if (TryGetEventID(out int eventID))
            {
                LoadEvent(eventID);
                CheckAchievement(eventID);
            }
            else
                SceneLoader.instance.LoadScene("GameAdventure", 0f);

            foreach (var el in onStartEnable)
                el.SetActive(true);
        }
        private void LoadEvent(int eventID)
        {
            for (int i = 0; i < eventsInfo.Count; i++)
                eventsInfo[i].sceneEvent.SetActive(i == eventID);
        }
        private bool TryGetEventID(out int eventID)
        {
            eventID = -1;
            int currentLocation = GameDataInit.data.currentLocation;
            List<EventInfo> results = eventsInfo.FindAll(x => x.eventLocations.Contains(currentLocation));
            if (results.Count == 0) return false;
            eventID = eventsInfo.IndexOf(results[Random.Range(0, results.Count)]);
            return true;
        }
        private void RemoveEventPoint()
        {
            TempData location = GameDataInit.data.tempData[GameDataInit.data.currentLocation];
            try
            {
                int respawnedIndex = location.respawnedPoints.FindIndex(x => x.id == location.currentPoint);
                location.respawnedPoints.RemoveAt(respawnedIndex);
                if (!location.defeatedPoints.Contains(location.currentPoint))
                    location.defeatedPoints.Add(location.currentPoint);
                print($"respawned index {respawnedIndex} deleted");

            }
            catch
            {
                Debug.Log($"can't find respawned point at {GameDataInit.data.currentLocation} location in {location.currentPoint} point");
            }
        }
        private void CheckAchievement(int eventID) => Achievements.SetAchievement($"A_EVENT_{eventID}");
        #endregion methods

        [ContextMenu("LoadEvent")]
        private void LoadEvent()
        {
            CheckAchievement(eventIDToLoad);
            LoadEvent(eventIDToLoad);
        }

            [System.Serializable]
        private class EventInfo
        {
            [field: SerializeField] public EventVariations sceneEvent { get; private set; }
            [field: SerializeField] public List<int> eventLocations { get; private set; }
        }
        [System.Serializable]
        public class VariationLanguageInfo
        {
            [field: SerializeField] public VariationAnswer answer { get; private set; }
            [field: SerializeField] public int textID { get; private set; }
        }
    }
}