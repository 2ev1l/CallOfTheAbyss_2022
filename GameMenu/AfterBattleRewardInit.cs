using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace GameMenu
{
    [RequireComponent(typeof(GameMenuButtons))]
    public sealed class AfterBattleRewardInit : SingleSceneInstance
    {
        private List<string> rewardNames => new List<string> { "IconSilver_1", "IconGold_1", "IconLootBox_1", "IconPotions_1", "IconArtifacts_1", "IconCards_1" };
        private List<int> rewardCount => new List<int>() { GameDataInit.data.earnedSilver, GameDataInit.data.earnedGold, GameDataInit.data.earnedChests.Count, GameDataInit.data.earnedPotions.Count, GameDataInit.data.earnedArtifacts.Count, GameDataInit.data.earnedCards.Count };
        [SerializeField] private GameMenuSoundUpdater soundUpdater;

        protected override void Awake()
        {
            CheckInstances(GetType());
        }
        private void Start()
        {
            if (!GameDataInit.earnReward) return;
            soundUpdater.PlayCoinSound((GameDataInit.data.earnedGold + GameDataInit.data.earnedSilver) > 0);
            ShowRewardLoot();
        }
        private void ShowRewardLoot()
        {
            Vector3 toPosition = Vector3.right * (-0.24f) + Vector3.up * 0.85f;
            float offset = 0.35f;
            GameDataInit.earnReward = false;
            float lerp = 0f;
            for (int i = 0; i < rewardNames.Count; i++)
            {
                toPosition = CustomAnimation.instance.UpdateIntCounterSmooth(rewardNames[i], rewardCount[i], lerp, false, offset, toPosition);
                lerp += 0.05f;
            }
            if (rewardCount.Sum() > 0)
                GetComponent<GameMenuButtons>().GameMenuPressedOpen("Loot");
            GameDataInit.ResetAdventureProgress();
        }
    }
}
