using UnityEngine;
using Universal;

namespace GameFight.Equipment
{
    public class EquipmentPanelInit : SingleSceneInstance
    {
        #region fields
        public static EquipmentPanelInit instance { get; private set; }

        [SerializeField] private ItemList potionList;
        [SerializeField] private ItemList artifactList;
        [SerializeField] private Animator[] panelAnimators;
        [SerializeField] private GameObject fakeRayCast;
        private bool isPanelOpened;
        #endregion fields

        #region methods
        protected override void Awake()
        {
            instance = this;
            CheckPanelAvailability();
            CheckInstances(GetType());
            UpdatePotionsList();
            UpdateArtifactsList();
        }
        private void OnEnable()
        {
            StageEndInit.instance.OnStageEnded += delegate { TryClosePanel(false); FightPotion.TryDeselectPotion(); };
        }
        private void OnDisable()
        {
            StageEndInit.instance.OnStageEnded -= delegate { TryClosePanel(false); FightPotion.TryDeselectPotion(); };
        }
        public void CheckPanelAvailability() => instance.gameObject.SetActive(GameDataInit.deskPotions.Count != 0 || GameDataInit.deskArtifacts.Count != 0);
        public void TryClosePanel(bool checkOnFightAnimation)
        {
            if (!isPanelOpened || (checkOnFightAnimation && FightAnimationInit.isFightAnimation)) return;
            isPanelOpened = false;
            DoPanelAnimation();
        }
        private void ChangePanelState()
        {
            isPanelOpened = !isPanelOpened;
            DoPanelAnimation();
        }
        private void DoPanelAnimation()
        {
            fakeRayCast.SetActive(isPanelOpened);
            string panelAnimName = isPanelOpened ? "Open" : "Close";
            foreach (var el in panelAnimators)
            {
                el.Play(panelAnimName);
            }
        }
        public void PressedChangePanelState()
        {
            if (FightAnimationInit.isFightAnimation) return;
            ChangePanelState();
            CursorSettings.instance.DoClickSound();
        }
        public void UpdatePotionsList() => potionList.UpdateListData();
        private void UpdateArtifactsList() => artifactList.UpdateListData();
        #endregion methods
    }
}