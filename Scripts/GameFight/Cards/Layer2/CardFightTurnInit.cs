using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class CardFightTurnInit : MonoBehaviour
    {
        [SerializeField] private CardFight cardFight;
        public static bool isEnemyTurn;
        private static Canvas allyCanvas;
        private static Canvas enemyCanvas;

        private void Awake()
        {
            allyCanvas = GameObject.Find(CardFight.allyPanel).GetComponent<Canvas>();
            enemyCanvas = GameObject.Find(CardFight.enemyPanel).GetComponent<Canvas>();
        }
        public IEnumerator StartNextTurn()
        {
            if (!CardFight.currentCard.cardInit.isEnemy)
            {
                isEnemyTurn = true;
                CardFight.currentCard = null;
                yield return StartEnemyTurn();
            }
            else
            {
                isEnemyTurn = false;
                yield return StartAllyTurn();
            }
        }
        private IEnumerator StartAllyTurn()
        {
            cardFight.OnCardPriorityChange?.Invoke(false);
            yield return CustomMath.WaitAFrame();

            if (GetAllowedCard(CardFight.allyPanel) == null)
            {
                CardFight.allyCardsUsed = new List<CardFight>();
                cardFight.OnCardPriorityChange?.Invoke(false);
            }

            CardFight.currentCard = null;
            FightAnimationInit.instance.OnChangeAnimationState?.Invoke(false);
        }
        private IEnumerator StartEnemyTurn()
        {
            cardFight.OnCardPriorityChange?.Invoke(true);
            yield return CustomMath.WaitAFrame();

            CardFight enemyCard = GetAllowedCard(CardFight.enemyPanel);
            if (enemyCard == null)
            {
                CardFight.enemyCardsUsed = new List<CardFight>();
                cardFight.OnCardPriorityChange?.Invoke(true);
                enemyCard = GetAllowedCard(CardFight.enemyPanel);
            }

            CardFight.currentCard = enemyCard;
            CardFight allyCard = GetAllowedCard(CardFight.allyPanel);
            if (allyCard == null)
                yield break;
            allyCanvas.sortingOrder = 9;
            enemyCanvas.sortingOrder = 10;
            allyCard.HitThisCard();
        }
        private CardFight GetAllowedCard(string panelName)
        {
            List<CardFight> allowedCards = new List<CardFight>();
            foreach (GameObject el in CardFight.GetChildCardsInParent(panelName))
            {
                CardFightInit elCardInit = el.GetComponent<CardFightInit>();
                if (!elCardInit.IsCardDead() && elCardInit.canBeChoosed)
                    allowedCards.Add(elCardInit.cardFight);
            }

            return (panelName.Equals(CardFight.enemyPanel)) ? GetAllowedEnemyCard(allowedCards) : GetAllowedAllyCard(allowedCards);
        }
        private CardFight GetAllowedEnemyCard(List<CardFight> allowedCards)
        {
            if (!IsEnemySmart(0)) return CheckAllowedCards(allowedCards);
            if (IsEnemySmart(2))
                allowedCards = CustomMath.FindAllResults(allowedCards, x => x.cardInit.statusEffect.damage * x.cardInit.statusEffect.duration * (x.cardInit.statusEffect.isIgnoreDefense ? 1.3f : 1), FindResult.Max);
            allowedCards = CustomMath.FindAllResults(allowedCards, x => x.cardInit.damage, FindResult.Max);

            if (!IsEnemySmart(1)) return CheckAllowedCards(allowedCards);
            allowedCards = CustomMath.FindAllResults(allowedCards, x => x.cardInit.hp, FindResult.Min);

            return CheckAllowedCards(allowedCards);
        }
        private CardFight GetAllowedAllyCard(List<CardFight> allowedCards)
        {
            if (!IsEnemySmart(0)) return CheckAllowedCards(allowedCards);
            allowedCards = CustomMath.FindAllResults(allowedCards, x => x.cardInit.defense, FindResult.Min);
            if (IsEnemySmart(2))
                allowedCards = CustomMath.FindAllResults(allowedCards, x => x.cardInit.cardFight.statusEffectsInit.effectsApplied.Count, FindResult.Min);

            if (!IsEnemySmart(1)) return CheckAllowedCards(allowedCards);
            allowedCards = CustomMath.FindAllResults(allowedCards, x => x.cardInit.hp, FindResult.Min);

            return CheckAllowedCards(allowedCards);
        }
        private bool IsEnemySmart(short smartLevel) => smartLevel switch
        {
            0 => GameDataInit.data.reachedLocation >= 1 && GameFightInit.enemyIQDecrease < 3,
            1 => GameDataInit.data.reachedLocation >= 2 && GameFightInit.enemyIQDecrease < 1,
            2 => GameDataInit.data.reachedLocation >= 3 && GameFightInit.enemyIQDecrease < 2,
            _ => throw new System.NotImplementedException()
        };
        private CardFight CheckAllowedCards(List<CardFight> allowedCards) => (allowedCards.Count == 0) ? null : allowedCards[Random.Range(0, allowedCards.Count)];
    }
}