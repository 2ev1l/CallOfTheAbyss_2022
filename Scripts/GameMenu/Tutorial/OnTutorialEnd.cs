using UnityEngine;
using Universal;

namespace GameMenu.Tutorial
{
    public class OnTutorialEnd : MonoBehaviour
    {
        private void Start() => SceneLoader.instance.LoadSceneFade("GameMenu", 2f);
    }
}