using UnityEngine;
using Data;
using System.Collections.Generic;
using System.Linq;

namespace Universal
{
    public class PrefabsData : MonoBehaviour
    {
        #region fields & properties
        public static PrefabsData instance { get; private set; }
        [field: SerializeField] public CardInfoSO[] cardPrefabs { get; private set; }
        [field: SerializeField] public PotionInfoSO[] potionPrefabs { get; private set; }
        [field: SerializeField] public List<PotionInfo> potionInfo { get; private set; }
        [field: SerializeField] public ArtifactInfoSO[] artifactPrefabs { get; private set; }
        [field: SerializeField] public List<ArtifactInfo> artifactInfo { get; private set; }
        #endregion fields & properties

        #region methods
        public void Init()
        {
            instance = this;
        }
        #endregion methods
        [ContextMenu("order card prefabs by id")]
        private void abc()
        {
            cardPrefabs = cardPrefabs.OrderBy(x => x.id).ToArray();
        }
        [ContextMenu("order potions by id")]
        private void abcf()
        {
            potionPrefabs = potionPrefabs.OrderBy(x => x.id).ToArray();
        }
        [ContextMenu("order artifacts by id")]
        private void abcfg()
        {
            artifactPrefabs = artifactPrefabs.OrderBy(x => x.id).ToArray();
        }
    }
    [System.Serializable]
    public class ArtifactInfo
    {
        public ArtifactEffect effect;
        public Sprite sprite;
        public int helpID;
    }
    [System.Serializable]
    public class PotionInfo
    {
        public PotionEffect effect;
        public Sprite sprite;
        public int helpID;
    }
    [System.Serializable]
    public class ShortPotionInfo
    {
        public PotionEffect effect;
        public int value;
        public ShortPotionInfo(PotionEffect effect, int value)
        {
            this.effect = effect;
            this.value = value;
        }
    }
}