using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Universal
{
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(Text))]
    public class TextOutline : MonoBehaviour
    {
        #region fields & properties
        private static LanguageData LanguageData;
        public static LanguageData languageData
        {
            get
            {
                LanguageData ??= SavingUtils.LoadLanguage();
                return LanguageData;
            }
            set
            {
                LanguageData = value;
            }
        }
        private Outline Outline;
        private Outline outline
        {
            get 
            {
                Outline ??= GetComponent<Outline>();
                return Outline;
            }
            set
            {
                Outline = value;
            }
        }
        private Text CurrentText;
        private Text currentText
        {
            get
            {
                CurrentText ??= GetComponent<Text>();
                return CurrentText;
            }
            set
            {
                CurrentText = value;
            }
        }
        [HideInInspector] public float lineScaler = 1f;
        #endregion fields & properties

        #region methods
        public IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            if (!GetComponent<LanguageLoad>())
            {
                SetAll();
            }
        }
        public void SetAll()
        {
            outline.effectDistance = Vector2.one * (currentText.cachedTextGenerator.fontSizeUsedForBestFit / 25f) * lineScaler;
            currentText.lineSpacing = languageData.fontSpacing;
            currentText.fontStyle = languageData.fontStyle;
            currentText.font = LanguagesData.instance.fonts[languageData.fontType];
        }

        #endregion methods
    }
}