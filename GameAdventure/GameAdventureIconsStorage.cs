using UnityEngine;
using Universal;

namespace GameAdventure
{
    public class GameAdventureIconsStorage : Storage
    {
        public static GameAdventureIconsStorage instance { get; private set; }
        [field: SerializeField] public Sprite iconCurrent { get; private set; }
        [field: SerializeField] public Sprite iconRespawned { get; private set; }
        [field: SerializeField] public Sprite iconRandom { get; private set; }
        [field: SerializeField] public Sprite iconDefeated { get; private set; }
        [field: SerializeField] public Sprite iconDefeatedBoss { get; private set; }
        [field: SerializeField] public Sprite iconFight { get; private set; }
        [field: SerializeField] public Sprite iconFightBoss { get; private set; }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
    }
}