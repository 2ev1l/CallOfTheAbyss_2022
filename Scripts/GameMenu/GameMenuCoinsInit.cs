using System.Collections;
using UnityEngine;
using Universal;

namespace GameMenu
{
    public class GameMenuCoinsInit : SingleSceneInstance
    {
        public static GameMenuCoinsInit instance { get; private set; }
        [field: SerializeField] public GameObject silverCoins { get; private set; }
        [field: SerializeField] public GameObject goldCoins { get; private set; }

        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            UpdateCoinsFast();
        }
        private void OnEnable()
        {
            GameDataInit.instance.OnCoinsChanged += UpdateCoinsFast;
        }
        private void OnDisable()
        {
            GameDataInit.instance.OnCoinsChanged -= UpdateCoinsFast;
        }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        public void UpdateCoinsFast()
        {
            silverCoins.SetActive(true);
            goldCoins.SetActive(true);
            Vector3 toPosition = Vector3.right * (-1186) + Vector3.up * 466;
            toPosition = CustomAnimation.instance.UpdateIntCounterFast("IconSilver", GameDataInit.data.coinsSilver, false, 100, toPosition);
            CustomAnimation.instance.UpdateIntCounterFast("IconGold", GameDataInit.data.coinsGold, false, 100, toPosition);
        }
    }
}