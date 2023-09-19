using UnityEngine;
using Universal;

namespace GameMenu.TrainingCamp
{
    public class TrainingCampInit : SingleSceneInstance
    {
        #region fields & properties
        public static TrainingCampInit instance { get; private set; }

        [SerializeField] private ItemList trainingCampList;
        #endregion fields & properties

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        public void UpdateTrainingCampList() => trainingCampList.UpdateListData();
        public void AB() { }
        #endregion methods
    }
}