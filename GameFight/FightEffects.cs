using UnityEngine;
using GameFight.Card;
using Universal;
using Data;

namespace GameFight
{
    public class FightEffects : SingleSceneInstance
    {
        #region fields & properties
        public static FightEffects instance { get; private set; }

        [SerializeField] private ParticleSystem singleBloodParticles;
        [SerializeField] private ParticleSystem singleDefenseParticles;
        [SerializeField] private ParticleSystem enemyChooseParticles;
        [SerializeField] private ParticleSystem singleEvasionParticles;
        [SerializeField] private ParticleSystem singleDarknessParticles;
        [SerializeField] private ParticleSystem singleHealParticles;
        [SerializeField] private ParticleSystem potionParticles;
        [SerializeField] private ParticleSystem invincibleParticles;
        #endregion fields & properties

        #region methods
        public void DoEffect(Transform toObject, AttackType attackType, int damage)
        {
            if (damage > 0 || attackType == AttackType.Heal)
            {
                var emission = singleBloodParticles.emission;
                var defaultBurst = new ParticleSystem.Burst(0, 3, 4, 1, 1f);
                short addedParticles = System.Convert.ToInt16(Mathf.Clamp((int)(damage * 2f), 0, 100));
                defaultBurst.minCount += addedParticles;
                defaultBurst.maxCount += addedParticles;
                emission.SetBurst(0, defaultBurst);
                switch (attackType)
                {
                    case AttackType aType when (int)aType <= 3: 
                        CustomAnimation.BurstParticlesAt(toObject.position, singleBloodParticles);
                        break;
                    case AttackType.Heal:
                        CustomAnimation.BurstParticlesAt(toObject.position, singleHealParticles);
                        break;
                    default: throw new System.NotImplementedException();
                }
            }
            else
            {
                CustomAnimation.BurstParticlesAt(toObject.position, singleDefenseParticles);
            }
        }
        public void OnEnemyChoosed(Transform card) => CustomAnimation.BurstParticlesAt(card.transform.position, enemyChooseParticles);
        public void OnSpecialAbilityTriggered(CardFightInit cardInit)
        {
            switch (cardInit.specialAbility.type)
            {
                case AbilityType.Evasion: OnEvasion(cardInit.transform); break;
                case AbilityType.Darkness: OnDarkness(cardInit.transform); break;
                case AbilityType.Spikes: break;
                case AbilityType.Crit: break;
            }
        }
        public void OnPotionUsed(CardFightInit cardInit, PotionEffect potionEffect) => OnPotion(cardInit.transform);
        public void OnPotionTriggered(CardFightInit cardInit, PotionEffect potionEffect)
        {
            switch (potionEffect)
            {
                case PotionEffect.Invincible:
                    OnInvincible(cardInit.transform);
                    break;
                default: throw new System.NotImplementedException();
            }

        }
        private void OnEvasion(Transform card) => CustomAnimation.BurstParticlesAt(card.position, singleEvasionParticles);
        private void OnDarkness(Transform card) => CustomAnimation.BurstParticlesAt(card.position, singleDarknessParticles);

        private void OnPotion(Transform card) => CustomAnimation.BurstParticlesAt(card.position, potionParticles);
        private void OnInvincible(Transform card) => CustomAnimation.BurstParticlesAt(card.position, invincibleParticles);

        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        #endregion methods
    }
}