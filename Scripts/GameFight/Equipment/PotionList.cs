using UnityEngine;
using Universal;

namespace GameFight.Equipment
{
    public class PotionList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            UpdateListDefault(GameDataInit.deskPotions, x => x.listPosition);
        }
        #endregion methods
    }
}