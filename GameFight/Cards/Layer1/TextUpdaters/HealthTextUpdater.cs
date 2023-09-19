using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public sealed class HealthTextUpdater : TextUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;
        
        protected override void OnEnable()
        {
            cardFightInit.OnHPChanged += SetText;
        }
        protected override void OnDisable()
        {
            cardFightInit.OnHPChanged -= SetText;
        }
        private void SetText(int count, bool isFast)
        {
            if (isFast)
                SetDefaultText(count);
            else
                FightAnimationInit.instance.UpdateIntCounterSmoothByText(txt, count, 0f, textPosition == TextPosition.Before, new Color(0.8f, 0f, 0.7f, 1f), Color.red, true, cardFightInit, UpdateValueType.HP);
        }
    }
}