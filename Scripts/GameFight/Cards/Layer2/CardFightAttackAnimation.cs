using Data;
using System.Collections;
using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class CardFightAttackAnimation : MonoBehaviour
    {
        #region fields
        [SerializeField] private CardFight cardFight;
        private AttackType lastCardAttackType;
        public int lastDamageGot { get; private set; } = 0;
        #endregion fields

        #region methods
        private void OnEnable()
        {
            cardFight.OnDamageTakenByEnemy += OnDamageTakenByEnemy;
            cardFight.OnHealToHPTaken += OnHealTaken;
        }
        private void OnDisable()
        {
            cardFight.OnDamageTakenByEnemy -= OnDamageTakenByEnemy;
            cardFight.OnHealToHPTaken -= OnHealTaken;
        }
        private void OnDamageTakenByEnemy(int damage, AttackType attackType)
        {
            if (damage > 0)
                StartCoroutine(ShakeCard());
            FightEffects.instance.DoEffect(cardFight.transform, lastCardAttackType, damage);
            lastDamageGot = damage;
        }
        private void OnHealTaken(int heal)
        {
            FightEffects.instance.DoEffect(cardFight.transform, AttackType.Heal, heal);
        }
        private IEnumerator ShakeCard()
        {
            Transform card = cardFight.cardInit.transform;
            Vector3 startPosition = card.position;
            int randomMoves = 3;
            float rndX = 0;
            float rndY = 0;

            for (int i = 0; i < randomMoves; i++)
            {
                if (i % 2 == 0)
                {
                    rndX = Random.Range(0.05f, 0.1f);
                    rndX = CustomMath.GetRandomChance(50) ? rndX : -rndX;
                    rndY = Random.Range(0.05f, 0.1f);
                    rndY = CustomMath.GetRandomChance(50) ? rndY : -rndY;
                }
                else
                {
                    rndX = -rndX;
                    rndY = -rndY;
                }
                Vector3 randomPosition = startPosition + Vector3.right * rndX + Vector3.up * rndY;
                yield return CustomAnimation.MoveTo(randomPosition, card.gameObject, 6f, 0.5f);
            }
            yield return CustomAnimation.MoveTo(startPosition, card.gameObject, 6f, 0.5f);
        }
        public IEnumerator WaitForAttackAnimation(CardFightInit attackingCard, float speed)
        {
            lastDamageGot = 0;
            GameObject attackingCardObj = attackingCard.transform.gameObject;
            Vector3 startPosition = attackingCard.transform.position;
            SetCardLayerUp(attackingCardObj);
            AttackType aType = attackingCard.attackType;
            lastCardAttackType = aType;
            switch (aType)
            {
                case AttackType.Charge:
                    yield return MoveForCharge(attackingCardObj, speed);
                    goto case AttackType.Fast;
                
                case AttackType.Fast:
                    yield return MoveToEnemy(attackingCardObj, speed * 2f);
                    cardFight.TryGetDamages(aType);
                    yield return MoveBack(attackingCardObj, startPosition, speed);
                    break;
                
                case AttackType.SliceCharge:
                    yield return MoveForCharge(attackingCardObj, speed);
                    goto case AttackType.SliceFast;
                
                case AttackType.SliceFast:
                    yield return MoveToEnemy(attackingCardObj, speed * 2f);
                    cardFight.TryGetDamages(aType);
                    yield return MoveForSlice(attackingCardObj, speed * 1.3f);
                    yield return MoveBack(attackingCardObj, startPosition, speed);
                    break;

                case AttackType.Heal:
                    yield return MoveToEnemy(attackingCardObj, speed * 2f);
                    cardFight.GetHealToHPFromAbility();
                    yield return MoveBack(attackingCardObj, startPosition, speed);
                    break;
                default: throw new System.NotImplementedException();
            }

            SetCardLayerBack(attackingCardObj);
        }
        private void SetCardLayerUp(GameObject attackingCard)
        {
            attackingCard.transform.SetParent(FightCardSpawner.instance.attackPanelAnimation.transform);
        }
        private void SetCardLayerBack(GameObject attackingCard)
        {
            if (CardFight.currentCard.cardInit.isEnemy)
                attackingCard.transform.SetParent(FightCardSpawner.instance.enemyPanel.transform);
            else
                attackingCard.transform.SetParent(FightCardSpawner.instance.allyPanel.transform);
        }

        #region moving
        private IEnumerator MoveForCharge(GameObject attackingCard, float speed)
        {
            yield return CustomAnimation.MoveTo((attackingCard.transform.position - cardFight.cardInit.transform.position), attackingCard, speed);
        }
        private IEnumerator MoveForSlice(GameObject attackingCard, float speed)
        {
            int scale = attackingCard.GetComponent<CardFightInit>().isEnemy switch
            {
                true => -1,
                false => 1
            };
            Vector3 direction = cardFight.cardInit.transform.position - attackingCard.transform.position;
            float xrnd = UnityEngine.Random.Range(2f, 6f);
            float yrnd = UnityEngine.Random.Range(2f, 6f);
            direction = direction + scale * Vector3.up * yrnd + Vector3.right * xrnd;
            Vector3 finalPosition = attackingCard.transform.position + direction;
            yield return CustomAnimation.MoveTo(finalPosition, attackingCard.transform.gameObject, speed);
        }
        private IEnumerator MoveBack(GameObject attackingCard, Vector3 startPosition, float speed)
        {
            yield return CustomAnimation.MoveTo(startPosition, attackingCard.transform.gameObject, speed);
        }
        private IEnumerator MoveToEnemy(GameObject attackingCard, float speed)
        {
            yield return CustomAnimation.MoveTo(cardFight.cardInit.transform.position, attackingCard, speed);
        }
        #endregion moving
        #endregion methods
    }
}