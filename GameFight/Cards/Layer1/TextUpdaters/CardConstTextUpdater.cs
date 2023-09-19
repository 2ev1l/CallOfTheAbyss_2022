using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameFight.Card
{
    public class CardConstTextUpdater : DefaultUpdater
    {
        [SerializeField] CardFightInit cardFightInit;
        [SerializeField] private Text nameText;
        [SerializeField] private Text atkPriorityText;
        [SerializeField] private Text defPriorityText;
        protected override void OnEnable()
        {
            cardFightInit.UpdateCardUI += OnCardTextUpdate;
            cardFightInit.OnDefensePriorityUIChanged += UpdateDefPriority;
            cardFightInit.OnAttackPriorityUIChanged += UpdateAtkPriority;
        }
        protected override void OnDisable()
        {
            cardFightInit.UpdateCardUI -= OnCardTextUpdate;
            cardFightInit.OnDefensePriorityUIChanged -= UpdateDefPriority;
            cardFightInit.OnAttackPriorityUIChanged -= UpdateAtkPriority;
        }
        private void OnCardTextUpdate(bool isEnemy)
        {
            UpdateDefPriority();
            UpdateAtkPriority();
            if (!isEnemy) return;
            nameText.color = new Color(0.9f, 0f, 0.2f, 1f);
        }
        private void UpdateDefPriority() => FightAnimationInit.instance.UpdateIntCounterSmoothByText(defPriorityText, cardFightInit.defensePriority, 0.1f, false, Color.cyan, Color.white);
        private void UpdateAtkPriority() => FightAnimationInit.instance.UpdateIntCounterSmoothByText(atkPriorityText, cardFightInit.attackPriority, 0.2f, true, Color.cyan, Color.white);
        public void ResetTextColor()
        {
            atkPriorityText.color = Color.white;
            defPriorityText.color = Color.white;
        }
    }
}