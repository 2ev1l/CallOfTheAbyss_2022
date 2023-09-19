using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Universal
{
    public class ShowHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region fields
        public static GameObject helpText;
        private static string lastScene = "";
        protected static Text txt;
        private static TextOutline txtOutline;
        public static string[] langData;

        [Tooltip("ID of [LanguageData.HelpData]")]
        public int id;
        [HideInInspector] public string additionalText;
        #endregion fields

        #region methods
        private void Start()
        {
            if (helpText == null || lastScene == "" || lastScene != SceneManager.GetActiveScene().name)
            {
                helpText = GameObject.Find("HelpPanel");
                txt = helpText.transform.Find("HelpText").Find("Text").GetComponent<Text>();
                txtOutline = txt.gameObject.GetComponent<TextOutline>();
                langData = TextOutline.languageData.helpData;
                helpText.SetActive(false);
                lastScene = SceneManager.GetActiveScene().name;
                helpText.transform.localScale *= CustomMath.GetOptimalScreenScale();
                HelpUpdate.offsetHelpX = -1.34f * CustomMath.GetOptimalScreenScale();
                HelpUpdate.offsetHelpY = -0.24f * CustomMath.GetOptimalScreenScale();
            }
        }
        protected IEnumerator UpdateLine()
        {
            yield return CustomMath.WaitAFrame();
            txtOutline.SetAll();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!SceneLoader.IsBlackScreenFade())
            {
                helpText.SetActive(true);
                txt.text = id >= 0 ? langData[id] : "";
                txt.text += additionalText;
                StartCoroutine(UpdateLine());
            }
            else
            {
                helpText.SetActive(false);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (helpText != null)
                helpText.SetActive(false);
        }
        protected virtual void OnDisable()
        {
            if (helpText == null) return;
            helpText.SetActive(false);
        }
        #endregion methods
    }
}