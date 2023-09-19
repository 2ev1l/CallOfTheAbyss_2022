using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;

namespace Menu
{
    public class MenuLanguageInit : SingleSceneInstance
    {
        #region fields & properties
        public static MenuLanguageInit instance { get; private set; }

        private static string[] languageNames;
        private static int languageCounter;

        [SerializeField] private GameObject[] languageOptions;
        [SerializeField] private GameObject[] languageArrows;
        [SerializeField] private GameObject[] languageOptionsChoosed;
        #endregion fields & properties

        #region methods
        private IEnumerator Start()
        {
            var diInfo = new DirectoryInfo(SavingUtils.streamingAssetsPath);
            var filesInfo = diInfo.GetFiles("*.json");
            List<string> list = new List<string>();
            for (int i = 0; i < filesInfo.Length; i++)
            {
                if (filesInfo[i].Name == SavingUtils.choosedLanguage + ".json")
                    languageCounter = i;
                list.Add(filesInfo[i].Name.Remove(filesInfo[i].Name.Length - 5));
            }
            languageNames = list.ToArray();

            yield return CustomMath.WaitAFrame();
            UpdateLanguageTab();
        }

        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }

        public void UpdateLanguageTab()
        {
            languageArrows[0].SetActive(languageCounter != 0);
            languageArrows[1].SetActive(languageNames.Length - languageCounter >= 4);

            foreach (var el in languageOptions)
            {
                el.GetComponent<Text>().text = "LanguageX";
                el.SetActive(true);
            }
            foreach (var el in languageOptionsChoosed)
                el.SetActive(false);

            for (int i = 0; i < languageNames.Length - languageCounter; i++)
            {
                languageOptions[i].GetComponent<Text>().text = languageNames[languageCounter + i];
                if (i == 2) break;
            }

            for (int i = 0; i < languageOptions.Length; i++)
            {
                Text languageOptionText = languageOptions[i].GetComponent<Text>();

                if (languageOptionText.text == "LanguageX")
                {
                    languageOptions[i].SetActive(false);
                }
                else
                {
                    LanguageData itemData = SavingUtils.LoadLanguage(languageOptionText.text);
                    languageOptionText.font = LanguagesData.instance.fonts[SavingUtils.LoadLanguage(languageOptionText.text).fontType];
                    languageOptionText.fontStyle = itemData.fontStyle;
                    languageOptionText.lineSpacing = itemData.fontSpacing;
                }

                if (languageOptionText.text == SavingUtils.choosedLanguage)
                {
                    languageOptionsChoosed[i].SetActive(true);
                }
            }
        }
        public void PressedSettingsLanguageChange(Text txt)
        {
            if (txt.text != SavingUtils.choosedLanguage)
            {
                SaveLang(txt.text);
                var objs = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.GetComponent<LanguageLoad>());
                foreach (var el in objs)
                {
                    el.GetComponent<LanguageLoad>().Load();
                }
            }
        }
        public void PressedSettingsLanguageChangeArrow(bool up)
        {
            if (!up)
            {
                if (languageCounter < languageNames.Length - 1)
                    languageCounter++;
            }
            else
            {
                if (languageCounter > 0)
                    languageCounter--;
            }
            UpdateLanguageTab();
        }
        public void SaveLang(string lang)
        {
            PlayerPrefs.SetString(PlayerPrefsInit.prefLangName, lang);
            SavingUtils.choosedLanguage = lang;
            TextOutline.languageData = SavingUtils.LoadLanguage();
            UpdateLanguageTab();
        }
        #endregion methods
    }
}