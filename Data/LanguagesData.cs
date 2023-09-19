using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class LanguagesData : MonoBehaviour
    {
        #region fields
        public static LanguagesData instance;
        [field: SerializeField] public Font[] fonts { get; private set; }

        [field: SerializeField] public LanguageData languageData { get; private set; }
        [SerializeField] private Color colorPicker;
        #endregion fields
    }

    [System.Serializable]
    public class LanguageData
    {
        #region fields
        public float outlineScale = 1f;
        public int fontType = 0;
        public FontStyle fontStyle = 0;
        public float fontSpacing = 1f;
        public float subtitleSpeed = 0.07f;
        public float subtitleWaitingTime = 2f;
        public string[] menuData;
        [TextArea(0, 30)] public string[] interfaceData;
        [TextArea(0, 30)] public string[] cutSceneData;
        [TextArea(0, 30)] public string[] eventsData;
        public string[] helpData;
        public CardTextData[] cardsTextData;
        public string[] chestsNameData;
        public string[] potionsNameData;
        public string[] artifactsNameData;
        #endregion fields
    }
    [System.Serializable]
    public class CardTextData
    {
        //public int id; //to view an array element
        public string name;
        [TextArea(7, 45)] public string description;
    }
}