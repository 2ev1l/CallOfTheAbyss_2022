using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameMenu.Blacksmith
{
    public class BlacksmithInit : SingleSceneInstance
    {
        #region fields & properties
        public static BlacksmithInit instance { get; private set; }
        [field: SerializeField] public ItemList itemList { get; private set; }
        [field: SerializeField] public List<UpgradeInfo> upgradeInfos { get; private set; }
        private static float hpPriceScale = 1.3f;
        private static int dmgPriceScale = 2;
        private static int defPriceScale = 3;
        private static float goldPriceScale = 0.78f;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            itemList.UpdateListData();
        }
        public bool TryGetCardPricePerHP(CardData currentCard, out int silverPrice, out int goldPrice)
        {
            bool f = TryGetCardPricePerStats(currentCard, out int silverPricex, out int goldPricex, x => x.maxHP, y => y.hp, z => z.maxHPAdd);
            silverPrice = Mathf.RoundToInt(silverPricex * hpPriceScale);
            goldPrice = Mathf.RoundToInt(goldPricex * hpPriceScale * goldPriceScale);
            return f;
        }
        public bool TryGetCardPricePerDEF(CardData currentCard, out int silverPrice, out int goldPrice)
        {
            bool f = TryGetCardPricePerStats(currentCard, out int silverPricex, out int goldPricex, x => x.maxDefense, y => y.defense, z => z.maxDEFAdd);
            silverPrice = silverPricex * defPriceScale;
            goldPrice = Mathf.RoundToInt(goldPricex * defPriceScale * goldPriceScale);
            return f;
        }
        public bool TryGetCardPricePerDMG(CardData currentCard, out int silverPrice, out int goldPrice)
        {
            bool f = TryGetCardPricePerStats(currentCard, out int silverPricex, out int goldPricex, x => x.maxDamage, y => y.damage, z => z.maxDMGAdd);
            silverPrice = silverPricex * dmgPriceScale;
            goldPrice = Mathf.RoundToInt(goldPricex * dmgPriceScale * goldPriceScale);
            return f;
        }
        private bool TryGetCardPricePerStats(CardData currentCard, out int silverPrice, out int goldPrice, 
            System.Func<CardData, int> cardMaxStat, System.Func<CardInfoSO, int> cardInfoStat, System.Func<TierInfo, int> tierStat)
        {
            CardInfoSO cardInfo = PrefabsData.instance.cardPrefabs[currentCard.id];
            TierInfo tierInfo = GetCardTierInfo(cardInfo, out float priceDivision);
            silverPrice = goldPrice = 0;
            if (cardMaxStat.Invoke(currentCard) - cardInfoStat.Invoke(cardInfo) >= tierStat.Invoke(tierInfo)) return false;
            silverPrice = Mathf.RoundToInt( cardInfo.silverPrice / priceDivision);
            goldPrice = Mathf.RoundToInt(cardInfo.goldPrice / priceDivision);
            return true;
        }
        private TierInfo GetCardTierInfo(CardInfoSO cardInfo, out float priceDivision)
        {
            UpgradeInfo upgrade = upgradeInfos.Find(x => x.cardTier == cardInfo.rareTier);
            TierInfo tierInfo = upgrade.tierInfos.Find(x => x.cardUpgradeTier == cardInfo.upgradedTier);
            priceDivision = upgrade.priceDivision;
            return tierInfo;
        }
        #endregion methods

        [System.Serializable]
        public class UpgradeInfo
        {
            [field: SerializeField] public int cardTier { get; private set; }
            [field: SerializeField] public float priceDivision { get; private set; }
            [field: SerializeField] public List<TierInfo> tierInfos { get; private set; }
        }
        [System.Serializable]
        public class TierInfo
        {
            [field: SerializeField] public int cardUpgradeTier { get; private set; }
            [field: SerializeField] public int maxHPAdd { get; private set; }
            [field: SerializeField] public int maxDMGAdd { get; private set; }
            [field: SerializeField] public int maxDEFAdd { get; private set; }
        }
    }
}