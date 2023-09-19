using UnityEngine;
using System.Linq;
using Universal;

namespace GameMenu.House
{
    public class HouseSizeTextUpdater : TextUpdater
    {
        [SerializeField] private HousePanelInit housePanelInit;
        
        protected override void OnEnable()
        {
            housePanelInit.OnHouseSizeChanged += SetText;
        }
        protected override void OnDisable()
        {
            housePanelInit.OnHouseSizeChanged -= SetText;
        }
        private void SetText()
        {
            txt.text = $"{GameDataInit.data.cardsData.Where(x=>x.onHeal).Count()}/{GameDataInit.data.maxHouseSize}";
        }
    }
}