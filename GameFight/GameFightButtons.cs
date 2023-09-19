using GameAdventure;
using Universal;

namespace GameFight
{
    public sealed class GameFightButtons : GameAdventureButtons
    {
        #region methods
        public void GameFightPressedBack(bool isDead)
        {
            if (!isDead)
            {
                if (CheckTutorial()) return;

                if (FightPoint.fightParametersCopy.loadCutScene)
                {
                    GameDataInit.data.isCutSceneInAdventure = true;
                    SceneLoader.LoadCutScene(FightPoint.fightParametersCopy.cutSceneId, true);
                }
                else
                {
                    SceneLoader.instance.LoadSceneFade("GameAdventure", SceneLoader.screenFadeTime);
                }
            }
            else
            {
                SceneLoader.instance.LoadSceneFade("GameMenu", SceneLoader.screenFadeTime);
                MakeAfterClimbingEffectsAll();
            }
        }
        private bool CheckTutorial()
        {
            if (GameDataInit.data.isTutorialCompleted) return false;

            GameDataInit.data.tutorialProgress = 6;
            GameAdventurePressedBack();
            return true;
        }
        public void GameFightPressedSkipAnimation()
        {
            FightAnimationInit.instance.OnSkipFightAnimation?.Invoke();
        }
        #endregion methods
    }
}