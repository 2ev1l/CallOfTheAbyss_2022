using UnityEngine;
using Data;
using Steamworks;

namespace Universal
{
    public class SingleGameInstance : MonoBehaviour
    {
        #region fields
        [SerializeField] private LanguagesData languagesData;
        [SerializeField] private CursorSettings cursorSettings;
        [SerializeField] private SavingUtils savingUtils;
        [SerializeField] private GameDataInit gameDataInit;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioStorage audioStorage;
        [SerializeField] private CustomAnimation customAnimation;
        [SerializeField] private PrefabsData prefabsData;
        [SerializeField] private Achievements achievements;

        public static bool isInitialized = false;
        private bool isMain = false;
        #endregion fields

        #region methods
        private void Awake()
        {
            if (!isInitialized)
            {
                isMain = true;
                isInitialized = true;

                LanguagesData.instance = languagesData;
                savingUtils.Init();
                InitAfterReset();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (!isMain)
                    DestroyImmediate(gameObject);
                gameDataInit.Start();
                sceneLoader.Start();
                audioManager.Start();
                savingUtils.Start();
                achievements.Start();
            }
        }
        public void InitAfterReset()
        {
            cursorSettings.Init();

            PlayerPrefsInit.Init();
            gameDataInit.Init();

            sceneLoader.Init();
            audioStorage.Init();
            audioManager.Init();
            customAnimation.Init();
            prefabsData.Init();
        }
        private void OnApplicationQuit()
        {
            if (SteamManager.Initialized)
                SteamAPI.Shutdown();
        }
        #endregion methods
        [ContextMenu("test")]
        private void test()
        {
            var objs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject el in objs)
            {
                if (el.TryGetComponent<LanguageLoad>(out LanguageLoad ll))
                {
                    if (ll.ID == 60)
                        print(gameObject.name);
                }
            }
        }
    }
}