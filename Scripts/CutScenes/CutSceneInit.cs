using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace CutScene
{
    public sealed class CutSceneInit : SingleSceneInstance
    {
        #region fields
        public static string[] textData;
        public static int scenario = 0;

        [SerializeField] private CutSceneScenarios[] scenarios;
        [SerializeField] private GameObject[] helpObjects;
        [SerializeField] private SpriteRenderer[] backgrounds;
        [SerializeField] private AudioSource soundSource;
        [SerializeField] private AudioClip typingSound;

        private float waitBlackScreenTime = 2f;
        private float waitCharTime = 0.07f;
        private float pitchOffset = 0.08f;
        private CutSceneScenarios scen;
        private int currentProgress;
        private int currentTextProgress;
        private bool pressedForWait;
        private int timesClicked;
        private string sceneToLoad = "GameMenu";
        private int currentTextID => scen.scenarios[currentProgress].subtitleID;

        private List<string> colorNames = new List<string>();
        private List<int> colorPosStart = new List<int>();
        private List<int> colorPosEnd = new List<int>();
        #endregion fields
        public int sceneIdToLoad;
        [ContextMenu("load scene")]
        private void abads()
        {
            GameDataInit.data.currentScenario = sceneIdToLoad;
            StartCoroutine(Start());
        }
        #region methods
        private IEnumerator Start()
        {
            scenario = GameDataInit.data.currentScenario;
            if (GameDataInit.data.isCutSceneInAdventure)
            {
                sceneToLoad = "GameAdventure";
            }
            textData = TextOutline.languageData.cutSceneData;
            waitBlackScreenTime = TextOutline.languageData.subtitleWaitingTime;
            waitCharTime = TextOutline.languageData.subtitleSpeed;
            yield return CustomMath.WaitAFrame();
            scen = scenarios[scenario];
            CharLoading();
            PlayTypingSound(pitchOffset / waitCharTime);
            BGLoading();
            StartCoroutine(FindAllColors());
            if (scenario == 0)
            {
                foreach (var el in helpObjects)
                    el.SetActive(true);
            }
        }
        private IEnumerator FindAllColors()
        {
            colorNames = new List<string>();
            colorPosStart = new List<int>();
            colorPosEnd = new List<int>();
            int startIndex = textData[currentTextID].IndexOf("<color=");
            while (startIndex >= 0)
            {
                int endIndex = textData[currentTextID].IndexOf(">");
                colorPosStart.Add(startIndex + "<color=".Length);
                colorNames.Add(textData[currentTextID].Substring(startIndex + "<color=".Length, endIndex - startIndex - "<color=".Length));

                int finalIndex = textData[currentTextID].IndexOf("</color>");
                colorPosEnd.Add(finalIndex);

                textData[currentTextID] = textData[currentTextID].Remove(finalIndex, 8);
                textData[currentTextID] = textData[currentTextID].Remove(startIndex, 1 + endIndex - startIndex);

                startIndex = textData[currentTextID].IndexOf("<color=");
                yield return CustomMath.WaitAFrame();
            }
        }
        private void RestoreTextXML(bool removeColors = false)
        {
            for (int i = 0; i < colorNames.Count; i++)
            {
                textData[currentTextID] = textData[currentTextID].Insert(colorPosStart[i] + "<color=".Length, $"<color={colorNames[i]}>");
                textData[currentTextID] = textData[currentTextID].Insert(colorPosEnd[i], $"</color>");
                if (removeColors)
                {
                    colorNames.RemoveAt(i);
                    colorPosStart.RemoveAt(i);
                    colorPosEnd.RemoveAt(i);
                }
            }
        }
        private void CharLoading()
        {
            currentTextProgress++;
            Text subtitleText = GameObject.Find("SubtitleText").GetComponent<Text>();
            subtitleText.text = textData[currentTextID].Substring(0, currentTextProgress);
            string changedText = textData[currentTextID];
            int additiveLength = 0;
            for (int i = 0; i < colorNames.Count; i++)
            {
                if (currentTextProgress >= colorPosStart[i] - "<color=".Length)
                {
                    changedText = changedText.Insert(Mathf.Max(0, colorPosStart[i] - "<color=".Length + additiveLength), $"<color={colorNames[i]}>");
                    changedText = changedText.Insert(Mathf.Min(currentTextProgress + $"<color={colorNames[i]}>".Length + additiveLength, colorPosEnd[i] + additiveLength), $"</color>");
                    additiveLength += $"<color={colorNames[i]}></color>".Length;
                }
            }
            subtitleText.text = changedText.Substring(0, currentTextProgress + additiveLength);

            if (currentTextProgress == textData[currentTextID].Length)
            {
                StartCoroutine("BlackScreenLoading");
                StopTypingSound();
            }
            else
            {
                Invoke(nameof(CharLoading), waitCharTime);
            }
        }
        private void BGLoading()
        {
            foreach (SpriteRenderer el in backgrounds)
                el.sprite = scen.scenarios[currentProgress].bgSprite;
        }
        private IEnumerator BlackScreenLoading()
        {
            SceneLoader.BlackScreenFade(true, true, 1f / waitBlackScreenTime);
            float i = 0f;
            while (i < waitBlackScreenTime)
            {
                if (pressedForWait)
                {
                    pressedForWait = false;
                    yield break;
                }
                yield return CustomMath.WaitAFrame();
                i += Time.deltaTime;
            }
            if (pressedForWait)
            {
                pressedForWait = false;
                yield break;
            }
            if (currentProgress == scen.scenarios.Count - 1)
            {
                if (TryLoadScene())
                {
                    LoadScene(0);
                    yield break;
                }
            }
            SceneLoader.BlackScreenFade(false, true, 1f / waitBlackScreenTime);
            NextScene();
        }
        private void LoadScene(float waitTime)
        {
            SceneLoader.instance.LoadScene(sceneToLoad, waitTime);
        }
        private bool TryLoadScene() => !SceneLoader.isSceneLoading;
        private void NextScene()
        {
            CancelInvoke(nameof(CharLoading));
            timesClicked = 0;
            currentTextProgress = 0;
            currentProgress++;
            StartCoroutine(FindAllColors());
            BGLoading();
            CharLoading();
            PlayTypingSound(pitchOffset / waitCharTime);
        }
        private void PlayTypingSound(float pitch, [Optional] bool force)
        {
            if (!soundSource.isPlaying || force)
            {
                soundSource.clip = typingSound;
                soundSource.Play();
                AudioManager.UpdateVolume(soundSource, SoundType.Sound);
                soundSource.pitch = pitch;
            }
        }
        public void StopTypingSound() => soundSource.Stop();
        private void Update()
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Space))
            {
                if (SceneLoader.isSceneLoading)
                    return;
                timesClicked++;

                if (!pressedForWait)
                {
                    CancelInvoke(nameof(CharLoading));
                    currentTextProgress = textData[currentTextID].Length - 1;
                    CharLoading();
                    SceneLoader.BlackScreenFadeZero();
                    pressedForWait = true;
                    StopCoroutine("BlackScreenLoading");
                }
                if (timesClicked >= 3)
                {
                    if (currentProgress < scen.scenarios.Count - 1)
                    {
                        NextScene();
                    }
                    else
                    {
                        if (TryLoadScene())
                            LoadScene(0);
                    }
                }
            }
            if ((Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.Space)) && timesClicked > 0)
            {
                if (!SceneLoader.isSceneLoading)
                {
                    StopCoroutine("BlackScreenLoading");
                    pressedForWait = false;
                    StartCoroutine("BlackScreenLoading");
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneLoader.isSceneLoading)
                    return;
                SceneLoader.instance.LoadScene("Menu", 0f);
            }
        }
        protected override void Awake()
        {
            CheckInstances(GetType());
        }
        #endregion methods
    }
}