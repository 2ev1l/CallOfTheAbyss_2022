using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace GameMenu.House
{
    public class HouseTier : SingleSceneInstance
    {
        #region fields
        public static HouseTier instance { get; private set; }
        public static int[] silverPricePerHP;
        public static int[] hpPerMin;
        [SerializeField] private Sprite[] houseSprites;
        [SerializeField] private Image houseImage;
        #endregion fields

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
            UpdateValues();
        }
        private void ChangeImage(int tier)
        {
            houseImage.sprite = houseSprites[tier];
        }
        public void UpdateValues()
        {
            UpgradeData houseData = GameDataInit.data.upgradeData.Find(el => el.id == 0);
            int houseTier = houseData.tier;
            ChangeImage(houseTier);
            ChangeUpdatableValues(houseTier);
        }
        private void ChangeUpdatableValues(int tier)
        {
            switch (tier)
            {
                case 0:
                    silverPricePerHP = new int[] { 1, 2, 3, 5 };
                    hpPerMin = new int[] { 15, 7, 3, 1 };
                    break;
                case 1:
                    silverPricePerHP = new int[] { 1, 2, 3, 5 };
                    hpPerMin = new int[] { 20, 12, 6, 4 };
                    break;
                case 2:
                    silverPricePerHP = new int[] { 1, 1, 2, 4 };
                    hpPerMin = new int[] { 25, 18, 11, 7 };
                    break;
                case 3:
                    silverPricePerHP = new int[] { 1, 1, 1, 3 };
                    hpPerMin = new int[] { 33, 24, 17, 12 };
                    break;
                case 4:
                    silverPricePerHP = new int[] { 1, 1, 1, 2 };
                    hpPerMin = new int[] { 42, 30, 23, 18 };
                    break;
                default: throw new System.NotImplementedException();
            }
        }

        #endregion methods
    }
}