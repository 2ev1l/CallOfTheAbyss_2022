using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using GameAdventure;
using GameFight.Card;
using Data;
using Universal;
using System.Reflection;

namespace GameFight
{
    public class FightCardSpawner : SingleSceneInstance
    {
        #region fields & properties
        public static FightCardSpawner instance { get; private set; }
        public UnityAction OnCardSpawned;
        [field: SerializeField] public Canvas allyPanel { get; private set; }
        [field: SerializeField] public Canvas enemyPanel { get; private set; }
        [field: SerializeField] public GameObject attackPanelAnimation { get; private set; }
        [field: SerializeField] public GameObject[] cardsAllyPosition { get; private set; }
        [field: SerializeField] public GameObject[] cardsEnemyPosition { get; private set; }

        [SerializeField] private GameObject[] cardsEnemyOnSpawn;
        [SerializeField] private GameObject[] cardsAllyOnSpawn;
        [SerializeField] private GameObject cardFightPrefab;

        private int enemiesOnDesk = 0;
        private int alliesOnDesk = 0;
        private int lastPositionGot = 0;

        public float hpScale { get; private set; } = 1;
        public float dmgScale { get; private set; } = 1;
        public float defScale { get; private set; } = 1;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
            CheckArtifactEffects();
        }
        private void Start()
        {
            SetCardsPositions();
            StartCoroutine(TrySpawnCards(true));
            StartCoroutine(TrySpawnCards(false));
        }
        private void CheckArtifactEffects()
        {
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.RuneStone))
                dmgScale += 0.08f;
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.DarkStone))
                defScale += 0.1f;
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.EnchantedStone))
                hpScale += 0.06f;
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.GodsFlesh))
            {
                hpScale += 0.08f;
                defScale += 0.08f;
                dmgScale += 0.08f;
            }
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.RedCrystal))
                dmgScale += 0.1f;
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.WhiteCrystal))
                defScale += 0.14f;
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.VaryingCrystal))
                hpScale += 0.09f;
        }
        public IEnumerator TrySpawnCards(bool isEnemy, bool changeAnimationState = true)
        {
            if (changeAnimationState)
                FightAnimationInit.instance.OnChangeAnimationState?.Invoke(true);
            int i = 0;
            Vector3 finalPosition = GetFinalCardPosition(isEnemy);
            bool canSpawn = false;
            int enemyPanelSorting = enemyPanel.sortingOrder;
            int allyPanelSorting = allyPanel.sortingOrder;

            if (isEnemy)
            {
                enemyPanel.sortingOrder = 10;
                allyPanel.sortingOrder = 9;
                i = enemiesOnDesk;
                canSpawn = cardsEnemyOnSpawn[0].activeSelf;
            }
            else
            {
                enemyPanel.sortingOrder = 9;
                allyPanel.sortingOrder = 10;
                i = alliesOnDesk;
                canSpawn = cardsAllyOnSpawn[cardsAllyOnSpawn.Length - 1].activeSelf;
            }

            while (finalPosition != Vector3.zero && canSpawn)
            {
                GameObject spawnedCard = SpawnCard(isEnemy);
                InitializeSpawnedCard(i, spawnedCard.GetComponent<CardFightInit>(), isEnemy, out float scale, out GameObject trail);
                StartCoroutine(InitializeScale(spawnedCard));
                OnCardSpawned?.Invoke();
                yield return MoveToCenter(scale, spawnedCard);
                yield return MoveToSpawnPosition(finalPosition, spawnedCard);
                trail.SetActive(true);
                finalPosition = GetFinalCardPosition(isEnemy);
                i++;
            }
            enemyPanel.sortingOrder = enemyPanelSorting;
            allyPanel.sortingOrder = allyPanelSorting;

            FightAnimationInit.instance.OnAnimationEnd?.Invoke(isEnemy, changeAnimationState);
        }
        private void InitializeSpawnedCard(int index, CardFightInit spawnedCardInit, bool isEnemy, out float scale, out GameObject trail)
        {
            spawnedCardInit.cardFight.isFlipping = true;
            spawnedCardInit.fightPosition = lastPositionGot;
            CardInfoSO currentCard = isEnemy ? PrefabsData.instance.cardPrefabs[FightPoint.fightParametersCopy.mobsIdFinal[index]] : PrefabsData.instance.cardPrefabs[GameDataInit.deskCard(index).id];
            scale = -1f;

            if (isEnemy)
            {
                scale *= -1;
                spawnedCardInit.absolutePosition = -1;
                enemiesOnDesk++;
            }
            else
            {
                spawnedCardInit.absolutePosition = alliesOnDesk;
                alliesOnDesk++;
            }
            spawnedCardInit.Init(currentCard, isEnemy ? null : GameDataInit.deskCard(alliesOnDesk - 1));
            InitializeSpawnedCardTrail(spawnedCardInit, out GameObject newTrail);
            trail = newTrail;
        }
        private void InitializeSpawnedCardTrail(CardFightInit spawnedCardInit, out GameObject trail)
        {
            trail = spawnedCardInit.trail;
            Color trailColor = spawnedCardInit.creatureType switch
            {
                CreatureType.Ground => Color.red,
                CreatureType.Underwater => Color.blue,
                CreatureType.Flying => Color.cyan,
                _ => throw new System.NotImplementedException()
            };
            TrailRenderer trailRenderer = trail.GetComponent<TrailRenderer>();
            Gradient newGradient = new Gradient();
            newGradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(trailColor, 0.0f), new GradientColorKey(Color.black, 1.0f) },
                trailRenderer.colorGradient.alphaKeys);
            trailRenderer.colorGradient = newGradient;

            trail.SetActive(false);
        }
        private IEnumerator MoveToSpawnPosition(Vector3 finalPosition, GameObject spawnedCard)
        {
            yield return CustomAnimation.MoveTo(finalPosition, spawnedCard, 0.5f);
        }
        private IEnumerator MoveToCenter(float scale, GameObject spawnedCard)
        {
            yield return CustomAnimation.MoveTo(Vector3.right * scale, spawnedCard, 0.7f);
        }
        private IEnumerator InitializeScale(GameObject card)
        {
            yield return SetScale(card);
            StartCoroutine(FlipCard(card));
        }
        private IEnumerator SetScale(GameObject card)
        {
            float angle = 0.01f;
            Transform cardTransform = card.transform;
            while (cardTransform.localScale.z < CardFight.defaultScale)
            {
                if (FightAnimationInit.skipAnimation) break;

                cardTransform.localScale += angle * Vector3.one * FightAnimationInit.animationSpeed * 2f;
                yield return new WaitForFixedUpdate();
            }
            cardTransform.localScale = Vector3.one * CardFight.defaultScale;
        }
        private IEnumerator FlipCard(GameObject card)
        {
            float angle = 0.01f;
            CardFightInit currentCardInit = card.GetComponent<CardFightInit>();
            Transform cardTransform = card.transform;
            float scaleSpeed = 3f;
            while (cardTransform.localScale.x > 0)
            {
                if (FightAnimationInit.skipAnimation) break;

                cardTransform.localScale -= scaleSpeed * angle * FightAnimationInit.animationSpeed * Vector3.right;
                yield return new WaitForFixedUpdate();
            }
            currentCardInit.back.SetActive(false);
            while (cardTransform.localScale.x < CardFight.defaultScale)
            {
                if (FightAnimationInit.skipAnimation) break;

                cardTransform.localScale += scaleSpeed * angle * FightAnimationInit.animationSpeed * Vector3.right;
                yield return new WaitForFixedUpdate();
            }
            Vector3 scale = card.transform.localScale;
            scale.x = CardFight.defaultScale;
            cardTransform.localScale = scale;
            currentCardInit.cardFight.isFlipping = false;
        }
        private GameObject SpawnCard(bool isEnemy)
        {
            Vector3 spawnPosition = GetSpawnPosition(isEnemy);
            GameObject spawnedCard = Instantiate(cardFightPrefab, spawnPosition, Quaternion.identity, GameObject.Find(isEnemy ? "EnemyPanel" : "AllyPanel").transform);
            spawnedCard.transform.position = spawnPosition;
            UpdateSpawnPositions(isEnemy);
            return spawnedCard;
        }
        private Vector3 GetSpawnPosition(bool isEnemy)
        {
            if (isEnemy)
            {
                int maxValue = Mathf.Max(FightPoint.fightParametersCopy.mobsIdFinal.Count - 1 + FightPoint.fightParametersCopy.maxHandSize, 2);
                for (int i = maxValue + 1; i <= 2; i++)
                {
                    cardsEnemyOnSpawn[i].SetActive(false);
                }
                return cardsEnemyOnSpawn[0].transform.position;
            }
            else
            {
                int maxValue = Mathf.Max(GameDataInit.deskCards.Count - 1 + GameDataInit.data.maxHandSize, 2);
                for (int i = maxValue + 1; i <= 2; i++)
                {
                    cardsAllyOnSpawn[maxValue + 1 - i].SetActive(false);
                }
                return cardsAllyOnSpawn[cardsAllyOnSpawn.Length - 1].transform.position;
            }
        }
        private void UpdateSpawnPositions(bool isEnemy)
        {
            if (isEnemy)
            {
                int cardLasts = FightPoint.fightParametersCopy.mobsIdFinal.Count - enemiesOnDesk;
                for (int i = 0; i < cardsEnemyOnSpawn.Length; i++)
                {
                    if (i + 2 > cardLasts)
                        cardsEnemyOnSpawn[i].SetActive(false);
                }
            }
            else
            {
                int cardLasts = GameDataInit.deskCards.Count - alliesOnDesk;
                for (int i = 0; i < cardsAllyOnSpawn.Length; i++)
                {
                    if (i + 2 > cardLasts)
                        cardsAllyOnSpawn[cardsAllyOnSpawn.Length - i - 1].SetActive(false);
                    else
                    {
                        cardsAllyOnSpawn[cardsAllyOnSpawn.Length - i - 1].GetComponent<Image>().sprite = FightStorage.instance.backAllyAliveSprites[PrefabsData.instance.cardPrefabs[GameDataInit.deskCard(alliesOnDesk + i + 1).id].rareTier];
                    }
                }
            }
        }
        private Vector3 GetFinalCardPosition(bool isEnemy)
        {
            GameObject[] positions = isEnemy ? cardsEnemyPosition : cardsAllyPosition;
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].activeSelf)
                {
                    positions[i].SetActive(false);
                    lastPositionGot = i;
                    return positions[i].transform.position;
                }
            }
            return Vector3.zero;
        }
        private void SetCardsPositions()
        {
            for (int i = Mathf.Min(GameDataInit.data.maxHandSize, GameDataInit.deskCards.Count); i < cardsAllyPosition.Length; i++)
                cardsAllyPosition[i].SetActive(false);
            for (int i = Mathf.Min(FightPoint.fightParametersCopy.mobsIdFinal.Count, FightPoint.fightParametersCopy.maxHandSize); i < cardsEnemyPosition.Length; i++)
                cardsEnemyPosition[i].SetActive(false);
        }
        #endregion methods
    }
}