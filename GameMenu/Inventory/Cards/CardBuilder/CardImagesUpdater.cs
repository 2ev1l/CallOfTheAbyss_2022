using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFight.Card;
using Data;
using Universal;
using GameMenu.Inventory.Storages;

namespace GameMenu.Inventory.Cards.Builder
{
    public class CardImagesUpdater : DefaultUpdater
    {
        #region fields
        [SerializeField] protected CardMenuInit cardMenuInit;
        [SerializeField] private Image mainImage;
        [SerializeField] private Image onSelectImage;
        [SerializeField] protected Image layoutImage;
        [SerializeField] private Image typeImage;
        [SerializeField] private ShowHelp typeHelp;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            cardMenuInit.OnValuesUpdate += SetImages;
        }
        protected override void OnDisable()
        {
            cardMenuInit.OnValuesUpdate -= SetImages;
        }
        protected void SetImages(List<CardData> cardDatas, int listPosition, int id)
        {
            CardInfoSO cardInfo = cardMenuInit.cardInfo;
            mainImage.sprite = cardInfo.cardBG;
            onSelectImage.enabled = cardMenuInit.HasDescription();
            ChangeLayoutImage();
            switch (cardInfo.creatureType)
            {
                case CreatureType.Ground: ChangeGroundCreationValues(); break;
                case CreatureType.Underwater: ChangeUnderwaterCreationValues(); break;
                case CreatureType.Flying: ChangeFlyingCreationValues(); break;
                default: throw new System.NotImplementedException();
            }
        }
        protected virtual void ChangeLayoutImage()
        {
            layoutImage.sprite = InventoryCardStorage.instance.inventoryCardLayoutSprites[cardMenuInit.cardInfo.rareTier];
        }
        protected virtual void ChangeGroundCreationValues()
        {
            typeImage.sprite = InventoryCardStorage.instance.groundType;
            typeHelp.id = 27;
        }
        protected virtual void ChangeUnderwaterCreationValues()
        {
            typeImage.sprite = InventoryCardStorage.instance.waterType;
            typeHelp.id = 28;
        }
        protected virtual void ChangeFlyingCreationValues()
        {
            typeImage.sprite = InventoryCardStorage.instance.skyType;
            typeHelp.id = 29;
        }
        #endregion methods
    }
}