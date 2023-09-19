using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace Data
{
    public class SizesData : SingleSceneInstance
    {
        #region fields & properties
        public static SizesData instance { get; private set; }
        [field: SerializeField] public List<SizeData> sizes { get; private set; } = new List<SizeData>();
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        public void CheckAchievement(SizeType sizeType)
        {
            int sizeValue = GetSizeValueByType(sizeType);
            SizeData sizeData = sizes.Find(x => x.sizeType == sizeType);
            int maxSizeValue = sizeData.maxCount;
            if (sizeValue < maxSizeValue) return;
            Achievements.SetAchievement(sizeData.achievementNote);
        }
        public static int GetSizeValueByType(SizeType sizeType) => (sizeType) switch
        {
            SizeType.Desk => GameDataInit.data.maxDeskSize,
            SizeType.Artifact => GameDataInit.data.maxArtifactSize,
            SizeType.Inventory => GameDataInit.data.maxInventorySize,
            SizeType.Hand => GameDataInit.data.maxHandSize,
            SizeType.House => GameDataInit.data.maxHouseSize,
            SizeType.Potion => GameDataInit.data.maxPotionSize,
            _ => throw new System.NotImplementedException()
        };
        #endregion methods
    }
    public enum SizeType { Desk, Hand, Artifact, Inventory, House, None, Potion }

    [System.Serializable]
    public class SizeData
    {
        public SizeType sizeType = SizeType.None;
        public int maxCount = 0;
        [field: SerializeField] public string achievementNote { get; private set; }
        public List<SizeDataShopInfo> info = new List<SizeDataShopInfo>();
    }

    [System.Serializable]
    public class SizeDataShopInfo
    {
        public int minCount;
        public int addToCount;
        public int priceSilver;
        public int priceGold;
        public int location;
    }
}