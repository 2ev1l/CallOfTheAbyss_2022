using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameAdventure
{
    public class GameAdventureInit : HelpUpdate
    {
        #region fields & properties
        [SerializeField] private int gotolayerid;
        public static GameAdventureInit instance { get; private set; }
        [SerializeField] private GameObject[] onStartEnable;
        [SerializeField] private GameObject[] tutorialDisabled;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject[] stages;
        [SerializeField] private Transform[] gradients;
        [SerializeField] private GameObject iconDebuff;
        [SerializeField] private GameObject iconEscape;
        public static GameObject currentStage;
        public static Text locationText;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            foreach (var el in onStartEnable)
                el.SetActive(true);

            if (GameDataInit.data.currentLocation >= GameDataInit.data.tempData.Count)
                GameDataInit.data.tempData.Add(new TempData());

            if (!GameDataInit.data.isTutorialCompleted)
            {
                foreach (var el in tutorialDisabled)
                    el.SetActive(false);

                tutorialPanel.SetActive(true);
                currentStage = tutorialPanel;
                GameAdventurePointsInit.instance.OnPointsInit?.Invoke();
            }
            else
            {
                UpdateBG();
                GameAdventurePointsInit.instance.OnPointsInit?.Invoke();
                UpdateCoins();
            }
            foreach (var el in gradients)
                el.SetParent(currentStage.transform);

            InitDebuffIcon();
            InitEscapeIcon();
            InitText();
            SavingUtils.SaveGameData();

        }
        protected override void Awake()
        {
            base.Awake();
            instance = this;
            FightPoint.fightParametersCopy = new FightParameters();
        }
        private void InitDebuffIcon()
        {
            iconDebuff.SetActive(GameDataInit.data.currentLocation != 0);
            iconDebuff.GetComponent<ShowHelp>().id = GameDataInit.data.currentLocation switch
            {
                0 => 0,
                1 => 62,
                2 => 72,
                3 => 84,
                4 => 94,
                5 => 103,
                _ => 103
            };
        }
        private void InitEscapeIcon()
        {
            iconEscape.SetActive(GameDataInit.IsArtifactEffectApplied(ArtifactEffect.AncientTeleport));
        }
        private void InitText()
        {
            locationText = currentStage.transform.Find("LocationName").GetComponent<Text>();
            StartCoroutine(CustomAnimation.SetTextAlpha(locationText, 0, 1, 2));
        }
        public void UpdateBG()
        {
            currentStage = stages[GameDataInit.data.currentLocation];
            foreach (GameObject el in stages)
                el.SetActive(el == currentStage);
        }
        private void UpdateCoins()
        {
            Vector3 toPosition = Vector3.right * 1203 + Vector3.up * 465;
            float offset = 100f;
            toPosition = CustomAnimation.instance.UpdateIntCounterSmooth("IconSilverX", GameDataInit.data.earnedSilver, 0f, true, offset, toPosition);
            toPosition = CustomAnimation.instance.UpdateIntCounterSmooth("IconGoldX", GameDataInit.data.earnedGold, 0.05f, true, offset, toPosition);
            CustomAnimation.instance.UpdateIntCounterSmooth("IconBoxesX", GameDataInit.data.earnedChests.Count + GameDataInit.data.earnedArtifacts.Count + GameDataInit.data.earnedPotions.Count + GameDataInit.data.earnedCards.Count, 0.1f, true, offset, toPosition);
        }
        [ContextMenu("fps test")]
        private void fpstest()
        {
            Application.targetFrameRate = -1;
            print(Application.targetFrameRate);
        }
        [ContextMenu("test2")]
        private void sdal()
        {
            GameDataInit.data.percentRewardOnDeath = 0.5f;
        }
        [ContextMenu("test1")]
        private void addAllTemp()
        {
            GameDataInit.AddAnyLoot(1, LootType.Card, true);
            GameDataInit.AddAnyLoot(5, LootType.Card, true);
            GameDataInit.AddAnyLoot(1, LootType.Chest, true);
            GameDataInit.AddAnyLoot(4, LootType.Chest, true);
            GameDataInit.AddAnyLoot(1, LootType.Potion, true);
            GameDataInit.AddAnyLoot(3, LootType.Potion, true);
            GameDataInit.AddAnyLoot(1, LootType.Artifact, true);
            GameDataInit.AddAnyLoot(2, LootType.Artifact, true);
            GameDataInit.AddAnyLoot(10, LootType.Gold, true);
            GameDataInit.AddAnyLoot(100, LootType.Silver, true);
        }
        [ContextMenu("Go to layer")]
        private void GoTotLayer()
        {
            GameDataInit.data.currentLocation = gotolayerid;
            SceneLoader.instance.LoadScene("GameAdventure", 0f);
        }
        [ContextMenu("Go to next layer")]
        private void GoToNextLayer()
        {
            GameDataInit.data.currentLocation++;
            SceneLoader.instance.LoadScene("GameAdventure", 0f);
        }
        [ContextMenu("Go to prev layer")]
        private void GoToPrevLayer()
        {
            GameDataInit.data.currentLocation--;
            SceneLoader.instance.LoadScene("GameAdventure", 0f);
        }
        [ContextMenu("skip all point")]
        private void GoToNextPoints()
        {
            for (int i = 0; i < 99; i++)
            {
                try
                {
                    GameDataInit.data.tempData[GameDataInit.data.currentLocation].defeatedPoints.Add(
                        GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint);
                    GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint++;
                    GameAdventurePointsInit.instance.UpdatePoints();
                }
                catch { GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint--; break; }
            }
        }
        [ContextMenu("Go to next point")]
        private void GoToNextPoint()
        {
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].defeatedPoints.Add(
                GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint);
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint++;
            GameAdventurePointsInit.instance.UpdatePoints();
        }
        [ContextMenu("Go to prev point")]
        private void GoToPrevPoint()
        {
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].currentPoint--;
            GameAdventurePointsInit.instance.UpdatePoints();
        }
        [ContextMenu("add all defeated points")]
        private void AddAllDP()
        {
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].defeatedPoints = new List<int>();
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].defeatedPoints.AddRange(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            GameAdventurePointsInit.instance.UpdatePoints();
        }
        [ContextMenu("add all respawned points")]
        private void AddAllRP()
        {
            ResAllRP();
            for (int i = 0; i < 9; i++)
                GameDataInit.data.tempData[GameDataInit.data.currentLocation].respawnedPoints.Add(new RespawnedPointData(i, false));
            GameAdventurePointsInit.instance.UpdatePoints();
        }
        [ContextMenu("reset all respawned points")]
        private void ResAllRP()
        {
            GameDataInit.data.tempData[GameDataInit.data.currentLocation].respawnedPoints = new List<RespawnedPointData>();
            GameAdventurePointsInit.instance.UpdatePoints();
        }
        #endregion methods
    }
    public enum PointStatus { Normal, Defeated, Respawned };
}