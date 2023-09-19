using UnityEngine;
using UnityEngine.Events;
using Data;
using Universal;

namespace GameFight.Card
{
    public class CardFightInit : CardInfo
    {
        #region fields & properties
        public static string[] description;

        public UnityAction<int, bool> OnHPChanged;
        public UnityAction<int, bool> OnDamageChanged;
        public UnityAction<int, bool> OnDefenseChanged;

        public UnityAction<int, bool> OnHPPreviewChanged;
        public UnityAction<int, bool> OnDamagePreviewChanged;
        public UnityAction<int, bool> OnDefensePreviewChanged;

        public UnityAction<bool> UpdateCardUI;
        public UnityAction<bool> OnCardDeath;
        public UnityAction<bool> OnCardSpawn;

        public UnityAction OnDefensePriorityUIChanged;
        public UnityAction OnAttackPriorityUIChanged;

        [field: SerializeField] public CardConstTextUpdater constTextUpdater { get; private set; }
        [field: SerializeField] public CardFightAnimationInit cardFightAnimation { get; private set; }
        [field: SerializeField] public MaskFightUpdater maskFightUpdater { get; private set; }
        [field: SerializeField] public GameObject onSelect { get; private set; }
        [field: SerializeField] public GameObject back { get; private set; }
        [field: SerializeField] public GameObject trail { get; private set; }
        [field: SerializeField] public CardFight cardFight { get; private set; }

        [HideInInspector] public bool canBeChoosed = true;
        [HideInInspector] public bool isEnemy = false;
        [HideInInspector] public int fightPosition;
        [HideInInspector] public int absolutePosition;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            UpdateCardUI?.Invoke(isEnemy);
        }
        private void OnEnable()
        {
            cardFight.OnDamageTakenByEnemy += OnDamageTakeToHP;
            cardFight.statusEffectsInit.OnDamageTakenByStatus += OnDamageTakeToHP;
            cardFight.OnDamageToDefenseTaken += OnDamageTakeToDefense;
            cardFight.OnDamageToAttackTaken += OnDamageTakeToAttack;
            cardFight.OnHealToHPTaken += OnHealToHPTake;
            cardFight.OnHealToAttackTaken += OnHealToDamageTake;
            cardFight.OnHealToDefenseTaken += OnHealToDefenseTake;
        }
        private void OnDisable()
        {
            cardFight.OnDamageTakenByEnemy -= OnDamageTakeToHP;
            cardFight.statusEffectsInit.OnDamageTakenByStatus -= OnDamageTakeToHP;
            cardFight.OnDamageToDefenseTaken -= OnDamageTakeToDefense;
            cardFight.OnDamageToAttackTaken -= OnDamageTakeToAttack;
            cardFight.OnHealToHPTaken -= OnHealToHPTake;
            cardFight.OnHealToAttackTaken -= OnHealToDamageTake;
            cardFight.OnHealToDefenseTaken -= OnHealToDefenseTake;
        }

        public void Init(CardInfoSO cardInfo, CardData spawnedCardData = null)
        {
            unpackValuesToBase(cardInfo);
            isEnemy = spawnedCardData == null;
            if (!isEnemy)
            {
                SetHP(Mathf.RoundToInt(spawnedCardData.hp * FightCardSpawner.instance.hpScale), true);
                SetDamage(Mathf.RoundToInt(spawnedCardData.damage * FightCardSpawner.instance.dmgScale), true);
                SetDefense(Mathf.RoundToInt(spawnedCardData.defense * FightCardSpawner.instance.defScale), true);
            }
            else
            {
                if (GameDataInit.data.reachedLocation >= 5)
                {
                    SetHP(Mathf.RoundToInt(hp * 1.05f), true);
                    SetDamage(Mathf.RoundToInt(damage * 1.1f), true);
                    SetDefense(Mathf.RoundToInt(defense * 1.1f), true);
                }
                if (GameDataInit.data.difficulty == Difficulty.Hard)
                {
                    SetHP(Mathf.RoundToInt(hp * 1.2f), true);
                    SetDamage(Mathf.RoundToInt(damage * 1.2f), true);
                    SetDefense(Mathf.RoundToInt(defense * 1.2f), true);
                }
            }
            setMaxValues(spawnedCardData);
            OnCardSpawn?.Invoke(isEnemy);
        }
        private void setMaxValues(CardData cardData)
        {
            int maxHP = isEnemy ? hp : cardData.maxHP;
            int maxDamage = isEnemy ? damage : cardData.maxDamage;
            int maxDefense = isEnemy ? defense : cardData.maxDefense;
            SetMaxValues(maxHP, maxDamage, maxDefense);
        }
        private void OnHealToHPTake(int heal)
        {
            if (heal > 0)
                SetHP(Mathf.Max(hp + heal, 0), true);
        }
        private void OnHealToDamageTake(int heal)
        {
            if (heal > 0)
                SetDamage(Mathf.Max(damage + heal, 0), true);
        }
        private void OnHealToDefenseTake(int heal)
        {
            if (heal > 0)
                SetDefense(Mathf.Max(defense + heal, 0), true);
        }
        private void OnDamageTakeToHP(int damage, AttackType attackType) => OnDamageTakeToHP(damage);
        private void OnDamageTakeToHP(int damage)
        {
            if (damage > 0)
                SetHP(Mathf.Max(hp - damage, 0), false);
        }
        private void OnDamageTakeToDefense(int damage)
        {
            if (damage > 0)
                SetDefense(Mathf.Max(defense - damage, 0), false);
        }
        private void OnDamageTakeToAttack(int damage)
        {
            if (damage > 0)
                SetDamage(Mathf.Max(this.damage - damage, 0), false);
        }
        private void unpackValuesToBase(CardInfoSO cardInfo)
        {
            UnpackValuesToBase(cardInfo);
            OnHPChanged?.Invoke(hp, true);
            OnDamageChanged?.Invoke(damage, true);
            OnDefenseChanged?.Invoke(defense, true);
        }
        public void SetHP(int amount, bool isFastUpdate)
        {
            SetHP(amount);
            OnHPChanged?.Invoke(hp, isFastUpdate);
        }
        public void SetDamage(int amount, bool isFastUpdate)
        {
            SetDamage(amount);
            OnDamageChanged?.Invoke(damage, isFastUpdate);
        }
        public void SetDefense(int amount, bool isFastUpdate)
        {
            SetDefense(amount);
            OnDefenseChanged?.Invoke(defense, isFastUpdate);
        }
        public void ChangeAttackType(AttackType attackType) => this.attackType = attackType;
        public void SetDefPriority(int amount)
        {
            base.SetDefensePriority(Mathf.Max(amount, 0));
            cardFight.OnCardPriorityChange?.Invoke(CardFightTurnInit.isEnemyTurn);
            UpdateCardUI?.Invoke(isEnemy);
        }
        public void SetAtkPriority(int amount)
        {
            base.SetAttackPriority(Mathf.Max(amount, 0));
            cardFight.OnCardPriorityChange?.Invoke(CardFightTurnInit.isEnemyTurn);
            UpdateCardUI?.Invoke(isEnemy);
        }
        public bool HasDescription() => (description[id] != "" && description[id] != " ");
        public bool IsCardDead() => hp <= 0;
        #endregion methods
    }
    public enum UpdateValueType { HP, ATK, DEF }
}