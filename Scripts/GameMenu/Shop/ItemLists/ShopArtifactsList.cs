using Data;
using System.Collections.Generic;
using System.Linq;
using Universal;

namespace GameMenu.Shop
{
    public class ShopArtifactsList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<ArtifactData> artifactsData = GameDataInit.data.artifactsData.Where(x => !x.onDesk).ToList();
            UpdateListDefault(artifactsData, x => x.listPosition);
        }
        #endregion methods
    }
}
