using UnityEngine;
using UnityEngine.Events;
using GameFight.Card;
using Universal;
using Data;

namespace GameMenu
{
    public sealed class GameMenuInit : HelpUpdate
    {
        #region fields & properties
        public UnityAction OnHouseSizeChanged;
        public static GameMenuInit instance;
        [SerializeField] private GameObject[] onStartEnable;
        [field: SerializeField] public GameObject[] upperPanels { get; private set; }
        #endregion fields & properties

        #region methods
        private void Start()
        {
            foreach (var el in onStartEnable)
                el.SetActive(true);
        }
        protected override void Awake()
        {
            base.Awake();
            CardFightInit.description = GameDataInit.GetCardsDescription();
            instance = this;
        }
        public void DefaultPanelAnimation(Animator anim) => anim.Play("Shop-Down0");
        #endregion methods
        [ContextMenu("aa")]
        private void aaa()
        {
            GameDataInit.data.reachedLocation = 3;
            GameDataInit.data.currentLocationMax = 3;
            GameDataInit.AddSilver(-700, false);
            GameDataInit.AddGold(-130, false);
        }
        [ContextMenu("test")]
        private void test()
        {
            GameDataInit.data.artifactsData = new System.Collections.Generic.List<ArtifactData>();
            GameDataInit.data.cardsData = new System.Collections.Generic.List<CardData>();
            GameDataInit.data.potionsData = new System.Collections.Generic.List<PotionData>();
            GameDataInit.data.chestsData = new System.Collections.Generic.List<int>();
            GameDataInit.data.maxDeskSize = 6;
            GameDataInit.data.maxInventorySize = 25;
            GameDataInit.data.maxHandSize = 3;
            for (int i = 0; i < 81; i++)
            {
                GameDataInit.AddCard(i, false);
            }
            for (int i = 0; i < 21; i++)
            {
                GameDataInit.AddChest(i, false);
            }
            for (int i = 0; i < 22; i++)
            {
                GameDataInit.AddPotion(i, false);
            }
            for (int i = 0; i < 16; i++)
            {
                GameDataInit.AddArtifact(i, false);
            }
            GameDataInit.AddSilver(731, false);
            GameDataInit.AddGold(142, false);
        }
    }
    public enum InventoryTabType { Cards, Chests, Potions, Artifacts, Preview, Trash }
}