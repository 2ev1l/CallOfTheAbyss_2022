using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu
{
    public class ElevatorInit : SingleSceneInstance
    {
        #region fields
        public static ElevatorInit instance { get; private set; }
        [SerializeField] private GameObject arrowNext;
        [SerializeField] private GameObject arrowPrev;
        [SerializeField] private Text layerText;
        [SerializeField] private OpenAtUpgrade openAtUpgrade;
        private int layerID = 0;
        private int maxLayerID => GameDataInit.data.upgradeData.Find(x => x.id == 1).tier;
        #endregion fields

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
            SetDefaultElevatorLayer();
        }
        public void CheckUpgrade()
        {
            if (openAtUpgrade.CheckUpgrade())
                SetDefaultElevatorLayer();
        }
        private void SetDefaultElevatorLayer() => SetElevatorLayer(GameDataInit.data.locationOffset);
        private void SetElevatorLayer(int layer)
        {
            layerID = Mathf.Clamp(layer, 0, maxLayerID);
            arrowPrev.SetActive(layerID != 0);
            arrowNext.SetActive(layerID != maxLayerID);
            layerText.text = layerID.ToString();
            GameDataInit.data.locationOffset = layerID;
        }
        public void ChangeElevatorLayer(bool isUp) => SetElevatorLayer(layerID + (isUp ? 1 : -1));
        #endregion methods
    }
}