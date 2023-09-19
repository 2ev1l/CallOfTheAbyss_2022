using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universal
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        [field: SerializeField] public AudioSource musicSource { get; private set; }

        #region methods
        public void Init()
        {
            instance = this;
        }
        public void Start()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Menu": PlayMusic(AudioStorage.instance.musicMenu); break;
                case "CutScenes": PlayMusic(AudioStorage.instance.musicMainBackground); break;
                case "GameMenu": PlayMusic(AudioStorage.instance.musicMainBackground); break;
                case "GameAdventure": PlayMusic(AudioStorage.instance.musicOnTravel); break;
                case "GameFight": PlayMusic(AudioStorage.instance.stageMusic[GameDataInit.data.currentLocation]); break;
                case "GameEvent": PlayMusic(AudioStorage.instance.stageMusic[GameDataInit.data.currentLocation]); break; 
                default: throw new NotImplementedException();
            }
        }
        private static string GetPrefKeyByType(SoundType type)
        {
            return type switch
            {
                SoundType.Music => PlayerPrefsInit.prefMusicName,
                SoundType.Sound => PlayerPrefsInit.prefSoundName,
                _ => throw new System.NotImplementedException()
            };
        }
        public static void PlayClip(AudioClip clip, SoundType type)
        {
            float volume = 1f * PlayerPrefs.GetFloat(GetPrefKeyByType(type));
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }
        public static void PlayMusic(AudioClip clip, [Optional] bool force)
        {
            if (force || instance.musicSource.clip != clip)
            {
                instance.musicSource.clip = clip;
                instance.musicSource.volume = 1f * PlayerPrefs.GetFloat(PlayerPrefsInit.prefMusicName);
                instance.musicSource.Play();
            }
        }
        public static void UpdateVolume(AudioSource audioSource, SoundType type)
        {
            audioSource.volume = 1f * PlayerPrefs.GetFloat(GetPrefKeyByType(type));
        }
        #endregion methods
    }
    public enum SoundType { Music, Sound }
}