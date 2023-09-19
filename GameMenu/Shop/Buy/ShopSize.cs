using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public sealed class ShopSize : ShopObject
    {
        #region fields
        [SerializeField] private Text itemText;
        #endregion fields

        #region methods
        public override void Init()
        {
            shopData ??= new ShopData();
            if (shopData.owned || shopData.itemID < 0)
            {
                ActivateObject(false);
                return;
            }
            base.Init();
            LanguageLoad ll = itemText.GetComponent<LanguageLoad>();
            int newID = (shopData.sizeType) switch
            {
                SizeType.Desk => 18,
                SizeType.Artifact => 20,
                SizeType.Inventory => 17,
                SizeType.Hand => 19,
                SizeType.House => 21,
                SizeType.Potion => 66,
                _ => throw new System.NotImplementedException()
            };
            ll.ChangeID(newID);
            int currentLevel = SizesData.GetSizeValueByType(shopData.sizeType);
            itemText.text += $": {currentLevel} => {currentLevel + shopData.addToSize}";
        }
        protected override void ActivateObject(bool f)
        {
            base.ActivateObject(f);
            itemText.enabled = f;
        }
        public override void BuyItem()
        {
            if (!canBuy) return;
            base.BuyItem();
            int addToSize = shopData.addToSize;
            switch (shopData.sizeType)
            {
                case SizeType.Desk: GameDataInit.data.maxDeskSize += addToSize; break;
                case SizeType.Artifact: GameDataInit.data.maxArtifactSize += addToSize; break;
                case SizeType.Inventory: GameDataInit.data.maxInventorySize += addToSize; break;
                case SizeType.Hand: GameDataInit.data.maxHandSize += addToSize; break;
                case SizeType.House: GameDataInit.data.maxHouseSize += addToSize; break;
                case SizeType.Potion: GameDataInit.data.maxPotionSize += addToSize; break;
                default: throw new System.NotImplementedException();
            }
            SizesData.instance.CheckAchievement(shopData.sizeType);
        }
        #endregion methods
    }
}