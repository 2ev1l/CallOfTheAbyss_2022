using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.House
{
    public class HouseEventTextUpdater : DefaultUpdater
    {
        [SerializeField] private Text eventText;
        protected override void OnEnable()
        {
            HousePanelInit.instance.OnHouseUpdate += SetText;
            HousePanelInit.instance.OnHouseSizeChanged += SetText;
        }
        protected override void OnDisable()
        {
            HousePanelInit.instance.OnHouseUpdate -= SetText;
            HousePanelInit.instance.OnHouseSizeChanged -= SetText;
        }
        private void SetText()
        {
            eventText.enabled = GameDataInit.data.cardsData.FindIndex(x => x.onHeal) > -1;
        }
    }
}
