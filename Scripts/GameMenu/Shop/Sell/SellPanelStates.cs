using UnityEngine;
using Universal;

namespace GameMenu.Shop
{
    public class SellPanelStates : ShopPanelStates
    {
        [SerializeField] private ItemList itemList;
        public override string stateNameNormalized => gameObject.name.Remove(gameObject.name.Length - 4);
        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active) return;
            if (itemList != null)
                itemList.UpdateListData();
        }
    }
}