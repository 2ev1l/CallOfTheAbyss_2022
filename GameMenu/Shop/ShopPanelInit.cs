using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Universal;

namespace GameMenu.Shop
{
    public sealed class ShopPanelInit : SingleSceneInstance
    {
        #region fields
        public static ShopPanelInit instance;
        public UnityAction<int> OnShopTimerChanged;
        [SerializeField] private ShopLootGenerator shopLootGenerator;
        [SerializeField] private List<ShopPanels> shopLoads;
        [SerializeField] private List<GameObject> shopBuyPages;
        [SerializeField] private List<GameObject> shopSellPages;
        [SerializeField] private ItemList sellCards;
        [SerializeField] private ItemList sellChests;
        [SerializeField] private ItemList sellPotions;
        [SerializeField] private ItemList sellArtifacts;
        #endregion fields

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            UpdateShopTimer();
        }
        private void OnEnable()
        {
            GameDataInit.instance.OnCoinsChanged += UpdateShopPrices;
            OnShopTimerChanged += CheckShopTimer;
            shopLootGenerator.OnShopLootGenerated += UpdateAllShopData;
        }
        private void OnDisable()
        {
            GameDataInit.instance.OnCoinsChanged -= UpdateShopPrices;
            OnShopTimerChanged -= CheckShopTimer;
            shopLootGenerator.OnShopLootGenerated -= UpdateAllShopData;
        }
        private void UpdateAllShopData()
        {
            foreach (ShopPanels panel in shopLoads)
            {
                foreach (ShopLoad shopLoad in panel.shopLoads)
                {
                    shopLoad.UpdateTab();
                    UpdateShopTabPrices(shopLoad);
                }
            }
        }
        private void UpdateShopPrices()
        {
            foreach (ShopPanels panel in shopLoads)
                foreach (ShopLoad shopLoad in panel.shopLoads)
                    UpdateShopTabPrices(shopLoad);
        }
        public void UpdateShopTabPrices(ShopLoad loader)
        {
            foreach (ShopObject pos in loader.positions)
                if (pos.CanInit())
                    pos.Init();
        }
        public void UpdateShopCardsData() => sellCards.UpdateListData();
        public void UpdateShopChestsData() => sellChests.UpdateListData();
        public void UpdateShopPotionsData() => sellPotions.UpdateListData();
        public void UpdateShopArtifactsData() => sellArtifacts.UpdateListData();
        private void CheckShopTimer(int remainingTime)
        {
            int currentTime = GameDataInit.data.shopTime;
            if (remainingTime <= 0)
            {
                GameDataInit.data.lastShopUpdated = (currentTime / 600) + 1;
            }
            Invoke(nameof(UpdateShopTimer), 1);
        }
        private void UpdateShopTimer()
        {
            int remainingTime = 600 * GameDataInit.data.lastShopUpdated - GameDataInit.data.shopTime;
            OnShopTimerChanged?.Invoke(remainingTime);
        }
        #endregion methods

        [System.Serializable]
        private class ShopPanels
        {
            [field: SerializeField] public List<ShopLoad> shopLoads { get; private set; }
        }
    }
}