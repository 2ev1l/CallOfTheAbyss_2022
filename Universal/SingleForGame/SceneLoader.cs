using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader instance { get; private set; }
        public static bool isSceneLoading;
        public static float screenFadeTime = 1f;

        private static readonly string fadeScreenName = "Color-Fade";
        private static string sceneToLoad;

        #region methods
        public void Init()
        {
            instance = this;
        }
        public void Start()
        {
            isSceneLoading = false;
            BlackScreenFade(false);
        }
        public static void LoadCutScene(int scenario, bool isCutSceneInAdventure)
        {
            isSceneLoading = true;
            CutScene.CutSceneInit.scenario = scenario;
            GameDataInit.data.isCutSceneInAdventure = isCutSceneInAdventure;
            GameDataInit.data.currentScenario = scenario;
            instance.LoadSceneFade("CutScenes", screenFadeTime);
        }
        public void LoadSceneFade(string scene, float time)
        {
            isSceneLoading = true;

            if (ShowHelp.helpText != null)
                ShowHelp.helpText.SetActive(false);
            CursorSettings.instance.SetDefaultCursor();
            BlackScreenFade(true);
            LoadScene(scene, time);
            RemoveEvents();
        }
        public void LoadScene(string scene, float time)
        {
            isSceneLoading = true;
            sceneToLoad = scene;
            Invoke(nameof(LoadSceneA), time);
            RemoveEvents();
        }
        private void LoadSceneA() => StartCoroutine(LoadScene(sceneToLoad));
        private static IEnumerator LoadScene(string scene)
        {
            isSceneLoading = true;
            SavingUtils.SaveGameData();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
            yield return asyncLoad;
            if (scene.Equals("GameAdventure"))
            {
                if (GameDataInit.data.isCutSceneInAdventure)
                    GameDataInit.data.isCutSceneInAdventure = false;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
            isSceneLoading = false;
        }
        public static void BlackScreenFade(bool fade, [Optional] bool editSpeed, [Optional] float animationSpeed)
        {
            Animator animator = GameObject.Find(fadeScreenName).GetComponent<Animator>();
            animator.SetBool("FadeUp", fade);
            animator.SetBool("FadeDown", !fade);
            if (editSpeed)
            {
                animator.speed = animationSpeed;
            }
        }
        public static void BlackScreenFadeZero()
        {
            Animator animator = GameObject.Find(fadeScreenName).GetComponent<Animator>();
            animator.SetBool("FadeUp", false);
            animator.SetBool("FadeDown", false);
        }
        private static void RemoveEvents()
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem != null)
                eventSystem.SetActive(false);
        }
        public static bool IsBlackScreenFade() => GameObject.Find(fadeScreenName).GetComponent<Animator>().GetBool("FadeUp");

        #endregion methods
    }
}