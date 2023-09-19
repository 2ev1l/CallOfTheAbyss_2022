using Data;
using GameMenu.Artifacts;
using UnityEngine;
using Universal;

namespace GameMenu.Shop
{
    public class SellArtifact : SellObject, ItemList.IListUpdater
    {
        #region fields
        [SerializeField] private ArtifactInit artifactInit;
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;
        #endregion fields

        #region methods
        public override void Init()
        {
            artifactInit.UpdateValues(listPosition);
            ArtifactInfoSO artifactInfo = PrefabsData.instance.artifactPrefabs[GameDataInit.data.artifactsData[listPosition].id];
            priceGold = artifactInfo.goldPrice;
            priceSilver = artifactInfo.silverPrice;
            base.Init();
        }
        public override void SellItem()
        {
            base.SellItem();
            GameDataInit.RemoveArtifact(listPosition);
            ShopPanelInit.instance.UpdateShopArtifactsData();
        }

        public void OnListUpdate(int param) => ChangeListPosition(param);

        #endregion methods
    }
}