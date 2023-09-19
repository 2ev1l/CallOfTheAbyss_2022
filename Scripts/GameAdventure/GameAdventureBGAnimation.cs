using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Universal;

namespace GameAdventure
{
    public class GameAdventureBGAnimation : SingleSceneInstance
    {
        private static Animator currentStageAnimator => GameAdventureInit.currentStage.GetComponent<Animator>();
        public static bool isPressedDown;
        protected override void Awake()
        {
            CheckInstances(GetType());
        }
        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();

            if (isPressedDown)
                currentStageAnimator.SetBool("BackDown1", true);
            else
                currentStageAnimator.SetBool("BackUp1", true);

            Invoke(nameof(StopAnimation), 1f);
        }
        private void StopAnimation()
        {
            currentStageAnimator.SetBool("BackDown1", false);
            currentStageAnimator.SetBool("BackUp1", false);
        }
        public static void LoadLocation(bool down)
        {
            GameDataInit.data.currentLocation += down ? 1 : -1;
            isPressedDown = down;
            if (down)
            {
                if (GameDataInit.data.currentLocation >= GameDataInit.data.currentLocationMax)
                    GameDataInit.data.currentLocationMax = GameDataInit.data.currentLocation;

                if (GameDataInit.data.currentLocation > GameDataInit.data.reachedLocation)
                {
                    GameDataInit.data.reachedLocation = GameDataInit.data.currentLocation;
                    GameDataInit.data.reachedPoint = 0;
                }
            }
            StartClosingAnimation(down);
            SceneLoader.instance.LoadSceneFade(SceneManager.GetActiveScene().name, SceneLoader.screenFadeTime + 0.3f);
        }
        public static void StartClosingAnimation(bool AnimationUp) => currentStageAnimator.SetBool(AnimationUp ? "Up1" : "Down1", true);
    }
}