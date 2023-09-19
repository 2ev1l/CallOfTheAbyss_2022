using UnityEngine;
using Universal;

namespace GameMenu.Inventory.Cards
{
    public class InventoryCardsParticles : SingleSceneInstance
    {
        #region fields & properties
        public static InventoryCardsParticles instance { get; private set; }

        [SerializeField] private ParticleSystem cardChooseParticles;
        [SerializeField] private ParticleSystem cardChooseAltParticles;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        public void SpawnChooseParticlesAt(Vector3 position) => CustomAnimation.BurstParticlesAt(position, cardChooseParticles);
        public void SpawnChooseAltParticlesAt(Vector3 position) => CustomAnimation.BurstParticlesAt(position, cardChooseAltParticles);
        #endregion methods
    }
}