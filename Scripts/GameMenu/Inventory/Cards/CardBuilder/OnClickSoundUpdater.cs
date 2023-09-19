using UnityEngine;
using Universal;

namespace GameMenu.Inventory.Cards.Builder
{
    public sealed class OnClickSoundUpdater : DefaultUpdater
    {
        [SerializeField] CardMenuInit cardMenuInit;
        
        protected override void OnEnable()
        {
            cardMenuInit.OnCardClick += PlaySound;
        }
        protected override void OnDisable()
        {
            cardMenuInit.OnCardClick -= PlaySound;
        }
        private void PlaySound() => CursorSettings.instance.DoClickSound();

    }
}