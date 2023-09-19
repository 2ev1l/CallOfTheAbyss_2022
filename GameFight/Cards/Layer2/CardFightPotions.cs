using GameFight.Equipment;
using UnityEngine;
using Universal;
using Data;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GameFight.Card
{
    public class CardFightPotions : DefaultUpdater
    {
        #region fields
        [SerializeField] private CardFight cardFight;
        public UnityAction<CardFightInit, PotionEffect> OnPotionTriggered;
        public UnityAction<CardFightInit, PotionEffect> OnPotionUsed;
        public List<ShortPotionInfo> potionsEffects { get; private set; } = new List<ShortPotionInfo>();
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            FightPotion.OnPotionChoosed += OnPotionChoosed;
            FightPotion.OnPotionDeselect += OnPotionDeselect;
        }
        protected override void OnDisable()
        {
            FightPotion.OnPotionChoosed -= OnPotionChoosed;
            FightPotion.OnPotionDeselect -= OnPotionDeselect;
        }
        private void OnPotionChoosed(FightPotion choosedPotion)
        {
            TryDeselectCard();
            CardFightInit cardInit = cardFight.cardInit;
            int value = choosedPotion.potionInfo.value;
            switch (choosedPotion.potionInfo.effect)
            {
                case PotionEffect.Heal: InvokeHealOnAlly(cardInit.OnHPPreviewChanged, value); break;
                case PotionEffect.Defense: InvokeHealOnAlly(cardInit.OnDefensePreviewChanged, value); break;
                case PotionEffect.Damage: InvokeHealOnAlly(cardInit.OnDamagePreviewChanged, value); break;
                case PotionEffect.Invincible: break;
                case PotionEffect.Weakness: break;
                case PotionEffect.Fragility: break;
                case PotionEffect.AntiDamage: InvokeDamageOnAll(cardInit.OnDamagePreviewChanged, value); break;
                case PotionEffect.AntiDefense: InvokeDamageOnAll(cardInit.OnDefensePreviewChanged, value); break;
                case PotionEffect.Strength: break;
                case PotionEffect.Confidence: break;
                default: throw new System.NotImplementedException();
            }
        }
        private void OnPotionDeselect(FightPotion dechoosedPotion)
        {
            TryDeselectCard();
            CardFightInit cardInit = cardFight.cardInit;
            switch (dechoosedPotion.potionInfo.effect)
            {
                case PotionEffect.Heal: InvokeHealOnAlly(cardInit.OnHPPreviewChanged, -1); break;
                case PotionEffect.Defense: InvokeHealOnAlly(cardInit.OnDefensePreviewChanged, -1); break;
                case PotionEffect.Damage: InvokeHealOnAlly(cardInit.OnDamagePreviewChanged, -1); break;
                case PotionEffect.Invincible: break;
                case PotionEffect.Weakness: break;
                case PotionEffect.Fragility: break;
                case PotionEffect.AntiDamage: InvokeDamageOnAll(cardInit.OnDamagePreviewChanged, -1); break;
                case PotionEffect.AntiDefense: InvokeDamageOnAll(cardInit.OnDefensePreviewChanged, -1); break;
                case PotionEffect.Strength: break;
                case PotionEffect.Confidence: break;
                default: throw new System.NotImplementedException();
            }
            if (cardInit.isEnemy == CardFightTurnInit.isEnemyTurn)
                cardFight.statusEffectsInit.OnStatusUpdate?.Invoke(cardInit);
        }
        private void InvokeHealOnAlly(UnityAction<int, bool> action, int value)
        {
            if (!cardFight.cardInit.isEnemy)
                action?.Invoke(value, true);
        }
        private void InvokeDamageOnAll(UnityAction<int, bool> action, int value)
        {
            action?.Invoke(value, false);
        }
        private void TryDeselectCard()
        {
            if (cardFight.isSelected)
            {
                cardFight.DeselectCard();
                cardFight.ClearSelectedAnimations();
            }
        }

        public bool TryUsePotion()
        {
            FightPotion choosedPotion = FightPotion.choosedPotion;
            if (!CanUsePotion()) return false;
            int value = choosedPotion.potionInfo.value;
            switch (choosedPotion.potionInfo.effect)
            {
                case PotionEffect.Heal: cardFight.GetHealToHP(value); break;
                case PotionEffect.Defense: cardFight.GetHealToDefense(value); break;
                case PotionEffect.Damage: cardFight.GetHealToDamage(value); break;
                case PotionEffect.Invincible: potionsEffects.Add(choosedPotion.potionInfo); break;
                case PotionEffect.Weakness: cardFight.cardInit.SetAtkPriority(value); break;
                case PotionEffect.Fragility: cardFight.cardInit.SetDefPriority(value); break;
                case PotionEffect.AntiDamage: cardFight.GetDamageToAttack(value); break;
                case PotionEffect.AntiDefense: cardFight.GetDamageToDefense(value); break;
                case PotionEffect.Strength: cardFight.cardInit.SetAtkPriority(9); break;
                case PotionEffect.Confidence: cardFight.cardInit.SetDefPriority(9); break;
                default: throw new System.NotImplementedException();
            }
            OnPotionUsed?.Invoke(cardFight.cardInit, choosedPotion.potionInfo.effect);
            FightPotion.RemoveUsedPotion();
            return true;
        }
        public bool CanUsePotion()
        {
            if (!FightPotion.isPotionChoosed) return false;
            return FightPotion.choosedPotion.potionInfo.effect switch
            {
                PotionEffect i when (int)i <= 4 => !cardFight.cardInit.isEnemy,
                PotionEffect i when (int)i <= 10 => true,
                _ => throw new System.NotImplementedException(),
            };
        }

        public bool TryInvincible(CardFightPotions currentCardPotions) => TryTriggerPotionEffect(currentCardPotions, PotionEffect.Invincible);

        private bool TryTriggerPotionEffect(CardFightPotions currentCardPotions, PotionEffect potionEffect)
        {
            int index = currentCardPotions.potionsEffects.FindIndex(x => x.effect == potionEffect);
            if (index < 0) return false;
            currentCardPotions.potionsEffects[index].value -= 1;
            TriggerPotion(currentCardPotions.cardFight.cardInit, currentCardPotions.potionsEffects[index].effect);
            if (currentCardPotions.potionsEffects[index].value <= 0)
                currentCardPotions.potionsEffects.RemoveAt(index);
            return true;
        }
        private void TriggerPotion(CardFightInit cardFightInit, PotionEffect potionEffect) => OnPotionTriggered?.Invoke(cardFightInit, potionEffect);
        #endregion methods
    }
}