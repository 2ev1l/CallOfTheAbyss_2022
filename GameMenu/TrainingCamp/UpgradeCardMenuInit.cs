using Data;
using GameMenu.Inventory.Cards;
using Universal;

namespace GameMenu.TrainingCamp
{
    public class UpgradeCardMenuInit : CardMenuInit
    {
        private int upgradedID => PrefabsData.instance.cardPrefabs[id].upgradedCardID;
        public void UpgradeCard()
        {
            CardInfoSO newCardInfo = PrefabsData.instance.cardPrefabs[upgradedID];
            CardData newCardData = GameDataInit.GetCardDataFromPrefab(newCardInfo, listPosition);
            GameDataInit.data.cardsData[listPosition] = newCardData;
            GameDataInit.AddSilver(-cardInfo.upgradeSilverPrice, false);
            GameDataInit.AddGold(-cardInfo.upgradeGoldPrice, false);
            int upgradeCopyPrice = cardInfo.upgradeDuplicatePrice;
            for (int i = 0; i < upgradeCopyPrice; i++)
            {
                int index = GameDataInit.data.cardsCopyData.FindIndex(x => x == id);
                GameDataInit.data.cardsCopyData.RemoveAt(index);
            }
            TrainingCampInit.instance.UpdateTrainingCampList();
        }
    }
}
