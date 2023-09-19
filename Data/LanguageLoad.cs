using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace Data
{
    [RequireComponent(typeof(TextOutline))]
    public class LanguageLoad : MonoBehaviour
    {
        #region fields
        [SerializeField] private TextType textType;
        [SerializeField] private int id;
        [SerializeField] private bool disableStart = false;
        public int ID => id;
        #endregion

        #region methods
        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            if (!disableStart)
                Load();
        }
        public void ChangeID(int id)
        {
            this.id = id;
            Load();
        }
        public void Load()
        {
            string[] textData = textType switch
            {
                TextType.Menu => TextOutline.languageData.menuData,
                TextType.Interface => TextOutline.languageData.interfaceData,
                TextType.CutScene => TextOutline.languageData.cutSceneData,
                TextType.CardName => GameDataInit.GetCardsName(),
                TextType.CardDescription => GameDataInit.GetCardsDescription(),
                TextType.ChestName => TextOutline.languageData.chestsNameData,
                TextType.PotionName => TextOutline.languageData.potionsNameData,
                TextType.ArtifactName => TextOutline.languageData.artifactsNameData,
                TextType.Events => TextOutline.languageData.eventsData,
                _ => throw new System.NotImplementedException()
            };
            gameObject.GetComponent<Text>().text = textData[id];
            TextOutline textOutline = gameObject.GetComponent<TextOutline>();
            textOutline.lineScaler = TextOutline.languageData.outlineScale;
            textOutline.SetAll();
        }
        #endregion methods

        public enum TextType { Menu, Interface, CutScene, CardName, CardDescription, ChestName, PotionName, ArtifactName, Events }
    }
}