using UnityEngine;
using UnityEngine.UI;
using Data;
using Universal;
using System.Collections;

namespace GameAdventure
{
    public class GameAdventureButtons : Buttons
    {
        #region methods
        public void GameAdventurePressedBack()
        {
            MakeAfterClimbingEffects();
            StartCoroutine(CustomAnimation.SetTextAlpha(GameAdventureInit.locationText, 1, 0, 0.3f));
            if (GameDataInit.data.currentLocation == GameDataInit.data.locationOffset)
                LoadMenuWithLoot();
            else
                GameAdventureBGAnimation.LoadLocation(false);
        }

        private void LoadMenuWithLoot()
        {
            GameDataInit.AddAllEarnedLoot();
            GameDataInit.earnReward = true;
            SceneLoader.instance.LoadSceneFade("GameMenu", SceneLoader.screenFadeTime + 0.3f);
            if (GameDataInit.data.isTutorialCompleted)
            {
                GameAdventureInit.currentStage.GetComponent<Animator>().SetBool("Down1", true);
                GameAdventureInit.currentStage.transform.Find("LocationName").gameObject.SetActive(false);
            }
        }
        protected void MakeAfterClimbingEffectsAll()
        {
            int maxLocation = GameDataInit.data.currentLocation;
            for (int i = 0; i <= maxLocation; i++)
                MakeAfterClimbingEffects(i);
        }
        protected void MakeAfterClimbingEffects() => MakeAfterClimbingEffects(GameDataInit.data.currentLocation);
        protected void MakeAfterClimbingEffects(int location)
        {
            if (GameDataInit.IsArtifactEffectApplied(ArtifactEffect.LiveCube))
                return;

            switch (location)
            {
                //none
                case 0: break;
                case 1:
                    foreach (CardData el in GameDataInit.deskCards)
                        if (CustomMath.GetRandomChance(30))
                            el.damage = Mathf.Max(el.damage - 1, 0);
                    break;
                case 2:
                    foreach (CardData el in GameDataInit.deskCards)
                        if (CustomMath.GetRandomChance(30))
                            el.hp = Mathf.Max(el.hp - (CustomMath.GetRandomChance(70) ? 3 : 4), 1);
                    break;
                case 3:
                    foreach (CardData el in GameDataInit.deskCards)
                        if (CustomMath.GetRandomChance(35))
                        {
                            el.hp = Mathf.Max(el.hp - (CustomMath.GetRandomChance(70) ? 7 : 11), 1);
                            el.defense = 0;
                        }
                    break;
                case 4:
                    foreach (CardData el in GameDataInit.deskCards)
                        if (CustomMath.GetRandomChance(40))
                        {
                            el.damage = Mathf.Max(el.damage - 4, 0);
                            el.defense = Mathf.Max(el.defense - 3, 0);
                        }
                    break;
                case 5:
                    foreach (CardData el in GameDataInit.deskCards)
                    {
                        int cardLocation = PrefabsData.instance.cardPrefabs[el.id].cardLocation;
                        if (cardLocation < 5)
                        {
                            if (cardLocation == 4)
                            {
                                el.hp = Mathf.Max(el.hp - 15, 1);
                                el.damage = Mathf.Max(el.damage - 5, 0);
                                el.defense = Mathf.Max(el.defense - 2, 0);
                                continue;
                            }
                            el.hp = 1;
                            el.damage = 0;
                            el.defense = 0;
                        }
                        else
                        {
                            if (CustomMath.GetRandomChance(40))
                            {
                                el.hp = Mathf.Max(el.hp / 2, 0);
                                el.damage = Mathf.Max(el.damage / 2, 0);
                                el.defense = Mathf.Max(el.defense - 3, 0);
                            }
                        }
                    }
                    break;
                default: goto case 5;
            }
        }
        public void GameAdventurePressedDie()
        {
            GameDataInit.ResetEarnedLoot(0, 0, false);
            foreach (var el in GameDataInit.deskCards)
            {
                el.hp = 0;
                el.damage = 0;
                el.defense = 0;
            }
            GameDataInit.RemoveCardsFromDesk();
            SceneLoader.instance.LoadSceneFade("GameMenu", SceneLoader.screenFadeTime + 0.3f);
        }
        public void GameAdventurePressedEscape()
        {
            if (CustomMath.GetRandomChance(24))
                LoadMenuWithLoot();
            else
                GameAdventurePressedDie();
        }
        #endregion methods
    }
}