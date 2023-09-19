using Data;
using GameFight.Equipment;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Universal;
using static GameFight.Card.CardFight;

namespace GameFight.Card
{
    public class MaskFightUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] private CardFightInit cardFightInit;
        [SerializeField] private GameObject blackMask;
        [SerializeField] private Image blackMaskImage;
        [SerializeField] private ShowHelp blackMaskShowHelp;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            cardFightInit.cardFight.OnCardPriorityChange += ShowCardsByTurn;
            FightPotion.OnPotionChoosed += ShowCardsByPotion;
            FightPotion.OnPotionDeselect += delegate { ShowCardsByTurn(CardFightTurnInit.isEnemyTurn); };
        }
        protected override void OnDisable()
        {
            cardFightInit.cardFight.OnCardPriorityChange -= ShowCardsByTurn;
            FightPotion.OnPotionChoosed -= ShowCardsByPotion;
            FightPotion.OnPotionDeselect -= delegate { ShowCardsByTurn(CardFightTurnInit.isEnemyTurn); };
        }
        private void ShowCardsByDefensePriority(string panelName)
        {
            List<CardFightInit> fightInits = CardFight.GetComponents<CardFightInit>(GetChildCardsInParent(panelName));
            int maxDefPriority = CustomMath.FindMax(fightInits, x => x.defensePriority);
            foreach (CardFightInit cardInit in fightInits.Where(x => x.defensePriority < maxDefPriority))
                ChangeMaskState(MaskState.ActiveDefense, cardInit);
        }
        private void ShowCardsByAttackPriority(string panelName)
        {
            List<CardFightInit> fightInits = CardFight.GetComponents<CardFightInit>(GetChildCardsInParent(panelName));
            int maxAtkPriority = CustomMath.FindMax(fightInits, x => x.attackPriority, x => !IsCardUsed(x.cardFight));
            foreach (CardFightInit cardInit in fightInits.Where(x => x.attackPriority < maxAtkPriority))
                ChangeMaskState(MaskState.ActiveAttack, cardInit);
        }
        private void ShowUsedCards(string panelName)
        {
            foreach (GameObject el in GetChildCardsInParent(panelName))
            {
                CardFightInit cardInit = el.GetComponent<CardFightInit>();
                if (panelName.Equals(allyPanel) && allyCardsUsed.Contains(cardInit.cardFight))
                    ShowUsedCard(cardInit);
                else if (panelName.Equals(enemyPanel) && enemyCardsUsed.Contains(cardInit.cardFight))
                    ShowUsedCard(cardInit);
            }
        }
        private void ShowUsedCard(CardFightInit cardInit) => ChangeMaskState(MaskState.ActiveDefault, cardInit);
        private void ChangeMaskState(MaskState maskState, CardFightInit cardInit)
        {
            cardInit.canBeChoosed = maskState == MaskState.UnActive;
            cardInit.maskFightUpdater.blackMask.SetActive(!cardInit.canBeChoosed);
            switch (maskState)
            {
                case MaskState.UnActive:
                    cardInit.constTextUpdater.ResetTextColor();
                    break;
                case MaskState.ActiveDefault:
                    cardInit.maskFightUpdater.blackMaskImage.color = new Color(0f, 0f, 0f, 0.5f);
                    cardInit.maskFightUpdater.blackMaskShowHelp.id = 32;
                    break;
                case MaskState.ActiveDefense:
                    cardInit.maskFightUpdater.blackMaskImage.color = new Color(0.4f, 0.5f, 1f, 0.4f);
                    cardInit.maskFightUpdater.blackMaskShowHelp.id = 31;
                    cardInit.OnDefensePriorityUIChanged?.Invoke();
                    break;
                case MaskState.ActiveAttack:
                    cardInit.maskFightUpdater.blackMaskImage.color = new Color(0.9f, 0f, 0.4f, 0.4f);
                    cardInit.maskFightUpdater.blackMaskShowHelp.id = 30;
                    cardInit.OnAttackPriorityUIChanged?.Invoke();
                    break;
                default: throw new System.NotImplementedException();
            }
        }
        private void ShowAllCards()
        {
            ShowAllCards(enemyPanel);
            ShowAllCards(allyPanel);
        }
        private void ShowCardsByPotion(FightPotion fightPotion)
        {
            switch (fightPotion.potionInfo.effect)
            {
                case PotionEffect i when (int)i > 0 && (int)i <= 4:
                    ShowAllCards(allyPanel);
                    break;
                case PotionEffect i when (int)i == 5 || (int)i == 6 || (int)i == 9 || (int)i ==10:
                    ShowAllCards();
                    break;
                case PotionEffect i when (int)i >= 7 && (int)i <= 8:
                    ShowAllCards(enemyPanel);
                    break;
                default: throw new System.NotImplementedException();
            }
        }
        public void ShowAllCards(string panel)
        {
            foreach (GameObject el in GetChildCardsInParent(panel))
                ChangeMaskState(MaskState.UnActive, el.GetComponent<CardFightInit>());
        }
        public void ShowCardsByTurn(bool isEnemyTurn)
        {
            ShowAllCards();
            ShowCardsByAttackPriority(isEnemyTurn ? enemyPanel : allyPanel);
            ShowCardsByDefensePriority(isEnemyTurn ? allyPanel : enemyPanel);
            ShowUsedCards(isEnemyTurn ? enemyPanel : allyPanel);
        }
        #endregion methods
        private enum MaskState { ActiveDefault, ActiveDefense, ActiveAttack, UnActive };
    }
}