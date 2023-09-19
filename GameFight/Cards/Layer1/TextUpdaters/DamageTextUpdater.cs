using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public sealed class DamageTextUpdater : TextUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;
        
        protected override void OnEnable()
        {
            cardFightInit.OnDamageChanged += SetText;
        }
        protected override void OnDisable()
        {
            cardFightInit.OnDamageChanged -= SetText;
        }
        private void SetText(int count, bool isFast)
        {
            if (isFast)
                SetDefaultText(count);
            else
                FightAnimationInit.instance.UpdateIntCounterSmoothByText(txt, count, 0.15f, textPosition == TextPosition.Before, Color.cyan, Color.white, true, cardFightInit, UpdateValueType.ATK);
        }
    }
}