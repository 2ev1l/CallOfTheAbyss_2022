using UnityEngine;
using Universal;

namespace Menu
{
    public class MenuMusicInit : SingleSceneInstance
    {
        #region fields & properties
        public static MenuMusicInit instance { get; private set; }

        public static float soundVolume = 1f;
        public static float musicVolume = 1f;

        [field: SerializeField] public Sprite volumeChoosedSprite { get; private set; }
        [field: SerializeField] public Sprite volumeUnChoosedSprite { get; private set; }

        [SerializeField] private GameObject[] volumeArrows;
        [SerializeField] private SpriteRenderer[] soundIndicator;
        [SerializeField] private SpriteRenderer[] musicIndicator;
        #endregion fields & properties

        #region methods
        private void Start()
        {
            int idSound = Mathf.RoundToInt((soundVolume * 10) - 1);
            int idMusic = Mathf.RoundToInt((musicVolume * 10) - 1);
            UpdateSettingsMusic(idMusic);
            UpdateSettingsSound(idSound);
        }
        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }

        public void PressedSettingsSoundChange(GameObject obj)
        {
            int id = System.Convert.ToInt32(obj.name[obj.name.Length - 1]) - 48;
            soundVolume = (id + 1) / 10f;
            if (id == -1) soundVolume /= 2f;
            UpdateSettingsSound(id);
        }
        public void PressedSettingsSoundChangeButton(bool decrease)
        {
            int counter = decrease switch
            {
                true => 1,
                false => -1
            };
            int id = Mathf.RoundToInt((soundVolume * 10) - 1 - counter);
            if (id == -2 || id == 10) return;
            soundVolume = (id + 1) / 10f;
            if (id == -1) soundVolume /= 2f;
            UpdateSettingsSound(id);
        }
        public void UpdateSettingsSound(int id)
        {
            switch (id)
            {
                case -1:
                    volumeArrows[0].SetActive(false);
                    break;
                case 9:
                    volumeArrows[1].SetActive(false);
                    break;
                default:
                    volumeArrows[0].SetActive(true);
                    volumeArrows[1].SetActive(true);
                    break;
            }
            for (int i = id; i >= 0; i--)
                soundIndicator[i].sprite = volumeUnChoosedSprite;
            for (int i = id + 1; i <= 9; i++)
                soundIndicator[i].sprite = volumeChoosedSprite;

            PlayerPrefs.SetFloat(PlayerPrefsInit.prefSoundName, soundVolume);
        }
        public void PressedSettingsMusicChange(GameObject obj)
        {
            int id = System.Convert.ToInt32(obj.name[obj.name.Length - 1]) - 48;
            musicVolume = (id + 1) / 10f;
            if (id == -1) musicVolume /= 2f;
            UpdateSettingsMusic(id);
        }
        public void PressedSettingsMusicChangeButton(bool decrease)
        {
            int counter = decrease switch
            {
                true => 1,
                false => -1
            };
            int id = Mathf.RoundToInt((musicVolume * 10) - 1 - counter);
            if (id == -2 || id == 10) return;
            musicVolume = (id + 1) / 10f;
            if (id == -1) musicVolume /= 2f;
            UpdateSettingsMusic(id);
        }
        public void UpdateSettingsMusic(int id)
        {
            switch (id)
            {
                case -1:
                    volumeArrows[2].SetActive(false);
                    break;
                case 9:
                    volumeArrows[3].SetActive(false);
                    break;
                default:
                    volumeArrows[2].SetActive(true);
                    volumeArrows[3].SetActive(true);
                    break;
            }
            for (int i = id; i >= 0; i--)
                musicIndicator[i].sprite = volumeUnChoosedSprite;
            for (int i = id + 1; i <= 9; i++)
                musicIndicator[i].sprite = volumeChoosedSprite;

            PlayerPrefs.SetFloat(PlayerPrefsInit.prefMusicName, musicVolume);
            AudioManager.UpdateVolume(AudioManager.instance.musicSource, SoundType.Music);
        }
        #endregion methods
    }
}