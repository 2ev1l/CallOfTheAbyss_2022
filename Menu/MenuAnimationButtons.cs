using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal;

namespace Menu
{
    public sealed class MenuAnimationButtons : Buttons
    {
        #region fields
        private static bool pressedPlay = false;
        private static bool pressedSettings = false;
        private static List<string> iconsAnimationNames => new List<string> { "MusicV2", "LanguageV2", "VideoV2", "DevelopersV2" };
        #endregion fields

        #region methods
        public void PressedMenuUp(bool up)
        {
            Animator animator = GameObject.Find("StartV2").GetComponent<Animator>();
            animator.SetBool("Down0", !up);
            animator.SetBool("Up0", up);
            animator = GameObject.Find("SettingsV2").GetComponent<Animator>();
            animator.SetBool("Down0", !up);
            animator.SetBool("Up0", up);
            animator = GameObject.Find("Logo").GetComponent<Animator>();
            animator.SetBool("Down0", !up);
            animator.SetBool("Up0", up);
        }

        #region Play
        public void PressedPlay()
        {
            if (pressedSettings)
                PressedSettingsClose();
            pressedPlay = !pressedPlay;
            switch (pressedPlay)
            {
                case true:
                    PressedMenuUp(true);
                    Animator anim0 = GameObject.Find("TextPanel0").GetComponent<Animator>();
                    anim0.SetBool("Down0", false);
                    anim0.SetBool("Up0", true);
                    break;
                case false:
                    PressedPlayClose();
                    break;
            }
        }
        public void PressedPlayNew()
        {
            Animator anim0 = GameObject.Find("TextPanel0").GetComponent<Animator>();
            Animator anim1 = GameObject.Find("TextPanel1").GetComponent<Animator>();
            anim0.SetBool("Down1", false);
            anim0.SetBool("Up1", true);
            anim1.SetBool("Down0", false);
            anim1.SetBool("Up0", true);
        }
        public void PressedPlayNewNo()
        {
            Animator anim0 = GameObject.Find("TextPanel0").GetComponent<Animator>();
            Animator anim1 = GameObject.Find("TextPanel1").GetComponent<Animator>();
            anim1.SetBool("Down0", true);
            anim1.SetBool("Up0", false);
            anim0.SetBool("Down1", true);
            anim0.SetBool("Up1", false);
            anim0.SetBool("Down0", false);
            anim0.SetBool("Up0", false);
        }
        public void PressedPlayClose()
        {
            if (pressedPlay)
                pressedPlay = !pressedPlay;
            PressedMenuUp(false);
            Animator anim0 = GameObject.Find("TextPanel0").GetComponent<Animator>();
            Animator anim1 = GameObject.Find("TextPanel1").GetComponent<Animator>();
            anim0.SetBool("Down1", true);
            anim0.SetBool("Up1", false);
            anim0.SetBool("Down0", true);
            anim0.SetBool("Up0", false);
            anim1.SetBool("Down0", true);
            anim1.SetBool("Up0", false);
        }
        public void PressedPlayNewYes()
        {
            SavingUtils.ResetTotalProgress(Data.Difficulty.Normal);
            SceneLoader.LoadCutScene(0, false);
        }
        public void PressedPlayNewExtra()
        {
            SavingUtils.ResetTotalProgress(Data.Difficulty.Hard);
            SceneLoader.LoadCutScene(0, false);
        }
        public void PressedPlayContinue()
        {
            SceneLoader.instance.LoadSceneFade(GameDataInit.data.sceneName, SceneLoader.screenFadeTime);
            CutScene.CutSceneInit.scenario = GameDataInit.data.currentScenario;
        }
        #endregion Play

        #region Settings
        public void PressedOnSettings()
        {
            if (pressedPlay)
                PressedPlayClose();
            pressedSettings = !pressedSettings;
            switch (pressedSettings)
            {
                case true:
                    PressedMenuUp(true);
                    Animator anim0 = GameObject.Find("ButtonsPanel0").GetComponent<Animator>();
                    anim0.SetBool("Left0", true);
                    anim0.SetBool("Right0", false);
                    break;
                case false:
                    PressedSettingsClose();
                    break;
            }
        }
        private void SettingsOpenAnimation(string panelName, string buttonName)
        {
            List<string> animNames = iconsAnimationNames.Where(el => !el.Equals(buttonName)).ToList();
            Animator panelAnimator = GameObject.Find(panelName).GetComponent<Animator>();
            Animator buttonAnimator = GameObject.Find(buttonName).GetComponent<Animator>();
            bool isCentred = buttonAnimator.GetBool("Center");
            PressedSettingsTabsClose();

            for (int i = 0; i < animNames.Count; i++)
            {
                Animator anim = GameObject.Find(animNames[i]).GetComponent<Animator>();
                if (anim.gameObject.transform.position.y < buttonAnimator.gameObject.transform.position.y)
                {
                    anim.SetBool("DownX", !isCentred);
                    anim.SetBool("DownBackX", isCentred);
                }
                else
                {
                    anim.SetBool("UpX", !isCentred);
                    anim.SetBool("UpBackX", isCentred);
                }
            }
            panelAnimator.SetBool("Left0", !isCentred);
            panelAnimator.SetBool("Right0", isCentred);
            buttonAnimator.SetBool("Back", isCentred);
            buttonAnimator.SetBool("Center", !isCentred);
        }
        private void SettingsCloseAnimation(string panelName)
        {
            Animator panelAnimator = GameObject.Find(panelName).GetComponent<Animator>();
            panelAnimator.SetBool("Left0", false);
            panelAnimator.SetBool("Right0", true);
            foreach (var el in iconsAnimationNames)
            {
                Animator buttonAnimator = GameObject.Find(el).GetComponent<Animator>();
                buttonAnimator.SetBool("Center", false);
                buttonAnimator.SetBool("Back", true);
                buttonAnimator.SetBool("DownBackX", true);
                buttonAnimator.SetBool("DownX", false);
                buttonAnimator.SetBool("UpBackX", true);
                buttonAnimator.SetBool("UpX", false);
            }
        }
        public void PressedSettingsClose()
        {
            if (pressedSettings)
                pressedSettings = !pressedSettings;
            Animator anim0 = GameObject.Find("ButtonsPanel0").GetComponent<Animator>();
            anim0.SetBool("Left0", false);
            anim0.SetBool("Right0", true);
            PressedSettingsTabsClose();
            PressedMenuUp(false);
        }
        private void PressedSettingsTabsClose()
        {
            PressedSettingsMusicClose();
            PressedSettingsLanguageClose();
            PressedSettingsVideoClose();
            PressedSettingsDevsClose();
        }

        #region Music
        public void PressedSettingsMusic()
        {
            SettingsOpenAnimation("ButtonsPanel1", "MusicV2");
        }
        public void PressedSettingsMusicClose()
        {
            SettingsCloseAnimation("ButtonsPanel1");
        }
        #endregion Music

        #region Language
        public void PressedSettingsLanguage()
        {
            SettingsOpenAnimation("ButtonsPanel2", "LanguageV2");
            MenuLanguageInit.instance.UpdateLanguageTab();
        }
        public void PressedSettingsLanguageClose()
        {
            SettingsCloseAnimation("ButtonsPanel2");
        }
        #endregion Language

        #region Video
        public void PressedSettingsVideo()
        {
            SettingsOpenAnimation("ButtonsPanel3", "VideoV2");
        }
        public void PressedSettingsVideoClose()
        {
            SettingsCloseAnimation("ButtonsPanel3");
        }
        #endregion Video

        #region Devs
        public void PressedSettingsDevs()
        {
            SettingsOpenAnimation("ButtonsPanel4", "DevelopersV2");
        }
        public void PressedSettingsDevsClose()
        {
            SettingsCloseAnimation("ButtonsPanel4");
        }
        #endregion Devs
        #endregion Settings
        #endregion methods
    }
}