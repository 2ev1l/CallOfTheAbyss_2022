using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;
using GameMenu.Inventory.Storages;

namespace GameMenu.Shop
{
    public sealed class SellChest : SellObject, ItemList.IListUpdater
    {
        #region fields
        [SerializeField] private Image mainImage;
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;
        #endregion fields

        #region methods
        public override void Init()
        {
            ChestInfo chestInfo = InventoryChestStorage.instance.chestPrefabs[listPosition];
            mainImage.sprite = InventoryChestStorage.instance.GetChestSprite(chestInfo);
            priceGold = chestInfo.goldPrice;
            priceSilver = chestInfo.silverPrice;
            base.Init();
        }
        public override void SellItem()
        {
            base.SellItem();
            GameDataInit.RemoveChest(listPosition);
            ShopPanelInit.instance.UpdateShopChestsData();
        }
        public void OnListUpdate(int param) => ChangeListPosition(param);
        #endregion methods
    }
}