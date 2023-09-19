using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class RectTransformUpdater : DefaultUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;
        [SerializeField] private RectTransform cardRect;
        
        protected override void OnEnable()
        {
            cardFightInit.OnCardDeath += OnDeath;
            cardFightInit.OnCardSpawn += OnSpawn;
        }
        protected override void OnDisable()
        {
            cardFightInit.OnCardDeath -= OnDeath;
            cardFightInit.OnCardSpawn -= OnSpawn;
        }
        private void OnDeath(bool isEnemy)
        {
            cardRect.anchorMin = Vector2.one / 2f;
            cardRect.anchorMax = Vector2.one / 2f;
            int mult = isEnemy ? 1 : -1;
            cardRect.anchoredPosition += mult * (Vector2.right * 1280 + Vector2.up * 720);

            cardFightInit.transform.SetParent(GameObject.Find("DeathPanelAnimation").transform);
            Vector2 newVec = isEnemy ? Vector2.one : Vector2.zero;
            cardRect.anchorMin = newVec;
            cardRect.anchorMax = newVec;
            cardRect.anchoredPosition -= mult * (Vector2.right * 1280 + Vector2.up * 720);
        }
        private void OnSpawn(bool isEnemy)
        {
            cardRect.localScale = isEnemy ? FightStorage.instance.enemySpawnPointRect.localScale : FightStorage.instance.allySpawnPointRect.localScale;
            int mult = isEnemy ? -1 : 1;
            Vector2 newVec = isEnemy ? Vector2.one : Vector2.zero;
            cardRect.anchorMin = newVec;
            cardRect.anchorMax = newVec;
            cardRect.anchoredPosition += mult * (Vector2.right * 1280 + Vector2.up * 720);
        }
    }
}