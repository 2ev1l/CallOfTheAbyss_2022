using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using GameAdventure;
using GameFight.Card;
using Data;
using Universal;

namespace GameFight
{
    public class StageEndInit : SingleSceneInstance
    {
        #region fields & properties
        public static StageEndInit instance { get; private set; }
        public UnityAction<bool> OnStageEnded;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void OnStageEnd(bool isCompleted)
        {
            OnStageEnded?.Invoke(isCompleted);
            SaveCardsStatsToDesk();
            SaveCardsStatsToInventory();
            SavingUtils.SaveGameData();
        }
        private void SaveCardsStatsToDesk()
        {
            var allyCards = GameObject.FindGameObjectsWithTag("Card").Where(obj => obj.TryGetComponent(out CardFightInit cfi) && !cfi.isEnemy);
            foreach (GameObject el in allyCards)
            {
                CardFightInit elCardInit = el.GetComponent<CardFightInit>();
                CardData deskCard = GameDataInit.deskCard(elCardInit.absolutePosition);
                deskCard.hp = Mathf.Min(elCardInit.hp, elCardInit.maxHP);
                deskCard.damage = Mathf.Min(elCardInit.damage, elCardInit.maxDamage);
                deskCard.defense = Mathf.Min(elCardInit.defense, elCardInit.maxDefense);
                GameDataInit.data.cardsData[deskCard.listPosition] = deskCard;
            }
        }
        private void SaveCardsStatsToInventory()
        {
            int i = 0;
            while (true)
            {
                if (i >= GameDataInit.deskCards.Count)
                    break;
                CardData deskCard = GameDataInit.deskCard(i);
                int deskCardListPosition = deskCard.listPosition;
                GameDataInit.data.cardsData[deskCardListPosition].hp = deskCard.hp;
                GameDataInit.data.cardsData[deskCardListPosition].damage = deskCard.damage;
                GameDataInit.data.cardsData[deskCardListPosition].defense = deskCard.defense;
                if (deskCard.hp == 0)
                {
                    GameDataInit.RemoveCardFromDesk(deskCardListPosition);
                    continue;
                }
                i++;
            }
        }
        public void OnStageFailed()
        {
            OnStageEnd(false);
            GameDataInit.ResetEarnedLoot(FightStorage.totalSilverGain, FightStorage.totalGoldGain);
        }
        public void OnStageCompleted()
        {
            #region values
            OnStageEnd(true);
            FightParameters fightParameters = FightPoint.fightParametersCopy;
            int silverAdded = Mathf.RoundToInt(FightStorage.totalSilverGain / fightParameters.rewardDivision);
            int goldAdded = Mathf.RoundToInt(FightStorage.totalGoldGain / fightParameters.rewardDivision / 2.2f);
            GameDataInit.AddSilver(silverAdded, true);
            GameDataInit.AddGold(goldAdded, true);
            TempData location = GameDataInit.data.tempData[GameDataInit.data.currentLocation];
            int currentPoint = location.currentPoint;
            if (!location.defeatedPoints.Contains(currentPoint))
                location.defeatedPoints.Add(currentPoint);
            if (GameDataInit.data.currentLocation == 6)
            {
                if (location.defeatedPoints.Contains(currentPoint))
                    location.defeatedPoints.Remove(currentPoint);
                if (GameDataInit.data.reachedPoint <= 1)
                    GameDataInit.data.reachedPoint = 2;
                GameDataInit.data.isGameCompleted = true;
            }

            int respawnedIndex = location.respawnedPoints.FindIndex(x => x.id == currentPoint);
            if (fightParameters.status == PointStatus.Respawned && respawnedIndex > -1)
                location.respawnedPoints.RemoveAt(respawnedIndex);

            GameDataInit.CheckReachedPoints();
            #endregion values

            #region rewards
            int rewardsGain = 0;
            for (int i = 0; i < fightParameters.specialRewardsAlways.Count; i++)
            {
                rewardsGain++;
                GameDataInit.AddAnyLoot(fightParameters.specialRewardsAlways[i].id, fightParameters.specialRewardsAlways[i].lootType, true);
            };
            for (int i = 0; i < fightParameters.specialRewardsOnce.Count; i++)
            {
                rewardsGain++;
                GameDataInit.AddAnyLoot(fightParameters.specialRewardsOnce[i].id, fightParameters.specialRewardsOnce[i].lootType, true);
            };
            SavingUtils.SaveGameData();

            Vector3 toPosition = -Vector3.up * 287;
            float offset = 150f;
            toPosition = CustomAnimation.instance.UpdateIntCounterSmooth("IconSilver", silverAdded, 0f, false, offset, toPosition);
            toPosition = CustomAnimation.instance.UpdateIntCounterSmooth("IconGold", goldAdded, 0.05f, false, offset, toPosition);
            CustomAnimation.instance.UpdateIntCounterSmooth("IconBoxes", rewardsGain, 0.1f, false, offset, toPosition);
            #endregion rewards
        }
        #endregion methods
    }
}