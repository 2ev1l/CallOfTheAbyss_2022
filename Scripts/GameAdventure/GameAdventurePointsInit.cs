using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Universal;
using static GameAdventure.GameAdventureInit;

namespace GameAdventure
{
    public class GameAdventurePointsInit : SingleSceneInstance
    {
        #region fields & properties
        public UnityAction OnPointsInit;
        public static GameAdventurePointsInit instance { get; private set; }

        [SerializeField] private GameObject backButton;

        private List<FightPoint> currentIconsFight;
        private List<GameObject> currentIconsNext;
        [SerializeField][HideInInspector] private List<SpriteRenderer> currentIconsNextSprite;

        public static float mobsRespawnChance = 15f;
        public static int mobsRespawnLocation = 2;
        public static int eventRespawnLocation = 3;

        public static bool isInvisibleFlowerApplied;
        public static bool isInvisibleFlowerLocation = false;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            OnPointsInit += InitPoints;
        }
        private void OnDisable()
        {
            OnPointsInit -= InitPoints;
        }
        private void InitPoints()
        {
            if (!GameDataInit.data.isTutorialCompleted)
            {
                UpdateIconsTable();
                UpdatePoints();
            }
            else
            {
                UpdateIconsTable();
                ValidatePoints(out int currentPoint, out int reachedPoint, out List<int> defeatedPoints, out List<RespawnedPointData> respawnedPoints, out List<int> respawnedPointsID);
                if (GameDataInit.data.reachedLocation >= mobsRespawnLocation)
                    SetRandomPoints(GameDataInit.data.currentLocation);
                UpdatePoints();
            }
        }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
            CheckArtifactEffects();
            CheckDifficulty();
        }
        private void CheckArtifactEffects()
        {
            isInvisibleFlowerApplied = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.InvisibleFlower);
        }
        private void CheckDifficulty()
        {
            if (GameDataInit.data.difficulty != Difficulty.Hard) return;
            mobsRespawnChance = 1;
            eventRespawnLocation = 2;
        }
        public void SetRandomPoints(int atLocation)
        {
            TempData location = GameDataInit.data.tempData[atLocation];
            int currentPoint = location.currentPoint;
            List<int> defeatedPoints = location.defeatedPoints;
            List<RespawnedPointData> respawnedPoints = location.respawnedPoints;
            int nmb = CustomMath.GetRandomChance() switch
            {
                float i when i < 10 => 2,
                float i when i < 80 => 3,
                float i when i <= 100 => 4,
                _ => 3
            };
            int cnt = 0;
            while (cnt <= defeatedPoints.Count - 1)
            {
                if (Mathf.Abs(defeatedPoints[cnt] - currentPoint) >= nmb && CustomMath.GetRandomChance(mobsRespawnChance))
                {
                    AddRespawnedPoint(defeatedPoints, respawnedPoints, cnt, atLocation);
                    continue;
                }
                cnt++;
            }
        }
        private void AddRespawnedPoint(List<int> defeatedPoints, List<RespawnedPointData> respawnedPoints, int cnt, int atLocation)
        {
            RespawnedPointData pointData = new RespawnedPointData(-1, false);
            switch (CustomMath.GetRandomChance())
            {
                //random event
                case float i when i < 35 && eventRespawnLocation <= GameDataInit.data.reachedLocation:
                    pointData.id = defeatedPoints[cnt];
                    pointData.isRandomEvent = true;
                    print($"{defeatedPoints[cnt]} respawned - random event - {atLocation} location");
                    defeatedPoints.RemoveAt(cnt);
                    break;
                //respawned point
                case float i when i <= 100:
                    //print("respawned enemies count = " + currentIconsFight[defeatedPoints[cnt]].fightParameters.respawnedEnemiesCount);
                    pointData.id = defeatedPoints[cnt];
                    pointData.isRandomEvent = false;
                    print($"{defeatedPoints[cnt]} tried to respawn - fight - {atLocation} location");
                    defeatedPoints.RemoveAt(cnt);
                    break;
            }
            if (pointData.id != -1)
                respawnedPoints.Add(pointData);
        }
        private List<int> GetRespawnedPointsID(TempData location)
        {
            List<int> respawnedPointsID = new List<int>();
            foreach (var el in location.respawnedPoints)
            {
                if (el.id == -1) Debug.LogError("RESPAWNED ID IS -1");
                respawnedPointsID.Add(el.id);
            }
            return respawnedPointsID;
        }
        private void ResetRespawnedPointsID(List<int> respawnedPointsID, TempData location)
        {
            location.respawnedPoints = new List<RespawnedPointData>();
            int i = 0;
            while (i < respawnedPointsID.Count)
            {
                RespawnedPointData pointData = new RespawnedPointData(-1, false);
                pointData.id = respawnedPointsID[i];
                location.respawnedPoints.Add(pointData);
                i++;
            }
        }
        private void UpdateIconsTable()
        {
            currentIconsFight = new List<FightPoint>();
            currentIconsNext = new List<GameObject>();
            for (int i = 0; i < currentStage.transform.childCount; i++)
            {
                try
                {
                    GameObject icon = currentStage.transform.Find($"IconFight_{i}").gameObject;
                    FightPoint fightPoint = icon.GetComponent<FightPoint>();
                    currentIconsFight.Add(fightPoint);
                    icon = currentStage.transform.Find($"IconNext_{i}").gameObject;
                    currentIconsNext.Add(icon);
                    currentIconsNextSprite.Add(icon.GetComponent<SpriteRenderer>());
                }
                catch
                {
                    break;
                }
            }
        }
        public void UpdatePoints()
        {
            ValidatePoints(out int currentPoint, out int reachedPoint, out List<int> defeatedPoints, out List<RespawnedPointData> respawnedPoints, out List<int> respawnedPointsID);

            ResetPoints(currentPoint, reachedPoint, defeatedPoints);
            UpdateNormalPoints(defeatedPoints, respawnedPointsID);
            UpdateDefeatedPoints(defeatedPoints);
            UpdateRespawnedPoints(respawnedPoints);
            UpdateCurrentPoint(currentPoint, reachedPoint, defeatedPoints, respawnedPointsID);
            UpdateUI(currentPoint, respawnedPointsID);
        }
        private void ValidatePoints(out int currentPoint, out int reachedPoint, out List<int> defeatedPoints, out List<RespawnedPointData> respawnedPoints, out List<int> respawnedPointsID)
        {
            TempData location = GameDataInit.data.tempData[GameDataInit.data.currentLocation];
            currentPoint = location.currentPoint;
            reachedPoint = location.reachedPoint;
            defeatedPoints = location.defeatedPoints;
            respawnedPointsID = GetRespawnedPointsID(location);
            CheckRespawnedPoints(location, respawnedPointsID);
            respawnedPoints = location.respawnedPoints;

            int maxPoint = Mathf.Max(currentIconsFight.Count - 1, 0);
            int maxDefeatedPoint = CustomMath.FindMax(defeatedPoints, x => x);
            int maxRespawnedPoint = CustomMath.FindMax(respawnedPointsID, x => x);

            if (reachedPoint > currentIconsFight.Count || maxDefeatedPoint > maxPoint || maxRespawnedPoint > maxPoint)
            {
                ValidateList(defeatedPoints, maxPoint);
                ValidateList(respawnedPointsID, maxPoint);
                currentPoint = maxPoint;
                reachedPoint = maxPoint;
                location.currentPoint = currentPoint;
                location.reachedPoint = reachedPoint;
                location.defeatedPoints = defeatedPoints;
                ResetRespawnedPointsID(respawnedPointsID, location);
                respawnedPoints = location.respawnedPoints;
            }
        }
        private void CheckRespawnedPoints(TempData location, List<int> respawnedPointsID)
        {
            int i = 0;
            while (i < respawnedPointsID.Count)
            {
                if (currentIconsFight[respawnedPointsID[i]].fightParameters.respawnedEnemiesCount == 0)
                {
                    location.defeatedPoints.Add(respawnedPointsID[i]);
                    respawnedPointsID.RemoveAt(i);
                    location.respawnedPoints.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        private List<int> ValidateList(List<int> list, int maxPoint)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > maxPoint)
                {
                    list.RemoveRange(i, list.Count - i);
                    break;
                }
            }
            return list;
        }
        private void ResetPoints(int currentPoint, int reachedPoint, List<int> defeatedPoints)
        {
            for (int i = currentIconsFight.Count - 1; i >= 0; i--)
            {
                currentIconsFight[i].EnableTrigger((Mathf.Abs(currentPoint - i) <= 1) && defeatedPoints.Contains(currentPoint));
                if (i != currentIconsNext.Count)
                {
                    currentIconsNext[i].SetActive(((i == currentPoint && currentPoint < reachedPoint) || i == currentPoint - 1) && defeatedPoints.Contains(currentPoint));
                    currentIconsNextSprite[i].color = new Color(1f, 1f, 1f, (currentIconsFight.Count - currentPoint + 1) / (float)currentIconsFight.Count);
                }

                if (i > reachedPoint)
                    currentIconsFight[i].gameObject.SetActive(false);
                float colorDecrease = Mathf.Abs(i - currentPoint) / 5f;
                currentIconsFight[i].ChangeSpriteColor(new Color(1f, 1f, 1f, 1 - colorDecrease));
                currentIconsFight[i].ChangeLayoutSpriteColor(new Color(0f, 0f, 0f, 0.5f - colorDecrease));
            }
        }
        private void UpdateNormalPoints(List<int> defeatedPoints, List<int> respawnedPoints)
        {
            List<int> excludedPoints = new List<int>();
            excludedPoints.AddRange(defeatedPoints);
            excludedPoints.AddRange(respawnedPoints);
            for (int i = 0; i < currentIconsFight.Count; i++)
            {
                if (excludedPoints.Contains(i)) continue;
                currentIconsFight[i].UpdatePointStatus(PointStatus.Normal);
            }
        }
        private void UpdateDefeatedPoints(List<int> defeatedPoints)
        {
            foreach (int el in defeatedPoints)
            {
                currentIconsFight[el].UpdatePointStatus(PointStatus.Defeated);
            }
        }
        private void UpdateRespawnedPoints(List<RespawnedPointData> respawnedPoints)
        {
            foreach (var el in respawnedPoints)
            {
                currentIconsFight[el.id].isRandomEvent = el.isRandomEvent;
                currentIconsFight[el.id].UpdatePointStatus(PointStatus.Respawned);
            }
        }
        private void UpdateCurrentPoint(int currentPoint, int reachedPoint, List<int> defeatedPoints, List<int> respawnedPoints)
        {
            FightPoint currentFightPoint = currentIconsFight[currentPoint];
            currentFightPoint.EnableTrigger(true);
            for (int i = currentPoint; i < currentIconsFight.Count; i++)
            {
                currentIconsFight[i].gameObject.SetActive(true);

                if (!defeatedPoints.Contains(i)) break;
            }

            if (defeatedPoints.Contains(currentPoint))
            {
                currentFightPoint.EnableTrigger(currentFightPoint.fightParameters.isLastLocation);
                if (!currentFightPoint.fightParameters.isLastLocation)
                    currentFightPoint.ChangeSprite(GameAdventureIconsStorage.instance.iconCurrent);

                if (currentPoint + 1 < currentIconsFight.Count)
                {
                    currentIconsFight[currentPoint + 1].EnableTrigger(true);
                    currentIconsNext[currentPoint].SetActive(true);
                }
                if (currentPoint - 1 >= 0)
                {
                    currentIconsFight[currentPoint - 1].gameObject.SetActive(true);
                    currentIconsFight[currentPoint - 1].EnableTrigger(true);
                    currentIconsNext[currentPoint - 1].SetActive(true);
                }
            }
            else
            {
                if (currentPoint == reachedPoint && currentPoint - 1 >= 0)
                    currentIconsFight[currentPoint - 1].EnableTrigger(false); //disable if clicked on fight and returned to this scene
            }
            CheckInvisibleFlowerEffect(defeatedPoints);
        }
        private void CheckInvisibleFlowerEffect(List<int> defeatedPoints)
        {
            isInvisibleFlowerLocation = GameDataInit.data.currentLocation == 4;
            if (!isInvisibleFlowerLocation || CustomMath.FindMax(defeatedPoints, x => x) != 6) return;
            FightPoint finalPoint = currentIconsFight.Find(x => x.fightParameters.isLastLocation);
            finalPoint.UpdatePointStatus(PointStatus.Defeated, 1);
        }
        private void UpdateUI(int currentPoint, List<int> respawnedPoints)
        {
            backButton.GetComponent<ShowHelp>().id = GameDataInit.data.currentLocation == GameDataInit.data.locationOffset ? 6 : 21;
            backButton.SetActive(!respawnedPoints.Contains(0) && currentPoint == 0);

            float mult = (currentIconsFight.Count - currentPoint + 1) / (float)currentIconsFight.Count;
            currentStage.GetComponent<Image>().color = new Color(1f * mult, 1f * mult, 1f * mult, 1f);
        }
        #endregion methods
    }
}