using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameMenu.Tutorial
{
    public sealed class GameMenuTutorialInit : SingleSceneInstance
    {
        public static GameMenuTutorialInit instance;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject[] tutorialDisabled;
        [field: SerializeField] public List<TutorialProgress> tutorialProgresses { get; private set; }

        protected override void Awake()
        {
            instance = this;
            CheckInstances(GetType());
        }
        private void Start()
        {
            if (GameDataInit.data.isTutorialCompleted) return;

            tutorialPanel.SetActive(true);
            foreach (var el in tutorialDisabled)
                el.SetActive(false);
            tutorialProgresses.Find(x => x.id == GameDataInit.data.tutorialProgress).Init();
        }
        public void TutorialProgressInit()
        {
            if (GameDataInit.data.tutorialProgress < 4)
                tutorialProgresses[4].Init();
            else if (GameDataInit.data.tutorialProgress < 7)
                tutorialProgresses[7].Init();
        }
    }
}