using UnityEngine;

namespace GameMenu.Shop
{
    public class EquipmentPanelState : BuyPanelStates
    {
        [SerializeField] private ShopLoad shopLoad2;
        [SerializeField] private ShopLoad shopLoad3;
        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active) return;
            shopLoad2.UpdateTab();
            shopLoad3.UpdateTab();
        }
    }
}