using UnityEngine;

namespace GameMenu.Shop
{
    public class BuyPanelStates : ShopPanelStates
    {
        [SerializeField] private ShopLoad shopLoad;
        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active) return;
            shopLoad.UpdateTab();
        }
    }
}