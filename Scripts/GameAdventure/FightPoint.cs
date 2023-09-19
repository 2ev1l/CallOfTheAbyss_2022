using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Universal;

namespace GameAdventure
{
    public class FightPoint : MonoBehaviour
    {
        #region fields
        public FightParameters fightParameters;
        public static FightParameters fightParametersCopy;
        [SerializeField] private PointUIUpdater uiUpdater;

        private int fightId;
        private List<int> mobsIdMaxCopy;
        private List<int> mobsIdGenerated;
        public IEnumerable<int> MobsIdGenerated => mobsIdGenerated;
        private List<int> mobsIdRespawned;
        private bool isMobsGenerated;
        [HideInInspector] public bool isRandomEvent;
        #endregion fields

        #region methods
        private void Start()
        {
            fightId = System.Convert.ToInt32(gameObject.name[gameObject.name.Length - 1].ToString());
        }
        public void UpdatePointStatus(PointStatus status, int arg = 0) => uiUpdater.UpdatePointStatus(status, arg);
        public void EnableTrigger(bool enabled) => uiUpdater.EnableTrigger(enabled);
        public void ChangeSprite(Sprite newSprite) => uiUpdater.ChangeSprite(newSprite);
        public void ChangeSpriteColor(Color newColor) => uiUpdater.ChangeSpriteColor(newColor);
        public void ChangeLayoutSpriteColor(Color newColor) => uiUpdater.ChangeLayoutSpriteColor(newColor);
        public void GenerateMobs()
        {
            switch (fightParameters.status)
            {
                case PointStatus.Normal:
                    mobsIdGenerated = fightParameters.mobsIdMax.ToList();
                    int c = 0;
                    while (c < mobsIdGenerated.Count)
                    {
                        int toRemove = -1;
                        if (CustomMath.GetRandomChance(fightParameters.mobsRemoveChance))
                        {
                            try { toRemove = fightParameters.mobsIdException.FindIndex(value => value == mobsIdGenerated[c]); }
                            catch { Debug.LogError($"index was out of range at {gameObject.name}"); }
                        }
                        if (toRemove >= 0)
                        {
                            mobsIdGenerated.RemoveAt(c);
                            fightParameters.mobsIdException.RemoveAt(toRemove);
                            continue;
                        }
                        c++;
                    }
                    break;
                case PointStatus.Defeated:
                    mobsIdGenerated = new List<int>();
                    mobsIdRespawned = new List<int>();
                    break;
                case PointStatus.Respawned:
                    mobsIdMaxCopy = fightParameters.mobsIdMax.ToList();
                    mobsIdRespawned = new List<int>();
                    for (int i = 0; i < fightParameters.respawnedEnemiesCount; i++)
                    {
                        int rnd = UnityEngine.Random.Range(0, mobsIdMaxCopy.Count);
                        mobsIdRespawned.Add(fightParameters.mobsIdMax[rnd]);
                        mobsIdMaxCopy.RemoveAt(rnd);
                    }
                    break;
            }
            isMobsGenerated = true;
        }
        public bool CanGenerateMobs() => !isMobsGenerated;
        public void LoadScenario()
        {
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint = fightId;
            GameAdventurePointsInit pointsInit = GameAdventurePointsInit.instance;
            pointsInit.UpdatePoints();
            switch (fightParameters.status)
            {
                case PointStatus.Defeated:
                    GameDataInit.CheckReachedPoints();
                    if (GameDataInit.data.reachedLocation >= GameAdventurePointsInit.mobsRespawnLocation && CustomMath.GetRandomChance(GameAdventurePointsInit.mobsRespawnChance))
                    {
                        for (int i = 0; i <= GameDataInit.data.currentLocation; i++)
                            pointsInit.SetRandomPoints(i);

                        pointsInit.UpdatePoints();
                    }

                    if (fightParameters.isLastLocation)
                    {
                        if (GameAdventurePointsInit.isInvisibleFlowerLocation)
                        {
                            if (GameAdventurePointsInit.isInvisibleFlowerApplied)
                                GameAdventureBGAnimation.LoadLocation(true);
                            else return;
                        }
                        else
                            GameAdventureBGAnimation.LoadLocation(true);
                    }
                    return;
                case PointStatus.Respawned:
                    if (!isRandomEvent) break;
                    SceneLoader.instance.LoadSceneFade("GameEvent", SceneLoader.screenFadeTime + 0.3f);
                    return;
            }
            LoadFight();
        }
        public void LoadFight()
        {
            fightParametersCopy.loadCutScene = false;
            fightParametersCopy.specialRewardsOnce = new List<BattleReward>();
            fightParametersCopy.specialRewardsAlways = new List<BattleReward>();
            fightParametersCopy.maxHandSize = fightParameters.maxHandSize;
            fightParametersCopy.fightBG = fightParameters.fightBG;
            fightParametersCopy.status = fightParameters.status;
            float locationDivision = Mathf.Clamp(Mathf.Pow(GameDataInit.data.reachedLocation - GameDataInit.data.currentLocation + 1, 1.3f), 1, 1000);

            switch (fightParameters.status)
            {
                case PointStatus.Normal:
                    fightParametersCopy.mobsIdFinal = mobsIdGenerated;
                    fightParametersCopy.isLastLocation = fightParameters.isLastLocation;
                    fightParametersCopy.rewardDivision = fightParameters.rewardDivision * locationDivision;
                    if (GameDataInit.data.difficulty == Difficulty.Hard)
                        fightParametersCopy.rewardDivision /= 1.15f;

                    if (GameDataInit.data.reachedLocation - GameDataInit.data.currentLocation == 0)
                    {
                        float specialRewardsChance = fightParameters.specialRewardsChance;
                        if (GameDataInit.data.difficulty == Difficulty.Hard)
                            specialRewardsChance *= 1.2f;

                        for (int i = 0; i < fightParameters.specialRewardsAlways.Count; i++)
                        {
                            if (CustomMath.GetRandomChance(specialRewardsChance))
                            {
                                print("always reward added: " + fightParameters.specialRewardsAlways[i].id + $" {fightParameters.specialRewardsAlways[i].lootType}");
                                fightParametersCopy.specialRewardsAlways.Add(fightParameters.specialRewardsAlways[i]);
                            }
                        }
                    }
                    if (GameDataInit.data.tempData[GameDataInit.data.currentLocation].reachedPoint == GameDataInit.data.reachedPoint && GameDataInit.data.reachedLocation == GameDataInit.data.currentLocation)
                    {
                        print("once rewards enabled");
                        fightParametersCopy.cutSceneId = fightParameters.cutSceneId;
                        fightParametersCopy.loadCutScene = fightParameters.loadCutScene;
                        fightParametersCopy.specialRewardsOnce = fightParameters.specialRewardsOnce;
                    }
                    break;

                case PointStatus.Respawned:
                    fightParametersCopy.mobsIdFinal = mobsIdRespawned;
                    fightParametersCopy.rewardDivision = fightParameters.rewardDivision * 3 * locationDivision;
                    break;

                default: throw new System.NotImplementedException();
            }
            GameAdventureBGAnimation.StartClosingAnimation(true);

            SceneLoader.instance.LoadSceneFade("GameFight", SceneLoader.screenFadeTime + 0.3f);
        }

        #endregion methods
    }
    [System.Serializable]
    public class FightParameters
    {
        [HideInInspector] public List<int> mobsIdFinal;
        public PointStatus status = PointStatus.Normal;

        [Header("Fight Options")]
        [Range(1, 5)] public int maxHandSize = 3;
        public int respawnedEnemiesCount;
        public Sprite fightBG;
        public bool isLastLocation;

        [Header("Mobs Options")]
        public int[] mobsIdMax;
        [Range(0, 100)] public int mobsRemoveChance = 40;
        public List<int> mobsIdException;

        [Header("Reward Options")]
        [Min(1f)] public float rewardDivision = 3f;
        public List<BattleReward> specialRewardsOnce;
        [Range(0, 100)] public float specialRewardsChance;
        public List<BattleReward> specialRewardsAlways;

        [Header("Cut Scene Options")]
        public bool loadCutScene;
        public int cutSceneId;
    }

    [System.Serializable]
    public class BattleReward
    {
        public int id = -1;
        public LootType lootType = LootType.None;
    }

}