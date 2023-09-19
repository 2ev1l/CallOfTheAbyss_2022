using System.Collections.Generic;
using UnityEngine;

namespace CutScene
{
    [CreateAssetMenu(fileName = "CutSceneScenarios", menuName = "ScriptableObjects/CutSceneScenarios")]
    public class CutSceneScenarios : ScriptableObject
    {
        #region fields
        [field: SerializeField] public List<CutSceneScenario> scenarios { get; private set; } = new List<CutSceneScenario>();
        #endregion fields
    }
    [System.Serializable]
    public class CutSceneScenario
    {
        public Sprite bgSprite;
        public int subtitleID;
    }
}