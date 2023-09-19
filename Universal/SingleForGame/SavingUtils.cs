using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Data;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class SavingUtils : Cryptography
    {
        public static SavingUtils instance { get; private set; }

        public static string choosedLanguage = "English";
        public static string streamingAssetsPath;
        [SerializeField] private SingleGameInstance singleGameInstance;

        private static readonly string[] notUpdatingTimeScenes = new string[] { "Menu", "CutScenes" };
        private static string currentScene;
        private static bool isTimeNotScaling;
        private static bool isTimeNotUpdating;
        public void Init()
        {
            instance = this;
            choosedLanguage = GetChoosedLanguageName();
            streamingAssetsPath = Application.dataPath + "/StreamingAssets";
            if (!File.Exists(Path.Combine(Application.persistentDataPath, GameDataInit.saveName + ".data")))
                ResetTotalProgress(Difficulty.Normal);

            StartCoroutine(UpdateTime());
            LoadGameData();
        }
        private void Awake()
        {
            CheckArtifactEffects();
            currentScene = SceneManager.GetActiveScene().name;
        }
        private void OnEnable()
        {
            GameDataInit.instance.OnArtifactEffectsChanged += CheckArtifactEffects;
        }
        private void OnDisable()
        {
            GameDataInit.instance.OnArtifactEffectsChanged -= CheckArtifactEffects;
        }
        private void CheckArtifactEffects()
        {
            isTimeNotScaling = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.WarpedHourglass);
            isTimeNotUpdating = GameDataInit.IsArtifactEffectApplied(ArtifactEffect.BrokenHourglass);
        }
        public void Start()
        {
            CheckArtifactEffects();
            currentScene = SceneManager.GetActiveScene().name;
            CheckAchievement(GameDataInit.data.playTime);
            SaveGameData();
        }
        public static string GetChoosedLanguageName() => PlayerPrefs.HasKey(PlayerPrefsInit.prefLangName) ? PlayerPrefs.GetString(PlayerPrefsInit.prefLangName) : choosedLanguage;

        [ContextMenu("Save ENGLISH Language")]
        private void SaveLanguage()
        {
            LanguageData data = LanguagesData.instance.languageData;
            string json = JsonUtility.ToJson(data);
            string path = Path.Combine(streamingAssetsPath, $"English.json");
            File.WriteAllText(path, json);
            Debug.Log(path + " saved");
        }
        public static LanguageData LoadLanguage()
        {
            string json = File.ReadAllText(Path.Combine(streamingAssetsPath, $"{choosedLanguage}.json"));
            LanguageData data = JsonUtility.FromJson<LanguageData>(json);
            return data;
        }
        public static LanguageData LoadLanguage(string lang)
        {
            string json = File.ReadAllText(Path.Combine(streamingAssetsPath, $"{lang}.json"));
            LanguageData data = JsonUtility.FromJson<LanguageData>(json);
            return data;
        }
        public static void SaveGameData()
        {
            string rawJson = JsonUtility.ToJson(GameDataInit.data);
            string json = Encrypt(rawJson);
            using (FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, GameDataInit.saveName + ".data"), FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, json);
                fs.Close();
            }
        }
        public static void LoadGameData()
        {
            string json;
            using (FileStream fs = new FileStream(Path.Combine(Application.persistentDataPath, GameDataInit.saveName + ".data"), FileMode.Open))
            {
                var bf = new BinaryFormatter();
                json = bf.Deserialize(fs).ToString();
                json = Decrypt(json);
                fs.Close();
            }
            GameDataInit.data = JsonUtility.FromJson<GameData>(json);
        }

        public static void ResetTotalProgress(Difficulty difficulty)
        {
            bool isGameCompleted = false;
            int totalTime = 0;
            if (GameDataInit.data != null)
            {
                isGameCompleted = GameDataInit.data.isGameCompleted;
                totalTime = GameDataInit.data.playTime;
            }
            GameDataInit.data = new GameData();
            GameDataInit.data.difficulty = difficulty;
            GameDataInit.data.isGameCompleted = isGameCompleted;
            GameDataInit.data.playTime = totalTime;
            SaveGameData();
            Debug.Log("Progress reset");
            instance.singleGameInstance.InitAfterReset();
        }
        private static IEnumerator UpdateTime()
        {
            yield return new WaitForSecondsRealtime(1);
            TryGetTimeScaled(out int time);
            GameDataInit.data.playTime++;
            GameDataInit.data.shopTime += time;
            GameDataInit.data.trashTime += time;
            GameDataInit.data.houseTime += time;
            instance.StartCoroutine(UpdateTime());
        }

        private static bool TryGetTimeScaled(out int time)
        {
            time = 1;
            if (notUpdatingTimeScenes.Contains(currentScene)) return false;
            time = GameDataInit.data.currentLocation switch
            {
                0 => 1,
                1 => 1,
                2 => Random.Range(1, 3),
                3 => Random.Range(1, 4),
                4 => Random.Range(1, 5),
                5 => Random.Range(1, 6),
                _ => 10,
            };
            time = GameDataInit.data.sceneName.Equals("GameMenu") ? 1 : time;
            time = isTimeNotScaling ? 1 : time;
            time = isTimeNotUpdating ? 0 : time;
            return true;
        }
        private static void CheckAchievement(int playTime)
        {
            switch (playTime)
            {
                case int i when i >= 36000: Achievements.SetAchievement("A_REALTIME_PLAYED_600"); break;
                case int i when i >= 14400: Achievements.SetAchievement("A_REALTIME_PLAYED_240"); break;
                case int i when i >= 7200: Achievements.SetAchievement("A_REALTIME_PLAYED_120"); break;
                case int i when i >= 3600: Achievements.SetAchievement("A_REALTIME_PLAYED_60"); break;
            }
        }
        private void OnApplicationQuit()
        {
            SaveGameData();
        }
    }
}