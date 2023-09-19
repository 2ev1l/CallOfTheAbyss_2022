using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ArtifactInfo", menuName = "ScriptableObjects/ArtifactInfo")]
    public class ArtifactInfoSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public int id { get; private set; }
        [field: SerializeField] public ArtifactEffect effect { get; private set; }
        [field: Space(5)]
        [field: SerializeField] public int silverPrice { get; private set; }
        [field: SerializeField] public int goldPrice { get; private set; }
        [field: SerializeField] public int artifactLocation { get; private set; }
        [field: SerializeField] public bool visibleInShop { get; private set; }
        #endregion fields & properties
    }
    public enum ArtifactEffect { 
        Hourglass, RottenBrain, AncientTeleport, WarpedHourglass, BrokenHourglass, 
        LiveCube, RuneStone, DarkStone, EnchantedStone, InvisibleFlower, UniverseInAJar, 
        GodsFlesh, RedCrystal, WhiteCrystal, VaryingCrystal, PocketSun }
}