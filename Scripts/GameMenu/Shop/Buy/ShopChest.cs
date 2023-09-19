using Universal;
using GameMenu.Inventory.Storages;

namespace GameMenu.Shop
{
    public class ShopChest : ShopObject
    {
        #region methods
        public override void Init()
        {
            base.Init();
            mainImage.sprite = InventoryChestStorage.instance.GetChestSprite(shopData.itemID);
        }
        public override void BuyItem()
        {
            if (!canBuy) return;
            base.BuyItem();
            GameDataInit.AddChest(shopData.itemID, false);
        }
        #endregion methods
    }
}