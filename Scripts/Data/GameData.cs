using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Data
{
    [System.Serializable]
    public class GameData
    {
        #region fields & properties
        #region tutorial
        public bool isTutorialCompleted = false;
        public int tutorialProgress = 0;
        #endregion tutorial

        #region cut scene
        public bool isCutSceneInAdventure = false;
        public int currentScenario = 0;
        #endregion cut scene

        #region main data
        public string sceneName = "Menu";

        public List<CardData> cardsData = new List<CardData>() {
        new CardData()
    {
        listPosition = 0,
        houseStartTimeHP = 0,
        houseStartTimeDMG = 0,
        houseStartTimeDEF = 0,
        deskPosition = -1,
        hp = 4,
        damage = 3,
        defense = 0,
        id = 0,
        maxDefense = 0,
        maxDamage = 3,
        maxHP = 5,
        trashStartTime = 0,
        onHeal = false,
        onDesk = false
    }
    };
        [SerializeField][HideInInspector] private List<CardData> CardsOnTrash = new List<CardData>();
        public List<CardData> cardsOnTrash
        {
            get => CardsOnTrash;
            set
            {
                CardsOnTrash = value;
                GameDataInit.instance.OnTrashCardsChanged?.Invoke();
            }
        }

        public List<int> chestsData = new List<int>();
        public List<PotionData> potionsData = new List<PotionData>();
        public List<ArtifactData> artifactsData = new List<ArtifactData>();

        public List<UpgradeData> upgradeData = new List<UpgradeData>();
        public List<ShopData> shopData = new List<ShopData>();

        public List<int> interfacesOpened = new List<int>();
        public List<int> cardsCopyData = new List<int>();

        [SerializeField][HideInInspector] private int CoinsSilver = 5;
        public int coinsSilver
        {
            get => CoinsSilver;
            set
            {
                CoinsSilver = value;
                GameDataInit.instance.OnCoinsChanged?.Invoke();
            }
        }
        [SerializeField][HideInInspector] private int CoinsGold = 0;
        public int coinsGold
        {
            get => CoinsGold;
            set
            {
                CoinsGold = value;
                GameDataInit.instance.OnCoinsChanged?.Invoke();
            }
        }
        public bool isGameCompleted = false;
        public Difficulty difficulty = Difficulty.Normal;
        #endregion main data

        #region adventure data
        public int reachedLocation = 0;
        public int reachedPoint = 0;
        #endregion adventure data
        
        #region upgrades
        public int locationOffset = 0;
        public float percentRewardOnDeath = 0f;
        public float percentRewardOnWin = 1f;
        public bool canSeeWinChance = false;
        #endregion upgrades

        #region time
        public int playTime = 0;
        public int shopTime = 0;
        public int trashTime = 0;
        public int houseTime = 0;
        public int lastShopUpdated = -1;
        #endregion time

        #region sizes
        public int maxDeskSize = 3;
        public int maxHandSize = 1;
        public int maxPotionSize = 1;
        public int maxArtifactSize = 1;
        public int maxInventorySize = 5;
        public int maxHouseSize = 1;
        #endregion sizes

        #region adventure temp
        public int currentLocation = 0;
        public int currentLocationMax = 0;
        public List<TempData> tempData = new List<TempData>();
        public int earnedSilver = 0;
        public int earnedGold = 0;
        public List<int> earnedChests = new List<int>();

        public List<PotionData> earnedPotions = new List<PotionData>();
        public List<ArtifactData> earnedArtifacts = new List<ArtifactData>();
        public List<CardData> earnedCards = new List<CardData>();
        #endregion adventure temp

        #endregion fields & properties
    }

    [System.Serializable]
    public class CardData: ItemData
    {
        public int hp = -1;
        public int defense = -1;
        public int damage = -1;

        public int maxHP = -1;
        public int maxDamage = -1;
        public int maxDefense = -1;
        public int houseStartTimeHP = -1;
        public int houseStartTimeDMG = -1;
        public int houseStartTimeDEF = -1;
        public int trashStartTime = -1;
        public bool onHeal = false;
    }
    [System.Serializable]
    public class ShopData
    {
        public int itemID = -1;
        public int priceSilver = 0;
        public int priceGold = 0;
        public int discount = 0;
        public bool owned = false;
        public LootType lootType = LootType.None;
        public UpgradeType upgradeType = UpgradeType.None;
        public SizeType sizeType = SizeType.None;
        public int addToSize = 0;
    }
    [System.Serializable]
    public class ItemData
    {
        public int id = -1;
        public int listPosition = -1;

        public int deskPosition = -1;
        public bool onDesk = false;
    }
    [System.Serializable]
    public class TempData
    {
        public int currentPoint = 0;
        public int reachedPoint = 0;
        public List<int> defeatedPoints = new List<int>();
        public List<RespawnedPointData> respawnedPoints = new List<RespawnedPointData>();
    }
    [System.Serializable]
    public class RespawnedPointData
    {
        public int id = -1;
        public bool isRandomEvent = false;
        public RespawnedPointData(int id, bool isRandomEvent)
        {
            this.id = id;
            this.isRandomEvent = isRandomEvent;
        }
    }
    [System.Serializable]
    public class ArtifactData : ItemData
    {
        public ArtifactEffect effect;
    }
    [System.Serializable]
    public class PotionData : ItemData
    {
        public PotionEffect effect = PotionEffect.None;
        public int value = 0;
    }
    public enum LootType { Chest, Potion, Artifact, Card, Gold, Silver, TrashCard, None }
    public enum Difficulty { Normal, Hard }
}