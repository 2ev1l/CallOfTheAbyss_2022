using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Data
{
    public class UpgradesData : SingleSceneInstance
    {
        #region fields & properties
        public static UpgradesData instance { get; private set; }

        [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();
        [field: SerializeField] public List<UpgradeDataInShop> upgradesByTier { get; private set; } = new List<UpgradeDataInShop>();
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            Init();
            CheckInstances(GetType());
        }
        public void Init()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                UpgradeData upgrade = upgrades[i];
                if (GameDataInit.data.upgradeData.Where(el => el.id == upgrade.id).Count() == 0)
                {
                    print($"Added new upgrade: {upgrade.id} id");
                    GameDataInit.data.upgradeData.Add(upgrade);
                }
            }
            for (int i = 0; i < upgrades.Count; i++)
            {
                int upgradeIndex = GameDataInit.data.upgradeData.FindIndex(el => el.id == upgrades[i].id);
                if (GameDataInit.data.upgradeData[upgradeIndex].maxTier != upgrades[i].maxTier)
                {
                    print($"tier of {upgrades[i].id} id is changed");
                    GameDataInit.data.upgradeData[upgradeIndex].maxTier = upgrades[i].maxTier;
                }
                if (GameDataInit.data.upgradeData[upgradeIndex].upgradeType != upgrades[i].upgradeType)
                {
                    print($"upgrade type of {upgrades[i].id} id is changed");
                    GameDataInit.data.upgradeData[upgradeIndex].upgradeType = upgrades[i].upgradeType;
                }
            }
        }

        public void CheckAchievement(int upgradeId)
        {
            UpgradeData upgrade = upgrades.Find(x => x.id == upgradeId);
            if (upgrade.maxTier != GameDataInit.data.upgradeData.Find(x => x.id == upgradeId).tier) return;
            Achievements.SetAchievement(upgrade.note);
        }
        [ContextMenu("Force update")]
        private void InitUpdate()
        {
            GameDataInit.data.upgradeData.Clear();
            Init();
        }
        #endregion methods
    }
    public enum UpgradeType { None, Other }

    [System.Serializable]
    public class UpgradeData
    {
        //unique id for each
        public int id = -1;
        public int tier = 0;
        public int maxTier = 0;
        public UpgradeType upgradeType = UpgradeType.None;
        public string note;
    }

    [System.Serializable]
    public class UpgradeDataInShop
    {
        //unique id for each
        public int id = -1;
        public int itemTextID = 0;
        public int infoTextID = 0;
        public List<UpgradeDataTierInfo> info = new List<UpgradeDataTierInfo>();
    }

    [System.Serializable]
    public class UpgradeDataTierInfo
    {
        public int tier = 0;
        public int minLocation = 0;
        public int priceSilver = 0;
        public int priceGold = 0;
    }
}