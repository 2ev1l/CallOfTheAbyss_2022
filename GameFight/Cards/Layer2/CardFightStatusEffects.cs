using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Data;
using Universal;
using GameFight.Equipment;

namespace GameFight.Card
{
    public class CardFightStatusEffects : DefaultUpdater
    {
        #region fields & properties
        public UnityAction<int> OnDamageTakenByStatus;
        public UnityAction<CardFightInit> OnStatusUpdate;

        [SerializeField] private CardFight cardFight;
        [SerializeField] private Image statusPanel;
        public List<StatusEffect> effectsApplied { get; private set; } = new List<StatusEffect>();
        #endregion fields & properties

        protected override void OnEnable()
        {
            OnStatusUpdate += UpdateStatusUI;
            cardFight.OnDamageTakenByEnemy += OnEnemyDamaged;
        }
        protected override void OnDisable()
        {
            OnStatusUpdate -= UpdateStatusUI;
            cardFight.OnDamageTakenByEnemy -= OnEnemyDamaged;
        }
        private void OnEnemyDamaged(int damage, AttackType attackType)
        {
            if (CardFight.currentCard.cardInit.statusEffect.effect != StatusType.None && cardFight.cardInit.hp > 0)
            {
                if ((damage <= 0 && CardFight.currentCard.cardInit.statusEffect.isIgnoreDefense) || damage > 0)
                    SetStatus(cardFight.cardInit.cardFight, CardFight.currentCard.cardInit);
            }
        }
        private void UpdateStatusUI(CardFightInit card)
        {
            switch (effectsApplied.Count)
            {
                case 0:
                    statusPanel.gameObject.SetActive(false);
                    break;
                case 1:
                    statusPanel.gameObject.SetActive(true);
                    statusPanel.sprite = FightStorage.instance.statusEffectSprites[(int)effectsApplied[0].effect];
                    break;
                case int i when i > 1:
                    statusPanel.gameObject.SetActive(true);
                    statusPanel.sprite = FightStorage.instance.statusEffectSprites[0];
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            int damageGainOnNextTurn = card.cardFight.statusEffectsInit.GetStatusDamage(card.cardFight);
            if (damageGainOnNextTurn > 0 && CardFightTurnInit.isEnemyTurn == card.isEnemy)
                card.OnHPPreviewChanged?.Invoke(damageGainOnNextTurn, false);
            else
                card.OnHPPreviewChanged?.Invoke(-1, false);
        }
        public void UpdateStatuses()
        {
            CardFightTurnInit.isEnemyTurn = !CardFightTurnInit.isEnemyTurn;
            UpdateStatusAllCards();
            CardFightTurnInit.isEnemyTurn = !CardFightTurnInit.isEnemyTurn;
        }
        public IEnumerator MakeStatusEffectsDamage()
        {
            var cardsWithStatus = FindAllCardsWithStatus(CardFight.currentCard.cardInit.isEnemy);
            foreach (GameObject el in cardsWithStatus)
            {
                CardFightInit cfi = el.GetComponent<CardFightInit>();
                yield return MakeStatusDamage(cfi.cardFight);
                if (cfi.IsCardDead())
                    yield return cfi.cardFightAnimation.InitCardDeath();
            }
            if (cardFight.cardInit.IsCardDead())
                yield return cardFight.cardInit.cardFightAnimation.InitCardDeath();
        }
        private void UpdateStatusAllCards()
        {
            UpdateStatusAllCards(false);
            UpdateStatusAllCards(true);
        }
        private void UpdateStatusAllCards(bool isEnemy)
        {
            var cardsWithStatus = FindAllCardsWithStatus(isEnemy);
            foreach (var el in cardsWithStatus)
            {
                CardFightInit cfi = el.GetComponent<CardFightInit>();
                OnStatusUpdate?.Invoke(cfi);
            }
        }
        private void SetStatus(CardFight toCard, CardFightInit fromCard)
        {
            bool isCurrentStatusApplied = false;
            StatusEffect newStatus = fromCard.statusEffect;
            List<StatusEffect> newStatusEffects = toCard.statusEffectsInit.effectsApplied;
            for (int i = 0; i < newStatusEffects.Count; i++)
            {
                if (newStatusEffects[i].effect == newStatus.effect)
                {
                    newStatusEffects[i].duration = newStatus.duration;
                    if (newStatusEffects.Count >= 1 && newStatus.isStackingWithCurrent)
                    {
                        newStatusEffects[i].damage += newStatus.damage;
                    }
                    isCurrentStatusApplied = true;
                    break;
                }
            }
            if (newStatusEffects.Count >= 1)
            {
                if (!isCurrentStatusApplied && newStatus.isStackingWithOther)
                    AddStatus(toCard);
            }
            else
                AddStatus(toCard);
        }
        private IEnumerator MakeStatusDamage(CardFight card)
        {
            int possibleDamage = GetStatusDamage(card);
            DecreaseStatusApplied(card);
            if (card.specialAbilities.TryDarkness(card.cardInit, possibleDamage) || card.fightPotions.TryInvincible(card.fightPotions)) yield break;

            card.statusEffectsInit.OnDamageTakenByStatus?.Invoke(possibleDamage);
            if (card.cardInit.IsCardDead())
                yield return card.cardInit.cardFightAnimation.InitCardDeath();
        }
        private void DecreaseStatusApplied(CardFight card)
        {
            int i = 0;
            List<StatusEffect> newStatusEffects = card.statusEffectsInit.effectsApplied;
            while (true)
            {
                if (i >= newStatusEffects.Count)
                    break;

                newStatusEffects[i].duration--;
                if (newStatusEffects[i].duration <= 0)
                {
                    RemoveStatus(card, i);
                    continue;
                }
                i++;
            }
        }
        private int GetStatusDamage(CardFight card)
        {
            int totalDamage = 0;
            foreach (StatusEffect el in card.statusEffectsInit.effectsApplied)
            {
                if (el.isIgnoreDefense)
                    totalDamage += el.damage;
                else if (el.damage > card.cardInit.defense)
                    totalDamage += el.damage - card.cardInit.defense;
            }
            return totalDamage;
        }
        private void AddStatus(CardFight card)
        {
            StatusEffect currentEffect = CardFight.currentCard.cardInit.statusEffect;
            card.statusEffectsInit.effectsApplied.Add(new StatusEffect(
                currentEffect.effect,
                currentEffect.duration,
                currentEffect.damage,
                currentEffect.isIgnoreDefense,
                currentEffect.isStackingWithOther,
                currentEffect.isStackingWithCurrent));
        }
        private void RemoveStatus(CardFight card, int id)
        {
            card.statusEffectsInit.effectsApplied.RemoveAt(id);
            card.statusEffectsInit.OnStatusUpdate?.Invoke(card.cardInit);
        }
        private List<GameObject> FindAllCardsWithStatus(bool isEnemy) =>
        GameObject.FindGameObjectsWithTag("Card").Where(obj => obj.TryGetComponent(out CardFightInit cfi) &&
            cfi.hp > 0 && cfi.isEnemy == isEnemy && cfi.cardFight.statusEffectsInit.effectsApplied.Count > 0 && !cfi.IsCardDead()).ToList();

    }
}