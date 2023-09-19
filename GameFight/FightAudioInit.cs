using Data;
using GameFight.Card;
using UnityEngine;
using Universal;

namespace GameFight
{
    public class FightAudioInit : SingleSceneInstance
    {
        #region fields & properties
        public static FightAudioInit instance { get; private set; }
        [SerializeField] private AudioClip defeatedSound;
        [SerializeField] private AudioClip completedSound;
        [SerializeField] private AudioClip spawnedCardSound;
        [SerializeField] private AudioClip deathCardSound;
        [SerializeField] private AudioClip evasionSound;
        [SerializeField] private AudioClip darknessSound;
        [SerializeField] private AudioClip healSound;
        [SerializeField] private AudioClip potionSound;
        [SerializeField] private AudioClip invincibleSound;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void OnEnable()
        {
            StageEndInit.instance.OnStageEnded += PlaySoundAfterBattle;
            FightCardSpawner.instance.OnCardSpawned += PlaySpawnedClip;
        }
        private void OnDisable()
        {
            StageEndInit.instance.OnStageEnded -= PlaySoundAfterBattle;
            FightCardSpawner.instance.OnCardSpawned -= PlaySpawnedClip;
        }
        public void PlayDeathClip() => AudioManager.PlayClip(deathCardSound, SoundType.Sound);
        public void PlayEvasionClip() => AudioManager.PlayClip(evasionSound, SoundType.Sound);
        public void PlayDarknessClip() => AudioManager.PlayClip(darknessSound, SoundType.Sound);
        public void PlayHealClip() => AudioManager.PlayClip(healSound, SoundType.Sound);

        public void PlayInvincibleClip() => AudioManager.PlayClip(invincibleSound, SoundType.Sound);
        public void PlayPotionUsedClip() => AudioManager.PlayClip(potionSound, SoundType.Sound);

        private void PlaySpawnedClip() => AudioManager.PlayClip(spawnedCardSound, SoundType.Sound);
        private void PlaySoundAfterBattle(bool isCompleted) => AudioManager.PlayClip(isCompleted ? completedSound : defeatedSound, SoundType.Music);
        #endregion methods
    }
}