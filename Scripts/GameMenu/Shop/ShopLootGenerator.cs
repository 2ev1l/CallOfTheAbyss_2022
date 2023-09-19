using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Data;
using Universal;
using GameMenu.Inventory.Storages;
using GameMenu.Artifacts;

namespace GameMenu.Shop
{
    public sealed class ShopLootGenerator : SingleSceneInstance
    {
        #region fields
        [SerializeField] ShopPanelInit shopPanelInit;
        public UnityAction OnShopLootGenerated;
        private float maxRndScale = 1.1f;
        private float minRndScale = 0.95f;
        #endregion fields

        #region methods
        protected override void Awake()
        {
            CheckInstances(GetType());
            CheckDifficulty();
        }
        private void CheckDifficulty()
        {
            if (GameDataInit.data.difficulty != Difficulty.Hard) return;
            maxRndScale = 1.4f;
            minRndScale = 1.3f;
        }
        private void OnEnable()
        {
            shopPanelInit.OnShopTimerChanged += CheckLootGeneration;
        }
        private void OnDisable()
        {
            shopPanelInit.OnShopTimerChanged -= CheckLootGeneration;
        }
        private void CheckLootGeneration(int remainingTime)
        {
            if (remainingTime > 0) return;
            GenerateShopLoot();
        }
        private int GetRandomLocation()
        {
            int choosedLocation = CustomMath.GetRandomChance() switch
            {
                float j when j < 7f => GameDataInit.data.reachedLocation - 1,
                float j when j <= 95f => GameDataInit.data.reachedLocation,
                float j when j <= 100f => GameDataInit.data.reachedLocation + 1,
                _ => GameDataInit.data.reachedLocation
            };
            return Mathf.Clamp(choosedLocation, 0, 5);
        }
        [ContextMenu("Generate loot")]
        public void GenerateShopLoot()
        {
            GameDataInit.data.shopData = new List<ShopData>();
            GenerateCards();
            GeneratePotions();
            GenerateArtifacts();
            GenerateChests();
            GenerateUpgrades();
            GenerateSizeItems();
            SavingUtils.SaveGameData();

            OnShopLootGenerated?.Invoke();
        }
        private void GenerateCards()
        {
            List<CardInfoSO> cardPrefabs = PrefabsData.instance.cardPrefabs.ToList();
            int maxCount = UnityEngine.Random.Range(1, 4);
            maxCount = GameDataInit.data.reachedLocation switch
            {
                int j when j <= 1 => Mathf.Min(maxCount, 1),
                2 => Mathf.Min(maxCount, 2),
                _ => 3
            };
            int mostRareIndex = 0;
            int mostRareTier = -1;
            int i = 0;
            while (i < maxCount)
            {
                List<CardInfoSO> cardInfos = cardPrefabs.Where(x => x.cardLocation == GetRandomLocation() && x.visibleInShop).ToList();
                if (cardInfos.Count == 0) continue;

                int index = Random.Range(0, cardInfos.Count);
                CardInfoSO cardInit = cardInfos[index];

                ShopData shopData = new ShopData();
                shopData.itemID = cardInit.id;
                shopData.lootType = LootType.Card;
                shopData.priceSilver = cardInit.silverPrice * (2 + cardInit.rareTier);
                shopData.priceGold = cardInit.goldPrice * (2 + cardInit.rareTier);

                if (cardInit.rareTier > mostRareTier)
                {
                    mostRareTier = cardInit.rareTier;
                    mostRareIndex = GameDataInit.data.shopData.Count;
                }
                RandomizeShopPrice(shopData);
                GameDataInit.data.shopData.Add(shopData);
                i++;
            }

            GameDataInit.data.shopData[mostRareIndex].discount = mostRareTier switch
            {
                0 => Random.Range(7, 20),
                1 => Random.Range(5, 14),
                2 => Random.Range(3, 10),
                3 => Random.Range(2, 7),
                _ => 0,
            };
        }
        private void GenerateChests()
        {
            List<ChestInfo> chestPrefabs = InventoryChestStorage.instance.chestPrefabs.ToList();
            int maxCount = UnityEngine.Random.Range(1, 5);
            maxCount = Mathf.Clamp(maxCount, 1, GameDataInit.data.reachedLocation + 1);
            int i = 0;
            while (i < maxCount)
            {
                List<ChestInfo> chestInfos = chestPrefabs.Where(chest => chest.chestLocation == GetRandomLocation() && chest.visibleInShop).ToList();
                if (chestInfos.Count == 0) continue;

                int index = Random.Range(0, chestInfos.Count);
                ChestInfo chestInfo = chestInfos[index];

                ShopData shopData = new ShopData();
                shopData.itemID = chestInfo.id;
                shopData.lootType = LootType.Chest;
                shopData.priceGold = chestInfo.goldPrice * 2;
                shopData.priceSilver = chestInfo.silverPrice * 2;
                RandomizeShopPrice(shopData);
                GameDataInit.data.shopData.Add(shopData);
                i++;
            }
        }
        private void GeneratePotions()
        {
            List<PotionInfoSO> potionPrefabs = PrefabsData.instance.potionPrefabs.ToList();
            int maxCount = Random.Range(1, 3);
            if (GameDataInit.data.reachedLocation < CustomMath.FindMin(potionPrefabs, x => x.potionLocation)) return;
            int i = 0;
            while (i < maxCount)
            {
                List<PotionInfoSO> potionInfos = potionPrefabs.Where(x => x.potionLocation == GetRandomLocation() && x.visibleInShop).ToList();
                if (potionInfos.Count == 0) continue;

                int index = Random.Range(0, potionInfos.Count);
                PotionInfoSO potionInfo = potionInfos[index];

                ShopData shopData = new ShopData();
                shopData.itemID = potionInfo.id;
                shopData.lootType = LootType.Potion;
                shopData.priceGold = potionInfo.goldPrice * 2;
                shopData.priceSilver = potionInfo.silverPrice * 2;
                RandomizeShopPrice(shopData);
                GameDataInit.data.shopData.Add(shopData);
                i++;
            }
        }
        private void GenerateArtifacts()
        {
            List<ArtifactInfoSO> artifactPrefabs = PrefabsData.instance.artifactPrefabs.ToList();
            int maxCount = 1;
            if (GameDataInit.data.reachedLocation < CustomMath.FindMin(artifactPrefabs, x => x.artifactLocation)) return;
            int i = 0;
            while (i < maxCount)
            {
                List<ArtifactInfoSO> artifactInfos = artifactPrefabs.Where(x => x.artifactLocation == GetRandomLocation() && x.visibleInShop).ToList();
                if (artifactInfos.Count == 0) continue;

                int index = Random.Range(0, artifactInfos.Count);
                ArtifactInfoSO artifactInfo = artifactInfos[index];
                if (GameDataInit.data.reachedLocation == 4 && GameDataInit.data.reachedPoint >= 5 && GameDataInit.data.artifactsData.FindIndex(x => x.effect == ArtifactEffect.InvisibleFlower) < 0)
                    GameDataInit.data.shopData.Add(GenerateArtifact(9));
                else
                    GameDataInit.data.shopData.Add(GenerateArtifact(artifactInfo.id));
                i++;
            }
        }
        private ShopData GenerateArtifact(int id)
        {
            ShopData shopData = new ShopData();
            ArtifactInfoSO artifactInfo = PrefabsData.instance.artifactPrefabs[id];
            shopData.itemID = artifactInfo.id;
            shopData.lootType = LootType.Artifact;
            shopData.priceGold = artifactInfo.goldPrice * 4;
            shopData.priceSilver = artifactInfo.silverPrice * 4;
            RandomizeShopPrice(shopData);
            shopData.discount = Random.Range(0, 40);
            return shopData;
        }
        private void GenerateUpgrades()
        {
            int maxUpgrades = 3;
            int addedUpgrades = 0;
            List<UpgradeData> possibleUpgrades = GameDataInit.data.upgradeData.Where(x => x.maxTier != x.tier).ToList();
            foreach (UpgradeData el in possibleUpgrades)
            {
                if (!CustomMath.GetRandomChance(100f / possibleUpgrades.Count * maxUpgrades)) continue;

                ShopData shopData = new ShopData();
                UpgradeDataTierInfo tierInfo = new UpgradeDataTierInfo();
                try { tierInfo = UpgradesData.instance.upgradesByTier.Find(x => x.id == el.id).info[el.tier]; }
                catch
                {
                    Debug.LogError($"{el.tier} TIER INFO FOR {el.id} ID IS NULL");
                    continue;
                }
                if (GameDataInit.data.reachedLocation < tierInfo.minLocation) continue;

                shopData.itemID = el.id;
                shopData.upgradeType = el.upgradeType;
                shopData.priceSilver = tierInfo.priceSilver;
                shopData.priceGold = tierInfo.priceGold;

                RandomizeShopPrice(shopData);
                GameDataInit.data.shopData.Add(shopData);
                addedUpgrades++;
                if (addedUpgrades == maxUpgrades) return;
            }
        }
        private void GenerateSizeItems()
        {
            for (int i = 0; i <= (int)SizeType.Potion; i++)
            {
                ShopData shopData = new ShopData();
                shopData.itemID = i;
                shopData.sizeType = (SizeType)i;
                if (shopData.sizeType == SizeType.None) continue;
                List<SizeDataShopInfo> sizeInfos = new List<SizeDataShopInfo>();
                SizeDataShopInfo sizeInfo = new SizeDataShopInfo();
                try
                {
                    SizeData currentSizeData = SizesData.instance.sizes.Find(x => x.sizeType == shopData.sizeType);
                    int sizeValue = GetSizeValue(shopData.sizeType);
                    if (sizeValue >= currentSizeData.maxCount)
                    {
                        print($"{shopData.sizeType} size is full : {sizeValue} / {currentSizeData.maxCount}");
                        continue;
                    }
                    sizeInfos = currentSizeData.info;
                    sizeInfo = FindLastSizeInfo(sizeInfos, sizeValue);
                    shopData.addToSize = sizeInfo.addToCount;
                    shopData.priceSilver = sizeInfo.priceSilver;
                    shopData.priceGold = sizeInfo.priceGold;
                }
                catch
                {
                    Debug.Log($"{shopData.sizeType} size info is null (or bad location)");
                    continue;
                }
            RandomizeShopPrice(shopData);
                GameDataInit.data.shopData.Add(shopData);
            }
        }
        private int GetSizeValue(SizeType sizeType) => (sizeType) switch
        {
            SizeType.Desk => GameDataInit.data.maxDeskSize,
            SizeType.Hand => GameDataInit.data.maxHandSize,
            SizeType.Artifact => GameDataInit.data.maxArtifactSize,
            SizeType.Inventory => GameDataInit.data.maxInventorySize,
            SizeType.House => GameDataInit.data.maxHouseSize,
            SizeType.Potion => GameDataInit.data.maxPotionSize,
            _ => throw new System.NotImplementedException()
        };
        private void RandomizeShopPrice(ShopData shopData)
        {
            shopData.priceSilver = (int)Random.Range(shopData.priceSilver * minRndScale, shopData.priceSilver * maxRndScale);
            shopData.priceGold = (int)Random.Range(shopData.priceGold * minRndScale, shopData.priceGold * maxRndScale);
        }
        private SizeDataShopInfo FindLastSizeInfo(List<SizeDataShopInfo> list, int value) =>
                list.FindLast(el => el.minCount <= value && el.location <= GameDataInit.data.reachedLocation);
        #endregion methods
    }
}