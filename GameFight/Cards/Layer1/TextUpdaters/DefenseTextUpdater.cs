using UnityEngine;
using Universal;

namespace GameFight.Card
{
    public class DefenseTextUpdater : TextUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;
        
        protected override void OnEnable()
        {
            cardFightInit.OnDefenseChanged += SetText;
        }
        protected override void OnDisable()
        {
            cardFightInit.OnDefenseChanged -= SetText;
        }
        private void SetText(int count, bool isFast)
        {
            if (isFast)
                SetDefaultText(count);
            else
                FightAnimationInit.instance.UpdateIntCounterSmoothByText(txt, count, 0.05f, textPosition == TextPosition.Before, Color.cyan, Color.white, true, cardFightInit, UpdateValueType.DEF);
        }
    }
}