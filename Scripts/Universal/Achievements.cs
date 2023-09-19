using Steamworks;
using UnityEngine;

namespace Universal
{
    public class Achievements : MonoBehaviour
    {
        #region methods
        public void Start()
        {
            CheckAchievements();
        }
        private void CheckAchievements()
        {
            if (GameDataInit.data.reachedLocation >= 6)
            {
                if (GameDataInit.data.difficulty == Data.Difficulty.Normal)
                    SetAchievement("A_FINAL_NORMAL");
                else if (GameDataInit.data.difficulty == Data.Difficulty.Hard)
                    SetAchievement("A_FINAL_HARD");
            }
            if (GameDataInit.data.artifactsData.Find(x => x.effect == Data.ArtifactEffect.InvisibleFlower) != null)
                SetAchievement("A_ARTIFACT_INVISIBLE_FLOWER");
        }
        public static void SetAchievement(string name)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement(name);
            SteamUserStats.StoreStats();
        }
        #endregion methods
    }
}