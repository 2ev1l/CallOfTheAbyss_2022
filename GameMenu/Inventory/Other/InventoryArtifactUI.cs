using GameMenu.Artifacts;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Inventory.Other
{
    public class InventoryArtifactUI : ArtifactUI
    {
        #region fields
        [SerializeField] private Text txt;
        #endregion fields

        #region methods
        protected override void UpdateValues()
        {
            ArtifactInfo artifactInfo = GetArtifactInfo();
            mainImage.sprite = artifactInfo.sprite;
            txt.text = GetArtifactInfoText();
        }
        #endregion methods
    }
}