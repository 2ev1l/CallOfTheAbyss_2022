using UnityEngine;
using Universal;

namespace GameMenu
{
    public sealed class GameMenuSoundUpdater : DefaultUpdater
    {
        [SerializeField] private AudioClip moneySound;

        protected override void OnEnable()
        {
            GameDataInit.instance.OnCoinsChanged += PlayCoinSound;
        }
        protected override void OnDisable()
        {
            GameDataInit.instance.OnCoinsChanged -= PlayCoinSound;
        }
        private void PlayCoinSound() => PlayCoinSound(true);
        public void PlayCoinSound(bool playSound)
        {
            if (!playSound) return;
            AudioManager.PlayClip(moneySound, SoundType.Sound);
        }
    }
}