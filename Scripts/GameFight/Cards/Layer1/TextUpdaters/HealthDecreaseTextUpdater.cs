using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class HealthDecreaseTextUpdater : TextUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;

        protected override void OnEnable()
        {
            cardFightInit.OnHPPreviewChanged += SetText;
            cardFightInit.cardFight.OnEnemyCardDeselect += ResetText;
        }
        protected override void OnDisable()
        {
            cardFightInit.OnHPPreviewChanged -= SetText;
            cardFightInit.cardFight.OnEnemyCardDeselect -= ResetText;
        }
        private void SetText(int count, bool isHeal)
        {
            if (count == -1)
            {
                ResetText();
                return;
            }

            if (!isHeal)
            {
                if (count >= cardFightInit.hp)
                {
                    SetDefaultText($"{ShowHelp.langData[36]}");
                    SetColor(isHeal);
                    return;
                }
                else if (count == 0)
                {
                    SetDefaultText($"{ShowHelp.langData[65]}");
                    txt.color = new Color(65 / 255f, 170 / 255f, 170 / 255f);
                    return;
                }
            }
            SetDefaultText($"({(isHeal ? "+" : "-")}{count})");
            SetColor(isHeal);
        }
        private void SetColor(bool isHeal) => txt.color = isHeal ? new Color(141 / 255f, 1f, 160 / 255f) : new Color(1f, 141 / 255f, 152 / 255f);
    }
}