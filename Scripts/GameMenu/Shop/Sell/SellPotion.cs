using Data;
using GameMenu.Potions;
using UnityEngine;
using Universal;

namespace GameMenu.Shop
{
    public class SellPotion : SellObject, ItemList.IListUpdater
    {
        #region fields
        [SerializeField] private PotionInit potionInit;
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;
        #endregion fields

        #region methods
        public override void Init()
        {
            potionInit.UpdateValues(listPosition);
            PotionInfoSO potionInfo = PrefabsData.instance.potionPrefabs[GameDataInit.data.potionsData[listPosition].id];
            priceGold = potionInfo.goldPrice;
            priceSilver = potionInfo.silverPrice;
            base.Init();
        }
        public override void SellItem()
        {
            base.SellItem();
            GameDataInit.RemovePotion(listPosition);
            ShopPanelInit.instance.UpdateShopPotionsData();
        }

        public void OnListUpdate(int param) => ChangeListPosition(param);

        #endregion methods
    }
}