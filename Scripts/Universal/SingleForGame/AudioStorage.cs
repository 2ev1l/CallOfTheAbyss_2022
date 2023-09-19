using UnityEngine;

namespace Universal
{
    public class AudioStorage : MonoBehaviour
    {
        public static AudioStorage instance { get; private set; }
        [field: SerializeField] public AudioClip musicMenu { get; private set; }
        [field: SerializeField] public AudioClip musicMainBackground { get; private set; }
        [field: SerializeField] public AudioClip musicOnTravel { get; private set; }
        [field: SerializeField] public AudioClip[] stageMusic { get; private set; }
        public void Init()
        {
            instance = this;
        }
    }
}