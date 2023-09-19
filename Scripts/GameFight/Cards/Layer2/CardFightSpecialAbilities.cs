using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Universal;
using static GameFight.Card.CardFight;

namespace GameFight.Card
{
    public class CardFightSpecialAbilities : MonoBehaviour
    {
        #region fields
        public UnityAction<CardFightInit> OnSpecialAbilityTriggered;

        private int darknessUsed = 0;
        private int healUsed = 0;
        #endregion fields

        #region methods
        public bool TryEvasion(CardFightInit currentCard)
        {
            bool isValid = currentCard.specialAbility.type == AbilityType.Evasion && CustomMath.GetRandomChance(currentCard.specialAbility.value);
            InvokeAbilityIf(isValid, currentCard);
            return isValid;
        }
        public bool TryDarkness(CardFightInit currentCard, int damage)
        {
            if (currentCard.specialAbility.type != AbilityType.Darkness || currentCard.hp - damage > 0) return false;

            darknessUsed++;
            bool isValid = darknessUsed <= currentCard.specialAbility.value;
            InvokeAbilityIf(isValid, currentCard);
            return isValid;
        }
        public bool TryAOE(CardFightInit attackingCard, CardFightInit enemyCard, Action<CardFightInit> doWithCards)
        {
            bool isValid = TryAOE(attackingCard, enemyCard, out List<CardFightInit> damagedCards);
            if (!isValid) return false;
            foreach (CardFightInit el in damagedCards)
                doWithCards.Invoke(el);
            return true;
        }
        public bool TryAOE(CardFightInit attackingCard, CardFightInit enemyCard, out List<CardFightInit> damagedCards)
        {
            damagedCards = new List<CardFightInit>();
            bool isValid = TryAOEAll(attackingCard, enemyCard, out damagedCards);
            if (!isValid) return false;
            damagedCards = damagedCards.Where(card => card != enemyCard).ToList();
            return true;
        }
        public bool TryAOEAll(CardFightInit attackingCard, CardFightInit enemyCard, out List<CardFightInit> allDamagedCards)
        {
            allDamagedCards = new List<CardFightInit>() { enemyCard };
            if (attackingCard.specialAbility.type != AbilityType.AOE) return false;
            allDamagedCards = CardFight.GetComponents<CardFightInit>(GetChildCardsInParent(enemyCard.isEnemy ? enemyPanel : allyPanel, true));
            int range = attackingCard.specialAbility.value;
            int currentPosition = enemyCard.fightPosition;
            allDamagedCards = allDamagedCards.Where(card => Mathf.Abs(card.fightPosition - currentPosition) <= range).ToList();
            InvokeAbilityIf(true, attackingCard);
            return true;
        }
        public bool CanHeal(CardFightInit cardToHeal)
        {
            if (cardToHeal.cardFight.specialAbilities.healUsed >= 1) return false;
            return true;
        }
        public void AddHeal() => healUsed++;
        public bool TrySpikes(CardFightInit currentCard, out float spikeScale)
        {
            bool isValid = currentCard.specialAbility.type == AbilityType.Spikes;
            InvokeAbilityIf(isValid, currentCard);
            spikeScale = currentCard.specialAbility.value;
            return isValid;
        }

        public bool TryCrit(CardFightInit attackingCard, out float critScale)
        {
            bool isValid = attackingCard.specialAbility.type == AbilityType.Crit && CustomMath.GetRandomChance(attackingCard.specialAbility.value);
            critScale = 1.5f;
            InvokeAbilityIf(isValid, attackingCard);
            return isValid;
        }
        public bool TryVampire(CardFightInit attackingCard)
        {
            bool isValid = attackingCard.specialAbility.type == AbilityType.Vampire;
            InvokeAbilityIf(isValid, attackingCard);
            return isValid;
        }
        private void InvokeAbilityIf(bool isInvoke, CardFightInit cardFightInit)
        {
            if (!isInvoke) return;
            OnSpecialAbilityTriggered?.Invoke(cardFightInit);
        }
        #endregion methods
    }
}