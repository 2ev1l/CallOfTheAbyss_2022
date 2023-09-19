using Universal;
using GameMenu.Inventory.Cards;
using UnityEngine;

namespace GameMenu.Blacksmith
{
    public sealed partial class BlacksmithCardMenuInit : CardMenuInit
    {
        #region fields
        private int pricePerHPSilver = -1;
        private int pricePerHPGold = -1;
        private int pricePerDMGSilver = -1;
        private int pricePerDMGGold = -1;
        private int pricePerDEFSilver = -1;
        private int pricePerDEFGold = -1;
        #endregion fields

        #region methods
        public void SetPricePerHP(int silverPrice, int goldPrice)
        {
            pricePerHPSilver = silverPrice;
            pricePerHPGold = goldPrice;
        }
        public void SetPricePerDMG(int silverPrice, int goldPrice)
        {
            pricePerDMGSilver = silverPrice;
            pricePerDMGGold = goldPrice;
        }
        public void SetPricePerDEF(int silverPrice, int goldPrice)
        {
            pricePerDEFSilver = silverPrice;
            pricePerDEFGold = goldPrice;
        }
        public void UpgradeHealth()
        {
            if (!CanBuy(pricePerHPSilver, pricePerHPGold)) return;
            GameDataInit.AddSilver(-pricePerHPSilver, false);
            GameDataInit.AddGold(-pricePerHPGold, false);
            GameDataInit.data.cardsData[listPosition].maxHP++;
            BlacksmithInit.instance.itemList.UpdateCurrentPage();
        }
        public void UpgradeDamage()
        {
            if (!CanBuy(pricePerDMGSilver, pricePerDMGGold)) return;
            GameDataInit.AddSilver(-pricePerDMGSilver, false);
            GameDataInit.AddGold(-pricePerDMGGold, false);
            GameDataInit.data.cardsData[listPosition].maxDamage++;
            BlacksmithInit.instance.itemList.UpdateCurrentPage();
        }
        public void UpgradeDefense()
        {
            if (!CanBuy(pricePerDEFSilver, pricePerDEFGold)) return;
            GameDataInit.AddSilver(-pricePerDEFSilver, false);
            GameDataInit.AddGold(-pricePerDEFGold, false);
            GameDataInit.data.cardsData[listPosition].maxDefense++;
            BlacksmithInit.instance.itemList.UpdateCurrentPage();
        }
        private bool CanBuy(int silverPrice, int goldPrice)
        {
            return (GameDataInit.data.coinsSilver >= silverPrice && GameDataInit.data.coinsGold >= goldPrice && silverPrice > -1 && goldPrice > -1);
        }
        #endregion methods
    }
}