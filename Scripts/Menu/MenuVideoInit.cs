using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Universal;

namespace Menu
{
    public class MenuVideoInit : SingleSceneInstance
    {
        #region fields & properties
        public static MenuVideoInit instance { get; private set; }

        private static int videoCounter;
        private static Resolution[] videoResolutions;

        [SerializeField] private GameObject[] videoArrows;
        [SerializeField] private GameObject[] videoQualityArrows;
        [SerializeField] private GameObject vsyncMark;
        [SerializeField] private Text videoOptionText;
        #endregion fields & properties

        #region methods
        private IEnumerator Start()
        {
            videoOptionText.text = UpdateVideoText(Screen.currentResolution.ToString());
            videoResolutions = ResolutionUniqueWH().ToArray();
            videoCounter = videoResolutions.ToList().FindIndex(res => res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height);
            videoCounter = videoCounter < 0 ? videoResolutions.Length - 1 : videoCounter;

            if (!PlayerPrefs.HasKey(PlayerPrefsInit.prefVsyncName))
                PlayerPrefs.SetInt(PlayerPrefsInit.prefVsyncName, 1);
            int vsyncCounter = PlayerPrefs.GetInt(PlayerPrefsInit.prefVsyncName);
            vsyncMark.SetActive(vsyncCounter == 1);
            QualitySettings.vSyncCount = vsyncCounter;
            videoQualityArrows[QualitySettings.GetQualityLevel()].SetActive(true);
            yield return CustomMath.WaitAFrame();
            UpdateVideoTab();
        }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }

        private string UpdateVideoText(string text)
        {
            text = text.Remove(CharPosition(text, '@') - 1);
            text = text.Remove(CharPosition(text, ' '), 1);
            text = text.Remove(CharPosition(text, ' '), 1);
            return text;
        }
        public void UpdateVideoTab()
        {
            videoArrows[1].SetActive(videoCounter != 0);
            videoArrows[0].SetActive(videoCounter != videoResolutions.Length - 1);

            Text videoText = GameObject.Find("VideoOption").GetComponent<Text>();
            videoText.text = UpdateVideoText(videoResolutions[videoCounter].ToString());
        }
        private List<Resolution> ResolutionUniqueWH()
        {
            int choosedWidth = 0;
            int choosedHeight = 0;
            List<Resolution> res = new List<Resolution>();
            int resLength = Screen.resolutions.Length;
            for (int i = 0; i < resLength; i++)
            {
                Resolution currentResolution = Screen.resolutions[i];
                if (choosedWidth != currentResolution.width && choosedHeight != currentResolution.height)
                {
                    choosedHeight = currentResolution.height;
                    choosedWidth = currentResolution.width;
                    res.Add(currentResolution);
                }
            }
            return res;
        }
        private static int CharPosition(string str, char chr)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == chr) return i;
            }
            return -1;
        }

        public void PressedVideoChangeArrow(bool up)
        {
            if (up)
            {
                if (videoCounter < videoResolutions.Length - 1)
                    videoCounter++;
            }
            else
            {
                if (videoCounter > 0)
                    videoCounter--;
            }
            UpdateVideoTab();
        }
        public void PressedVideoChange() => StartCoroutine(PressedVideoChanged());
        private IEnumerator PressedVideoChanged()
        {
            Screen.SetResolution(videoResolutions[videoCounter].width, videoResolutions[videoCounter].height, Screen.fullScreenMode);

            yield return CustomMath.WaitAFrame();
            var objs = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.GetComponent<TextOutline>());
            foreach (var el in objs)
                el.GetComponent<TextOutline>().SetAll();
        }
        public void PressedVideoGraphicsChange(int level)
        {
            QualitySettings.SetQualityLevel(level);
            for (int i = 0; i < videoQualityArrows.Length; i++)
                videoQualityArrows[i].SetActive(i == QualitySettings.GetQualityLevel());

            QualitySettings.vSyncCount = PlayerPrefs.GetInt(PlayerPrefsInit.prefVsyncName);
        }
        public void PressedVsyncChange()
        {
            PlayerPrefs.SetInt(PlayerPrefsInit.prefVsyncName, 1 - PlayerPrefs.GetInt(PlayerPrefsInit.prefVsyncName));
            vsyncMark.SetActive(PlayerPrefs.GetInt(PlayerPrefsInit.prefVsyncName) == 1);
            QualitySettings.vSyncCount = PlayerPrefs.GetInt(PlayerPrefsInit.prefVsyncName);
        }
        #endregion methods
    }
}