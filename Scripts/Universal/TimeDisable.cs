using System.Collections;
using UnityEngine;

namespace Universal
{
    public class TimeDisable : MonoBehaviour
    {
        #region fields
        [SerializeField] private int disableTime;
        [SerializeField] private UpdateType updateType = UpdateType.Start;
        [SerializeField] private TimeType timeType = TimeType.Play;
        #endregion

        #region methods
        private void Awake()
        {
            if (updateType == UpdateType.Awake)
            {
                StartCoroutine(CheckTime(false));
            }
        }
        private void Start()
        {
            switch (updateType)
            {
                case UpdateType.Start:
                    StartCoroutine(CheckTime(false));
                    break;
                case UpdateType.Update:
                    StartCoroutine(CheckTime(true));
                    break;
            }
        }
        private IEnumerator CheckTime(bool isRepeating)
        {
            int timeValue = timeType switch
            {
                TimeType.Play => GameDataInit.data.playTime,
                TimeType.Shop => GameDataInit.data.shopTime,
                TimeType.Trash => GameDataInit.data.trashTime,
                _ => throw new System.NotImplementedException()
            };
            if (timeValue > disableTime)
            {
                gameObject.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1);
            if (isRepeating)
                StartCoroutine(CheckTime(true));
        }
        #endregion methods

        public enum UpdateType { Start, Update, Awake }
    }
    public enum TimeType { Play, Trash, Shop }
}