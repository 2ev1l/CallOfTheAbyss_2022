using GameMenu.Potions;
using UnityEngine;
using Universal;
using UnityEngine.UI;

namespace GameMenu.Inventory.Other
{
    public class InventoryPotionUI : PotionUI
    {
        #region fields
        [SerializeField] private Text txt;
        #endregion fields

        #region methods
        protected override void UpdateValues()
        {
            PotionInfo potionInfo = GetPotionInfo();
            mainImage.sprite = potionInfo.sprite;
            txt.text = GetPotionInfoText(potionInfo);
        }
        #endregion methods
    }
}