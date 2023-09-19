using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class ParticlesFightUpdater : DefaultUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;

        protected override void OnEnable()
        {
            cardFightInit.cardFight.OnEnemyAttackAnimation += delegate { FightEffects.instance.OnEnemyChoosed(cardFightInit.transform); };
            cardFightInit.cardFight.specialAbilities.OnSpecialAbilityTriggered += FightEffects.instance.OnSpecialAbilityTriggered;
            cardFightInit.cardFight.fightPotions.OnPotionUsed += FightEffects.instance.OnPotionUsed;
            cardFightInit.cardFight.fightPotions.OnPotionTriggered += FightEffects.instance.OnPotionTriggered;
        }

        protected override void OnDisable()
        {
            cardFightInit.cardFight.OnEnemyAttackAnimation -= delegate { FightEffects.instance.OnEnemyChoosed(cardFightInit.transform); }; 
            cardFightInit.cardFight.specialAbilities.OnSpecialAbilityTriggered -= FightEffects.instance.OnSpecialAbilityTriggered;
            cardFightInit.cardFight.fightPotions.OnPotionUsed -= FightEffects.instance.OnPotionUsed;
            cardFightInit.cardFight.fightPotions.OnPotionTriggered -= FightEffects.instance.OnPotionTriggered;
        }
    }
}