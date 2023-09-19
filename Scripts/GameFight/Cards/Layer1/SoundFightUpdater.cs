using UnityEngine;
using Data;
using Universal;

namespace GameFight.Card
{
    public sealed class SoundFightUpdater : DefaultUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;

        protected override void OnEnable()
        {
            cardFightInit.cardFight.OnAllyCardChoosed += OnClick;
            cardFightInit.cardFight.OnEnemyCardChoosed += OnClick;
            cardFightInit.cardFight.OnDamageTakenByEnemy += OnEnemyDamaged;
            cardFightInit.OnCardDeath += OnDeath;
            cardFightInit.cardFight.specialAbilities.OnSpecialAbilityTriggered += OnSpecialAbilityTriggered;
            cardFightInit.cardFight.OnHealToHPTaken += OnHeal;
            cardFightInit.cardFight.OnHealToAttackTaken += OnHeal;
            cardFightInit.cardFight.OnHealToDefenseTaken += OnHeal;
            cardFightInit.cardFight.fightPotions.OnPotionUsed += OnPotionUsed;
            cardFightInit.cardFight.fightPotions.OnPotionTriggered += OnPotionTriggered;
        }
        protected override void OnDisable()
        {
            cardFightInit.cardFight.OnAllyCardChoosed -= OnClick;
            cardFightInit.cardFight.OnEnemyCardChoosed -= OnClick;
            cardFightInit.cardFight.OnDamageTakenByEnemy -= OnEnemyDamaged;
            cardFightInit.OnCardDeath -= OnDeath;
            cardFightInit.cardFight.specialAbilities.OnSpecialAbilityTriggered -= OnSpecialAbilityTriggered;
            cardFightInit.cardFight.OnHealToHPTaken -= OnHeal;
            cardFightInit.cardFight.OnHealToAttackTaken -= OnHeal;
            cardFightInit.cardFight.OnHealToDefenseTaken -= OnHeal;
            cardFightInit.cardFight.fightPotions.OnPotionUsed -= OnPotionUsed;
            cardFightInit.cardFight.fightPotions.OnPotionTriggered -= OnPotionTriggered;
        }
        private void OnClick() => CursorSettings.instance.DoClickSound();
        private void OnDeath(bool isEnemy) => FightAudioInit.instance.PlayDeathClip();
        private void OnEnemyDamaged(int damage, AttackType attackType)
        {
            if (damage <= 0)
            {
                AudioClip clip = FightStorage.instance.defSounds.Find(x => x.soundType == cardFightInit.defSoundType).clip;
                AudioManager.PlayClip(clip, SoundType.Sound);
            }
            else
            {
                AudioClip clip = FightStorage.instance.atkSounds.Find(x => x.soundType == CardFight.currentCard.cardInit.atkSoundType).clip;
                AudioManager.PlayClip(clip, SoundType.Sound);
            }
        }
        private void OnHeal(int heal) => FightAudioInit.instance.PlayHealClip();
        private void OnSpecialAbilityTriggered(CardFightInit cardInit)
        {
            switch (cardInit.specialAbility.type)
            {
                case AbilityType.Evasion: FightAudioInit.instance.PlayEvasionClip(); break;
                case AbilityType.Darkness: FightAudioInit.instance.PlayDarknessClip(); break;
                case AbilityType.Heal: break;
                case AbilityType.AOE: break;
                case AbilityType.Spikes: break; 
                case AbilityType.Crit: break; 
                case AbilityType.Vampire: break; 
                default: throw new System.NotImplementedException();
            }
        }
        private void OnPotionUsed(CardFightInit cardInit, PotionEffect potionEffect) => FightAudioInit.instance.PlayPotionUsedClip();
        private void OnPotionTriggered(CardFightInit cardInit, PotionEffect potionEffect)
        {
            switch (potionEffect)
            {
                case PotionEffect.Invincible: FightAudioInit.instance.PlayInvincibleClip(); break;
                default: throw new System.NotImplementedException();
            }
        }
    }
}