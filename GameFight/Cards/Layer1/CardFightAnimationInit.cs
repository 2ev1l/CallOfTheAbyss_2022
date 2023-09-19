using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;
using static GameFight.Card.CardFight;

namespace GameFight.Card
{
    public class CardFightAnimationInit : MonoBehaviour
    {
        #region fields
        [SerializeField] private CardFightInit cardFightInit;
        private bool onDeathPosition;
        #endregion fields

        #region methods
        public IEnumerator InitCardDeath()
        {
            if (onDeathPosition) yield break;
            cardFightInit.trail.SetActive(false);
            Canvas deathPanel = FightStorage.instance.deathAnimationPanel;
            deathPanel.sortingOrder = 14;
            yield return DeathProgression();
            deathPanel.sortingOrder = 5;
            onDeathPosition = true;
            yield return CustomMath.WaitAFrame();
        }
        private IEnumerator DeathProgression()
        {
            FightStorage fightStorage = FightStorage.instance;
            List<GameObject> deathPositions = transform.parent.name.Equals("AllyPanel") ? fightStorage.cardsAllyOnDeath : fightStorage.cardsEnemyOnDeath;
            GameObject deathPosition = GetDeathPosition(deathPositions);
            cardFightInit.OnCardDeath?.Invoke(cardFightInit.isEnemy);
            TryAddReward();

            yield return CustomMath.WaitAFrame();

            StartCoroutine(FlipCardBack());
            yield return MoveToCenter();
            yield return MoveToDeathPoint(deathPosition);

            if (cardFightInit.isEnemy)
                FightCardSpawner.instance.cardsEnemyPosition[cardFightInit.fightPosition].SetActive(true);
            else
                FightCardSpawner.instance.cardsAllyPosition[cardFightInit.fightPosition].SetActive(true);

            yield return FightCardSpawner.instance.TrySpawnCards(cardFightInit.isEnemy, false);
            transform.position = deathPosition.transform.position;
        }
        private void TryAddReward()
        {
            if (!cardFightInit.isEnemy) return;
            float mult = cardFightInit.rareTier switch
            {
                0 => 1,
                1 => 1.1f,
                2 => 1.4f,
                3 => 1.6f,
                _ => throw new System.NotImplementedException(),
            };
            float goldMult = cardFightInit.rareTier switch
            {
                0 => 1,
                1 => 1f,
                2 => 1.1f,
                3 => 0.9f,
                _ => throw new System.NotImplementedException(),
            };
            float silverMult = cardFightInit.rareTier switch
            {
                0 => 1,
                1 => 1f,
                2 => 1f,
                3 => 1.15f,
                _ => throw new System.NotImplementedException(),
            };
            mult *= GameDataInit.data.percentRewardOnWin;
            FightStorage.totalSilverGain += Mathf.RoundToInt(cardFightInit.silverPrice * mult * silverMult);
            FightStorage.totalGoldGain += Mathf.RoundToInt(cardFightInit.goldPrice * mult * goldMult);
        }
        private IEnumerator MoveToCenter()
        {
            yield return CustomAnimation.MoveTo(Vector3.zero, transform.gameObject, 0.7f);
        }
        private IEnumerator MoveToDeathPoint(GameObject deathPosition)
        {
            yield return CustomAnimation.MoveTo(deathPosition.transform.position, transform.gameObject, 0.7f);
        }
        private GameObject GetDeathPosition(List<GameObject> deathPos)
        {
            GameObject unActivePos = deathPos.Find(x => !x.activeSelf);
            if (unActivePos != null)
                unActivePos.SetActive(true);
            else
                unActivePos = deathPos[deathPos.Count - 1];
            return unActivePos;
        }
        private IEnumerator FlipCardBack()
        {
            yield return CustomMath.WaitAFrame();
            float angle = 0.01f;
            float scaleSpeed = 3f;
            float finalCardScale = CardFight.defaultScale - CardFight.plusScale * 2;
            while (transform.localScale.x > 0)
            {
                if (FightAnimationInit.skipAnimation) break;

                transform.localScale -= scaleSpeed * angle * FightAnimationInit.animationSpeed * Vector3.right;
                if (transform.localScale.y > finalCardScale)
                {
                    transform.localScale -= scaleSpeed * angle * FightAnimationInit.animationSpeed * Vector3.up;
                    transform.localScale -= scaleSpeed * angle * FightAnimationInit.animationSpeed * Vector3.forward;
                }
                yield return new WaitForFixedUpdate();
            }

            cardFightInit.back.SetActive(true);
            cardFightInit.back.GetComponent<ShowHelp>().enabled = true;
            cardFightInit.back.GetComponent<ShowHelp>().id = 33;

            while (transform.localScale.x < finalCardScale)
            {
                if (FightAnimationInit.skipAnimation) break;

                transform.localScale += scaleSpeed * angle * FightAnimationInit.animationSpeed * Vector3.right;
                yield return new WaitForFixedUpdate();
            }
            transform.localScale = Vector3.one * finalCardScale;
        }
        public IEnumerator CardScale(float speed)
        {
            Transform cardTransform = cardFightInit.gameObject.transform;
            CardFight cardFight = cardFightInit.cardFight;
            while (cardTransform.localScale.x < defaultScale + plusScale)
            {
                yield return new WaitForFixedUpdate();
                if (!cardFight.isScaling)
                {
                    if (cardFight.isSelected)
                        cardTransform.localScale = Vector3.one * (defaultScale + plusScale);
                    else
                        cardTransform.localScale = Vector3.one * defaultScale;
                    yield break;
                }
                cardTransform.localScale *= (1 + speed / defaultScale / 1000f * FightAnimationInit.animationSpeed);
            }
            while (cardTransform.localScale.x > defaultScale - plusScale)
            {
                yield return new WaitForFixedUpdate();
                if (!cardFight.isScaling)
                {
                    if (cardFight.isSelected)
                        cardTransform.localScale = Vector3.one * (defaultScale + plusScale);
                    else
                        cardTransform.localScale = Vector3.one * defaultScale;
                    yield break;
                }
                cardTransform.localScale *= (1 - speed / defaultScale / 1000f * FightAnimationInit.animationSpeed);
            }
            StartCoroutine(CardScale(speed));
        }

        #endregion methods
    }
}