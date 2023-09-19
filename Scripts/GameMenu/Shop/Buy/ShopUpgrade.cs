using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;
using GameMenu.House;

namespace GameMenu.Shop
{
    public sealed class ShopUpgrade : ShopObject
    {
        #region fields
        [SerializeField] private Text itemText;
        [SerializeField] private Text infoText;
        #endregion fields

        #region methods
        public override void Init()
        {
            shopData ??= new ShopData();
            base.Init();
            UpgradeDataInShop upgradeInfo = UpgradesData.instance.upgradesByTier.Find(x => x.id == shopData.itemID);
            if (upgradeInfo == null)
            {
                Debug.LogError($"Upgrade info in {shopData.upgradeType} type incorrect with {shopData.itemID} id");
                return;
            }
            LanguageLoad ll = itemText.GetComponent<LanguageLoad>();
            ll.ChangeID(upgradeInfo.itemTextID);
            int currentTier = GameDataInit.data.upgradeData[shopData.itemID].tier;
            itemText.text += $": {currentTier} => {currentTier + 1}";

            ll = infoText.GetComponent<LanguageLoad>();
            ll.ChangeID(upgradeInfo.infoTextID);
        }
        public override bool CanInit()
        {
            if (shopData == null || shopData.owned || shopData.itemID < 0 || shopData.upgradeType == UpgradeType.None)
            {
                ActivateObject(false);
                return false;
            }
            return true;
        }
        protected override void ActivateObject(bool f)
        {
            base.ActivateObject(f);
            itemText.enabled = f;
            infoText.enabled = f;
        }
        public override void BuyItem()
        {
            if (!canBuy) return;
            base.BuyItem();
            UpgradeData upgrade = GameDataInit.data.upgradeData[shopData.itemID];
            upgrade.tier++;
            switch (shopData.itemID)
            {
                case 0:
                    HouseTier.instance.UpdateValues();
                    break;
                case 1:
                    ElevatorInit.instance.CheckUpgrade();
                    break;
                case 2:
                    GameDataInit.data.percentRewardOnDeath = 0f + upgrade.tier / 10f;
                    break;
                case 3:
                    GameDataInit.data.canSeeWinChance = true;
                    break;
                case 4:
                    GameDataInit.data.percentRewardOnWin = 1f + upgrade.tier / 20f;
                    break;
                default: Debug.LogError("Upgrade type isn't initialized"); break;
            }
            UpgradesData.instance.CheckAchievement(shopData.itemID);
        }
        #endregion methods
    }
}