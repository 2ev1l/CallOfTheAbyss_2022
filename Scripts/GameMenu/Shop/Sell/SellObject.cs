using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Shop
{
    public abstract class SellObject : MonoBehaviour
    {
        #region fields
        [SerializeField] private Text priceText;
        protected int listPosition;
        protected int priceSilver;
        protected int priceGold;
        #endregion fields

        #region methods
        public virtual void Start()
        {
            if (priceSilver == 0 && priceGold == 0)
                Init();
        }
        public void ChangeListPosition(int listPosition)
        {
            this.listPosition = listPosition;
            Init();
        }
        public virtual void Init()
        {
            priceText.text = "";
            if (priceSilver > 0)
                PriceParser.InsertSilverPrice(priceText, priceSilver, false);
            if (priceSilver != 0 && priceGold != 0)
                priceText.text += " ";
            if (priceGold > 0)
                PriceParser.InsertGoldPrice(priceText, priceGold, false);
        }
        public virtual void SellItem()
        {
            GameDataInit.AddSilver(priceSilver, false);
            GameDataInit.AddGold(priceGold, false);
        }
        #endregion methods
    }
}