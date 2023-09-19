using UnityEngine;
using Universal;

namespace GameMenu.Shop
{
    public class ShopTimerTextUpdater : TextUpdater
    {
        [SerializeField] private ShopPanelInit shopPanelInit;
     
        protected override void OnEnable()
        {
            shopPanelInit.OnShopTimerChanged += SetText;
        }
        protected override void OnDisable()
        {
            shopPanelInit.OnShopTimerChanged -= SetText;
        }
        private void SetText(int remainingTime)
        {
            txt.text = ((int)(remainingTime / 60f)).ToString("00") + ":" + ((int)(remainingTime % 60f)).ToString("00");
        }
    }
}