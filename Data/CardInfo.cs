using UnityEngine;
using GameFight.Card;

namespace Data
{
    public class CardInfo : MonoBehaviour
    {
        #region fields & properties
        public int id { get; private set; }
        [field: SerializeField] public AtkSoundType atkSoundType { get; private set; }
        public DefSoundType defSoundType { get; private set; }
        public AttackType attackType { get; protected set; }
        public Sprite attackSprite { get; private set; }
        public bool isDamageIgnoreDefense { get; private set; }
        
        public DamageHandle damageToDefense { get; private set; }
        public DamageHandle damageToAttack { get; private set; }
        public SpecialAbility specialAbility { get; private set; }
        public StatusEffect statusEffect { get; private set; }
       
        public Sprite cardBG { get; private set; }
        public int hp { get; private set; }
        public int maxHP { get; private set; }
        public int damage { get; private set; }
        public int maxDamage { get; private set; }
        public int defense { get; private set; }
        public int maxDefense { get; private set; }
        public int attackPriority { get; private set; }
        public int defensePriority { get; private set; }
        public CreatureType creatureType { get; private set; }
       
        public int rareTier { get; private set; }
        public int silverPrice { get; private set; }
        public int goldPrice { get; private set; }
        #endregion fields & properties
        protected virtual void UnpackValuesToBase(CardInfoSO cardInfo)
        {
            id = cardInfo.id;
            atkSoundType = cardInfo.atkSoundType;
            attackType = cardInfo.attackType;
            attackSprite = cardInfo.attackSprite;
            isDamageIgnoreDefense = cardInfo.isDamageIgnoreDefense;
            damageToDefense = cardInfo.damageToDefense;
            damageToAttack = cardInfo.damageToAttack;
            specialAbility = cardInfo.specialAbility;
            StatusEffect status = cardInfo.statusEffect;
            statusEffect = new StatusEffect(status.effect, status.duration, status.damage, status.isIgnoreDefense, status.isStackingWithOther, status.isStackingWithCurrent);
            defSoundType = cardInfo.defSoundType;
            cardBG = cardInfo.cardBG;
            hp = cardInfo.hp;
            damage = cardInfo.damage;
            defense = cardInfo.defense;
            attackPriority = cardInfo.attackPriority;
            defensePriority = cardInfo.defensePriority;
            creatureType = cardInfo.creatureType;
            rareTier = cardInfo.rareTier;
            silverPrice = cardInfo.silverPrice;
            goldPrice = cardInfo.goldPrice;
        }
        protected virtual void SetMaxValues(int maxHP, int maxDamage, int maxDefense)
        {
            this.maxHP = maxHP;
            this.maxDamage = maxDamage;
            this.maxDefense = maxDefense;
        }
        protected virtual void SetHP(int amount) => hp = amount;
        protected virtual void SetDamage(int amount) => damage = amount;
        protected virtual void SetDefense(int amount) => defense = amount;
        protected virtual void SetDefensePriority(int amount) => defensePriority = amount;
        protected virtual void SetAttackPriority(int amount) => attackPriority = amount;
    }
    [System.Serializable]
    public class StatusEffect
    {
        public StatusType effect = StatusType.None;
        [Min(0)]
        public int duration = 0;
        [Min(0)]
        public int damage = 0;
        public bool isIgnoreDefense = false;
        public bool isStackingWithOther = false;
        public bool isStackingWithCurrent = false;
        public StatusEffect(StatusType effect, int duration, int damage, bool isIgnoreDefense, bool isStackingWithOther, bool isStackingWithCurrent)
        {
            this.effect = effect;
            this.duration = duration;
            this.damage = damage;
            this.isIgnoreDefense = isIgnoreDefense;
            this.isStackingWithOther = isStackingWithOther;
            this.isStackingWithCurrent = isStackingWithCurrent;
        }
    }
}
