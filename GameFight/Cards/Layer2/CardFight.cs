using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Data;
using Universal;

namespace GameFight.Card
{
    public class CardFight : CursorChange, IPointerClickHandler
    {
        #region fields
        public UnityAction OnEnemyCardDeselect;
        public UnityAction OnEnemyCardChoosed;
        public UnityAction OnAllyCardChoosed;

        public UnityAction<int, AttackType> OnDamageTakenByEnemy;
        public UnityAction<int> OnDamageToDefenseTaken;
        public UnityAction<int> OnDamageToAttackTaken;
        public UnityAction OnEnemyAttackAnimation;
        public UnityAction<bool> OnCardPriorityChange;

        public UnityAction<int> OnHealToHPTaken;
        public UnityAction<int> OnHealToAttackTaken;
        public UnityAction<int> OnHealToDefenseTaken;

        [field: SerializeField] public CardFightInit cardInit { get; private set; } //current (got damage) card
        public static CardFight currentCard; //attacking (deal damage) card

        [SerializeField] private CardFightAttackAnimation attackAnimation;
        [SerializeField] private CardFightAnimationInit cardAnimation;

        [field: SerializeField] public CardFightStatusEffects statusEffectsInit { get; private set; }
        [field: SerializeField] public CardFightSpecialAbilities specialAbilities { get; private set; }
        [field: SerializeField] public CardFightPotions fightPotions { get; private set; }

        [SerializeField] private CardFightTurnInit turnInit;
        [SerializeField] private MaskFightUpdater maskUpdater;

        [HideInInspector] public bool isFlipping;
        public bool isScaling { get; private set; }
        public bool isSelected { get; private set; }

        public static readonly float defaultScale = 1f;
        public static readonly float plusScale = 0.1f;
        public static readonly string allyPanel = "AllyPanel";
        public static readonly string enemyPanel = "EnemyPanel";
        public static List<CardFight> enemyCardsUsed = new List<CardFight>();
        public static List<CardFight> allyCardsUsed = new List<CardFight>();
        #endregion fields

        #region methods

        #region mouse events
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || isFlipping || FightAnimationInit.isFightAnimation || fightPotions.TryUsePotion())
                return;
            if (CanDetectEnemy())
            {
                base.OnPointerExit(eventData);
                ClearSelectedAnimations();
                specialAbilities.TryAOE(currentCard.cardInit, cardInit, el => el.cardFight.OnEnemyCardDeselect?.Invoke());
                OnEnemyCardDeselect?.Invoke();
                OnEnemyCardChoosed?.Invoke();
                HitThisCard();
            }
            else if (!cardInit.isEnemy)
            {
                currentCard = currentCard != null ? currentCard : this;
                if (currentCard == this)
                {
                    OnAllyCardChoosed?.Invoke();
                    if (!isSelected)
                    {
                        cardInit.gameObject.transform.localScale = Vector3.one * (defaultScale + plusScale);
                        currentCard = this;
                        if (currentCard.cardInit.specialAbility.type == AbilityType.Heal)
                            maskUpdater.ShowAllCards(allyPanel);
                    }
                    else
                    {
                        if (currentCard.cardInit.specialAbility.type == AbilityType.Heal)
                            maskUpdater.ShowCardsByTurn(false);
                        currentCard = null;
                        cardInit.gameObject.transform.localScale = Vector3.one * defaultScale;
                        StartCoroutine(cardAnimation.CardScale(1f));
                    }
                    isScaling = isSelected;
                    isSelected = !isSelected;
                }
                else if (CanDetectAlly())
                {
                    base.OnPointerExit(eventData);
                    ClearSelectedAnimations();
                    OnEnemyCardDeselect?.Invoke();
                    OnEnemyCardChoosed?.Invoke();
                    HealThisCard();
                }
            }
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (isFlipping || FightAnimationInit.isFightAnimation || isScaling) return;
            if (currentCard == this || fightPotions.CanUsePotion())
            {
                base.OnPointerEnter(eventData);
                return;
            }

            if (!cardInit.isEnemy)
            {
                if (!isSelected && currentCard == null)
                    CardSelectablePreview(eventData);
                else if (CanDetectAlly())
                {
                    CardSelectablePreview(eventData);
                    ShowHealPreview();
                }
            }
            else //isEnemy
            {
                ShowDescription(true);
                if (CanDetectEnemy())
                {
                    CardSelectablePreview(eventData);
                    specialAbilities.TryAOE(currentCard.cardInit, cardInit, el => el.cardFight.ShowDamagePreview());
                    ShowDamagePreview();
                }
            }
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (isFlipping || FightAnimationInit.isFightAnimation || isSelected || fightPotions.CanUsePotion()) return;
            if (currentCard != null)
                specialAbilities.TryAOE(currentCard.cardInit, cardInit, el => el.cardFight.DeselectCard());
            DeselectCard();
        }
        #endregion mouse events

        private IEnumerator Start()
        {
            cardInit.back.SetActive(true);
            yield return CustomMath.WaitAFrame();
            OnCardPriorityChange?.Invoke(false);
        }
        public void ClearSelectedAnimations()
        {
            ShowDescription(false);
            currentCard.ShowDescription(false);
            isSelected = false;
            isScaling = false;
            currentCard.isSelected = false;
            currentCard.isScaling = false;
            currentCard.cardInit.gameObject.transform.localScale = Vector3.one * defaultScale;
            cardInit.gameObject.transform.localScale = Vector3.one * defaultScale;
            cardInit.gameObject.transform.SetAsLastSibling();
            currentCard.cardInit.gameObject.transform.SetAsLastSibling();
        }
        public void DeselectCard()
        {
            isScaling = false;
            ShowDescription(false);
            if (cardInit.isEnemy || CanDetectAlly())
                OnEnemyCardDeselect?.Invoke();
        }
        private void CardSelectablePreview(PointerEventData eventData)
        {
            isScaling = true;
            StartCoroutine(cardAnimation.CardScale(1f));
            ShowDescription(true);
            base.OnPointerEnter(eventData);
        }
        public void ShowDescription(bool show)
        {
            if (!cardInit.HasDescription()) return;
            cardInit.onSelect.SetActive(show);
        }

        private void InitHit()
        {
            FightAnimationInit.instance.OnChangeAnimationState?.Invoke(true);
            if (currentCard.cardInit.isEnemy)
                enemyCardsUsed.Add(currentCard);
            else
                allyCardsUsed.Add(currentCard);
        }
        public void HitThisCard()
        {
            InitHit();
            StartCoroutine(HitProgression(currentCard.cardInit.attackType));
        }
        public void HealThisCard()
        {
            InitHit();
            StartCoroutine(HitProgression(AttackType.Heal));
        }
        private IEnumerator HitProgression(AttackType changedAttackType)
        {
            GameObject attackingCard = currentCard.cardInit.gameObject;
            OnEnemyAttackAnimation?.Invoke();

            AttackType defaultAType = currentCard.cardInit.attackType;
            currentCard.cardInit.ChangeAttackType(changedAttackType);
            yield return attackAnimation.WaitForAttackAnimation(currentCard.cardInit, 0.5f);
            int damageGot = attackAnimation.lastDamageGot;
            currentCard.cardInit.ChangeAttackType(defaultAType);

            #region modifiers
            yield return TryVampire(damageGot);
            yield return TrySpikes(damageGot);
            yield return statusEffectsInit.MakeStatusEffectsDamage();
            if (TryFightEnd()) yield break;
            #endregion modifiers

            #region next turn
            statusEffectsInit.UpdateStatuses();
            yield return CheckPossibleAOEDeathes();
            if (TryFightEnd()) yield break;

            yield return turnInit.StartNextTurn();
            #endregion next turn
        }
        private IEnumerator CheckPossibleAOEDeathes()
        {
            currentCard.specialAbilities.TryAOEAll(currentCard.cardInit, cardInit, out List<CardFightInit> damagedCards);
            foreach (CardFightInit el in damagedCards)
                if (el.IsCardDead())
                    yield return el.cardFightAnimation.InitCardDeath();
        }
        public void GetHealToHP(int value) => OnHealToHPTaken?.Invoke(value);
        public void GetHealToDamage(int value) => OnHealToAttackTaken?.Invoke(value);
        public void GetHealToDefense(int value) => OnHealToDefenseTaken?.Invoke(value);
        public void GetHealToHPFromAbility()
        {
            specialAbilities.AddHeal();
            OnHealToHPTaken?.Invoke(GetHealDealt());
        }
        private IEnumerator TrySpikes(int damage)
        {
            if (!specialAbilities.TrySpikes(cardInit, out float spikeScale)) yield break;
            currentCard.TryGetDamageToHP(Mathf.RoundToInt(damage * (spikeScale / 100f)), AttackType.Fast);
            if (currentCard.cardInit.IsCardDead())
                yield return currentCard.cardInit.cardFightAnimation.InitCardDeath();
        }
        private IEnumerator TryVampire(int damage)
        {
            if (!specialAbilities.TryVampire(currentCard.cardInit)) yield break;
            if (currentCard.cardInit.hp < currentCard.cardInit.maxHP)
                currentCard.GetHealToHP(Mathf.RoundToInt(damage * (currentCard.cardInit.specialAbility.value / 100f)));
        }
        public void TryGetDamages(AttackType attackType)
        {
            specialAbilities.TryAOE(currentCard.cardInit, cardInit, el => el.cardFight.TryGetDamagesToStats(attackType));
            TryGetDamagesToStats(attackType);
        }
        private void TryGetDamagesToStats(AttackType attackType)
        {
            if (fightPotions.TryInvincible(fightPotions) || specialAbilities.TryEvasion(cardInit)) return;
            TryGetDamageToHP(attackType);
            GetDamageToDefense();
            GetDamageToAttack();
        }

        private void TryGetDamageToHP(AttackType attackType) => TryGetDamageToHP(GetDamageToHPDealt(), attackType);
        private void TryGetDamageToHP(int possibleDamage, AttackType attackType)
        {
            if (specialAbilities.TryDarkness(cardInit, possibleDamage))
            {
                possibleDamage = cardInit.hp - 1;
                if (possibleDamage == 0) return;
            }
            OnDamageTakenByEnemy?.Invoke(possibleDamage, attackType);
        }
        public void GetDamageToDefense(int value) => OnDamageToDefenseTaken?.Invoke(value);
        public void GetDamageToAttack(int value) => OnDamageToAttackTaken?.Invoke(value);
        private void GetDamageToDefense() => GetDamageToDefense(GetDamageToDefenseDealt());
        private void GetDamageToAttack() => GetDamageToAttack(GetDamageToAttackDealt());
        private int GetDamageToHPDealt()
        {
            CardFightInit attackingCard = currentCard.cardInit;
            int currentDamage = attackingCard.damage;
            if (IsCreatureTypeStronger()) currentDamage = Mathf.RoundToInt(currentDamage * 1.2f);
            if (specialAbilities.TryCrit(attackingCard, out float critScale)) currentDamage = Mathf.RoundToInt(currentDamage * critScale);
            int finalDamage = attackingCard.isDamageIgnoreDefense ? currentDamage : Mathf.Clamp(currentDamage - cardInit.defense, 0, currentDamage);
            return finalDamage;
        }
        private int GetDamageToDefenseDealt()
        {
            CardFightInit attackingCard = currentCard.cardInit;
            int currentDamage = attackingCard.damageToDefense.damage;
            int finalDamage = attackingCard.damageToDefense.isIgnoreDefense ? currentDamage : Mathf.Clamp(currentDamage - cardInit.defense, 0, currentDamage);
            return finalDamage;
        }
        private int GetDamageToAttackDealt()
        {
            CardFightInit attackingCard = currentCard.cardInit;
            int currentDamage = attackingCard.damageToAttack.damage;
            int finalDamage = attackingCard.damageToAttack.isIgnoreDefense ? currentDamage : Mathf.Clamp(currentDamage - cardInit.defense, 0, currentDamage);
            return finalDamage;
        }
        private int GetHealDealt()
        {
            int heal = currentCard.cardInit.specialAbility.value;
            if (heal + cardInit.hp > cardInit.maxHP)
                heal = Mathf.Max(cardInit.maxHP - cardInit.hp, 0);
            return heal;
        }
        private void ShowDamagePreview()
        {
            cardInit.OnHPPreviewChanged?.Invoke(GetDamageToHPDealt(), false);
            cardInit.OnDamagePreviewChanged?.Invoke(GetDamageToAttackDealt(), false);
            cardInit.OnDefensePreviewChanged?.Invoke(GetDamageToDefenseDealt(), false);
        }
        private void ShowHealPreview()
        {
            cardInit.OnHPPreviewChanged?.Invoke(GetHealDealt(), true);
        }

        #region bools
        private bool IsCreatureTypeStronger()
        => ((currentCard.cardInit.creatureType == CreatureType.Ground && cardInit.creatureType == CreatureType.Underwater) ||
            (currentCard.cardInit.creatureType == CreatureType.Underwater && cardInit.creatureType == CreatureType.Flying) ||
            (currentCard.cardInit.creatureType == CreatureType.Flying && cardInit.creatureType == CreatureType.Ground));
        private bool CanDetectEnemy() => cardInit.isEnemy && currentCard != null && currentCard.isSelected;
        private bool CanDetectAlly() => !cardInit.isEnemy && currentCard != null && currentCard != this && currentCard.isSelected &&
            currentCard.cardInit.specialAbility.type == AbilityType.Heal && specialAbilities.CanHeal(cardInit);
        public static bool IsCardUsed(CardFight card) => (allyCardsUsed.Contains(card) || enemyCardsUsed.Contains(card));
        private bool TryFightEnd()
        {
            List<GameObject> allyCards = GetChildCardsInParent(allyPanel, true);
            if (allyCards.Count == 0)
            {
                StageEndInit.instance.OnStageFailed();
                FightAnimationInit.instance.OnChangeAnimationState?.Invoke(false);
                return true;
            }
            else if (GetChildCardsInParent(enemyPanel, true).Count == 0)
            {
                if (CardFight.GetComponents<CardFightInit>(allyCards).FindIndex(x => !x.IsCardDead()) >= 0)
                    StageEndInit.instance.OnStageCompleted();
                else
                    StageEndInit.instance.OnStageFailed();

                FightAnimationInit.instance.OnChangeAnimationState?.Invoke(false);
                return true;
            }
            return false;
        }
        #endregion bools

        #region static
        public static List<GameObject> GetChildCardsInParent(string parentName, bool ignoreCardDeath = false)
        {
            List<GameObject> list = new List<GameObject>();
            Transform parent = GameObject.Find(parentName).transform;
            int childCount = parent.childCount;
            List<string> excludedNames = new List<string>() { "Position_death", "Position_spawn", "Position", "DeathPanelAnimation" };
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = parent.GetChild(i).gameObject;
                if (child.activeSelf && !excludedNames.Contains(child.name) && (!child.GetComponent<CardFightInit>().IsCardDead() || ignoreCardDeath))
                    list.Add(child);
            }
            return list;
        }
        public static List<T> GetComponents<T>(List<GameObject> objs)
        {
            List<T> components = new List<T>();
            foreach (GameObject el in objs)
                components.Add(el.GetComponent<T>());
            return components;
        }
        #endregion static

        #endregion methods
    }
}