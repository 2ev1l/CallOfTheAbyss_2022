using UnityEngine;
using Menu;

namespace Universal
{
    public class PlayerPrefsInit : Cryptography
    {
        public static readonly string prefSoundName = "soundVolume";
        public static readonly string prefMusicName = "musicVolume";
        public static readonly string prefLangName = "language";
        public static readonly string prefVsyncName = "Vsync";

        public static void Init()
        {
            InitPrefVolumeKeys(prefSoundName, prefMusicName);
        }

        private static void InitPrefVolumeKeys(params string[] prefNames)
        {
            foreach (string el in prefNames)
            {
                if (!PlayerPrefs.HasKey(el))
                    PlayerPrefs.SetFloat(el, 0.5f);
            }
            MenuMusicInit.soundVolume = PlayerPrefs.GetFloat(prefSoundName);
            MenuMusicInit.musicVolume = PlayerPrefs.GetFloat(prefMusicName);
        }
    }
}