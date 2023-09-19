using UnityEngine;
using Universal;

namespace GameFight.Equipment
{
    public class ArtifactList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(GameDataInit.deskArtifacts, x => x.listPosition);
        }
        #endregion methods
    }
}