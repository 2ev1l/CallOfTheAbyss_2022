using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObjects/CardInfo")]
    public class CardInfoSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public int id { get; private set; }
        [field: SerializeField] public Sprite cardBG { get; private set; }
        [field: SerializeField] public AtkSoundType atkSoundType { get; private set; } = AtkSoundType.Punch01;
        [field: SerializeField] public DefSoundType defSoundType { get; private set; } = DefSoundType.Block01;
        [field: SerializeField] public AttackType attackType { get; private set; }
        [field: SerializeField] public Sprite attackSprite { get; private set; }
        [field: Header("Modules")]
        [field: SerializeField] public DamageHandle damageToDefense { get; private set; }
        [field: SerializeField] public DamageHandle damageToAttack { get; private set; }
        [field: SerializeField] public SpecialAbility specialAbility { get; private set; }
        [field: SerializeField] public StatusEffect statusEffect { get; private set; }

        [field: Header("Stats")]
        [field: SerializeField] public bool isDamageIgnoreDefense { get; private set; }
        [field: SerializeField] public CreatureType creatureType { get; private set; }
        [field: SerializeField] public int hp { get; private set; }
        [field: SerializeField] public int damage { get; private set; }
        [field: SerializeField] public int defense { get; private set; }
        [field: SerializeField] public int attackPriority { get; private set; }
        [field: SerializeField] public int defensePriority { get; private set; }

        [field: Space(4)]
        [field: SerializeField] public int rareTier { get; private set; }
        [field: SerializeField] public int silverPrice { get; private set; }
        [field: SerializeField] public int goldPrice { get; private set; }

        [field: Header("Upgrade")]
        [field: SerializeField] public int upgradeSilverPrice { get; private set; }
        [field: SerializeField] public int upgradeGoldPrice { get; private set; }
        [field: SerializeField] public int upgradeDuplicatePrice { get; private set; }
        [field: SerializeField] public int upgradedTier { get; private set; }
        [field: SerializeField] public int upgradedCardID { get; private set; }

        [field: Space(4)]
        [field: SerializeField] public int cardLocation { get; private set; }
        [field: SerializeField] public bool visibleInShop { get; private set; } = true;
        #endregion fields & properties
    }

    public enum DefSoundType { Block01 }
    public enum AtkSoundType { Punch01, Punch02, Punch03, Punch04, Punch05, Punch06, Animal01, Animal02, Animal03, Animal04, Animal05, Animal06, Animal07, Animal08, Animal09, Animal10, Cut01, Cut02, Cut03, Cut04, Hit01, Hit02, Hit03, Hit04, Hit05, Hit06, Hit07, Hit08, Hit09, Magic01, Magic02, Monster01, Monster02, Monster03, Monster04, Monster05, Monster06 }
    public enum AbilityType { None, Evasion, Heal, Darkness, AOE, Spikes, Crit, Vampire }
    public enum CreatureType { Ground, Underwater, Flying }
    public enum AttackType { Charge, Fast, SliceFast, SliceCharge, Heal };
    public enum StatusType { None, BleedingT0, BleedingT1, PoisonT0, PoisonT1, GhostBleedingT0, GhostBleedingT1, CurseT0, CurseT1 }

    [System.Serializable]
    public class SoundTypeAudio<T>
    {
        public T soundType;
        public AudioClip clip;
    }

    [System.Serializable]
    public class DamageHandle
    {
        [Min(0)]
        public int damage;
        public bool isIgnoreDefense;
    }

    [System.Serializable]
    public class SpecialAbility
    {
        public int value;
        public AbilityType type;
    }
}