using System.Collections.Generic;
using UnityEngine;
using GameFight.Card;
using Universal;
using Data;

namespace GameFight
{
    public sealed class GameFightInit : HelpUpdate
    {
        #region fields & properties
        [SerializeField] private GameObject[] tutorialEnabled;
        [SerializeField] private SpriteRenderer[] bg;
        [SerializeField] private GameObject[] enableOnStart;
        public static int enemyIQDecrease { get; private set; } = 0; 
        #endregion fields & properties

        #region methods
        private void Start()
        {
            CardFightInit.description = GameDataInit.GetCardsDescription();
            CardFight.enemyCardsUsed = new List<CardFight>();
            CardFight.allyCardsUsed = new List<CardFight>();
            CardFightTurnInit.isEnemyTurn = false;
            CardFight.currentCard = null;
            CheckEnemyIQ();
            if (!GameDataInit.data.isTutorialCompleted)
                foreach (var el in tutorialEnabled)
                    el.SetActive(true);
            foreach (var el in enableOnStart)
                el.SetActive(true);
            foreach (SpriteRenderer el in bg)
                el.sprite = GameAdventure.FightPoint.fightParametersCopy.fightBG;
        }

        private void CheckEnemyIQ()
        {
            enemyIQDecrease = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.RottenBrain) ? 1 : 0;
            enemyIQDecrease = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.UniverseInAJar) ? 2 : enemyIQDecrease;
            enemyIQDecrease = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.PocketSun) ? 3 : enemyIQDecrease;
        }
        [ContextMenu("enable skip")]
        private void es()
        {
            ArtifactData artifactData = new ArtifactData();
            artifactData.effect = ArtifactEffect.Hourglass;
            artifactData.listPosition = 0;
            artifactData.id = 0;
            artifactData.onDesk = true;
            GameDataInit.data.artifactsData.Add(artifactData);
        }
        #endregion methods
    }
}