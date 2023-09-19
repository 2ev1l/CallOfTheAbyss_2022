using Data;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Potions
{
    public class PotionUI : DefaultUpdater
    {
        #region fields
        [SerializeField] private PotionInit potion;
        [SerializeField] protected Image mainImage;
        [SerializeField] private LanguageLoad mainText;
        [SerializeField] private ShowHelp help;
        [SerializeField] private bool updateHelp = true;
        #endregion fields

        #region methods
        protected override void OnEnable()
        {
            potion.OnValuesUpdate += UpdateValues;
        }
        protected override void OnDisable()
        {
            potion.OnValuesUpdate -= UpdateValues;
        }
        protected virtual void UpdateValues()
        {
            PotionInfo potionInfo = GetPotionInfo();
            mainImage.sprite = potionInfo.sprite;
            mainText.ChangeID((int)potion.potionInfo.effect);
            if (!updateHelp) return;
            help.id = -1;
            help.additionalText = GetPotionInfoText(potionInfo);
        }
        protected PotionInfo GetPotionInfo() => PrefabsData.instance.potionInfo.Find(x => x.effect == potion.potionInfo.effect);
        protected string GetPotionInfoText(PotionInfo potionInfo)
        {
            string helpText = TextOutline.languageData.helpData[potionInfo.helpID];
            int index = helpText.IndexOf("[X]");
            if (index > -1)
            {
                helpText = helpText.Remove(index, 3);
                helpText = helpText.Insert(index, potion.potionInfo.value.ToString());
            }
            return helpText;
        }
        #endregion methods
    }
}