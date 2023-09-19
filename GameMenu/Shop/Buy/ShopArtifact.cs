using GameMenu.Artifacts;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Shop
{
    public class ShopArtifact : ShopObject
    {
        #region fields
        [SerializeField] private ArtifactInit artifactInit;
        [SerializeField] private Text artifactText;
        #endregion fields

        #region methods
        public override void Init()
        {
            base.Init();
            artifactInit.UpdateValuesById(shopData.itemID);
        }
        public override void BuyItem()
        {
            if (!canBuy) return;
            base.BuyItem();
            GameDataInit.AddArtifact(shopData.itemID, false);
        }
        protected override void ActivateObject(bool f)
        {
            base.ActivateObject(f);
            artifactText.enabled = f;
        }
        #endregion methods
    }
}
