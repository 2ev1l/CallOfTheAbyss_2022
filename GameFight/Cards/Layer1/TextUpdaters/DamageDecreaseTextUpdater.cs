using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class DamageDecreaseTextUpdater : TextUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;
        
        protected override void OnEnable()
        {
            cardFightInit.OnDamagePreviewChanged += SetText;
            cardFightInit.cardFight.OnEnemyCardDeselect += ResetText;
        }
        protected override void OnDisable()
        {
            cardFightInit.OnDamagePreviewChanged -= SetText;
            cardFightInit.cardFight.OnEnemyCardDeselect -= ResetText;
        }
        private void SetText(int count, bool isHeal)
        {
            if (count <= 0)
            {
                ResetText();
                return;
            }
            SetDefaultText($"({(isHeal ? "+" : "-")}{count})");
            txt.color = isHeal ? new Color(141 / 255f, 1f, 160 / 255f) : new Color(1f, 141 / 255f, 152 / 255f);
        }
    }
}