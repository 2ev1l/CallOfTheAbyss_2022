using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameMenu
{
    public class OpenForPricesInit : SingleSceneInstance
    {
        [SerializeField] private List<OpenForPrice> openForPrices;

        protected override void Awake()
        {
            CheckInstances(GetType());
        }
        private void OnEnable()
        {
            GameDataInit.instance.OnCoinsChanged += InitPrices;
        }
        private void OnDisable()
        {
            GameDataInit.instance.OnCoinsChanged -= InitPrices;
        }
        private void InitPrices()
        {
            foreach (var el in openForPrices)
                el.Init();
        }
    }
}