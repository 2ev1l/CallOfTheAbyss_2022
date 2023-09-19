using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;

namespace GameFight
{
    public class FightStorage : SingleSceneInstance
    {
        #region fields & properties
        public static FightStorage instance { get; private set; }
        [field: SerializeField] public Sprite backEnemyAliveSprite { get; private set; }
        [field: SerializeField] public Sprite backEnemyDeadSprite { get; private set; }
        [field: SerializeField] public Sprite[] backAllyAliveSprites { get; private set; }
        [field: SerializeField] public Sprite backAllyDeadSprite { get; private set; }
        [field: SerializeField] public Sprite[] layoutAllyAliveSprites { get; private set; }
        [field: SerializeField] public Sprite[] statusEffectSprites { get; private set; }
        [field: SerializeField] public List<Sprite> groundType { get; private set; }
        [field: SerializeField] public List<Sprite> waterType { get; private set; }
        [field: SerializeField] public List<Sprite> skyType { get; private set; }

        [field: SerializeField] public RectTransform enemySpawnPointRect { get; private set; }
        [field: SerializeField] public RectTransform allySpawnPointRect { get; private set; }
        [field: SerializeField] public Canvas deathAnimationPanel { get; private set; }
        [field: SerializeField] public List<GameObject> cardsAllyOnDeath { get; private set; } = new List<GameObject>();
        [field: SerializeField] public List<GameObject> cardsEnemyOnDeath { get; private set; } = new List<GameObject>();

        [field: SerializeField] public List<SoundTypeAudio<DefSoundType>> defSounds { get; private set; } = new List<SoundTypeAudio<DefSoundType>>();
        [field: SerializeField] public List<SoundTypeAudio<AtkSoundType>> atkSounds { get; private set; } = new List<SoundTypeAudio<AtkSoundType>>();

        public static int totalSilverGain;
        public static int totalGoldGain;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            totalSilverGain = 0;
            totalGoldGain = 0;
            instance = this;
            CheckInstances(GetType());
        }
        #endregion methods
    }
}