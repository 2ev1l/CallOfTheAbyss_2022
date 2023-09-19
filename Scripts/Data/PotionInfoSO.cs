using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PotionInfo", menuName = "ScriptableObjects/PotionInfo")]
    public class PotionInfoSO : ScriptableObject
    {
        #region fields & properties
        [field: SerializeField] public int id { get; private set; }
        [field: SerializeField] public PotionEffect effect { get; private set; }
        [field: SerializeField] public int value { get; private set; }
        [field: Space(5)]
        [field: SerializeField] public int silverPrice { get; private set; }
        [field: SerializeField] public int goldPrice { get; private set; }
        [field: SerializeField] public int potionLocation { get; private set; }
        [field: SerializeField] public bool visibleInShop { get; private set; }
        #endregion fields & properties
    }
    public enum PotionEffect { None, Heal, Damage, Defense, Invincible, Weakness, Fragility, AntiDamage, AntiDefense, Strength, Confidence }
}