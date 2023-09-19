using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using GameFight.Card;
using Data;
using Universal;
using GameMenu.Inventory.Cards.Builder;

namespace GameMenu.Inventory.Cards
{
    [RequireComponent(typeof(LanguageLoadsCardUpdater))]
    public class CardMenuInit : CursorChange, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ItemList.IListUpdater
    {
        #region fields & properties
        public UnityAction<List<CardData>, int, int> OnValuesUpdate;
        public UnityAction OnCardClick;
        public UnityAction OnCardEnter;
        public UnityAction OnCardExit;
        public int id { get; private set; }
        public int listPosition { get; protected set; }
        public bool isHPZero => GameDataInit.data.cardsData[listPosition].hp <= 0;
        [SerializeField] protected GameObject onSelect;
        [SerializeField] protected GameObject statsPanel;

        public CardInfoSO cardInfo { get; private set; }
        public GameObject rootObject => gameObject;
        public int listParam => listPosition;

        private static readonly List<string> excludedNames = new List<string>() { "TrashTimer", "DeleteIcon", "EventText_1", "EventText_2" };
        #endregion fields & properties

        #region methods
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !CanEnterPoint(eventData)) return;
            onSelect.SetActive(statsPanel.activeSelf);
            statsPanel.SetActive(!statsPanel.activeSelf);
            OnCardClick?.Invoke();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            CursorChangeEvent(eventData, true);
            onSelect.SetActive(true);
            OnCardEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            CursorChangeEvent(eventData, false);
            HideSelectedPanels();
            OnCardExit?.Invoke();
        }
        protected void CursorChangeEvent(PointerEventData eventData, bool onPointerEnter)
        {
            if (onPointerEnter)
                base.OnPointerEnter(eventData);
            else
                base.OnPointerExit(eventData);
        }
        protected virtual bool CanEnterPoint(PointerEventData eventData)
        {
            try { return gameObject != null && !excludedNames.Contains(eventData.pointerCurrentRaycast.gameObject.name); }
            catch { return false; }
        }
        protected virtual void Start()
        {
            StartCoroutine(UpdateValue());
        }
        private IEnumerator UpdateValue()
        {
            yield return CustomMath.WaitAFrame();
            UpdateValues();
        }
        public bool HasDescription() => (CardFightInit.description[id] != "" && CardFightInit.description[id] != " ");
        public void ChangeID(int id)
        {
            this.id = id;
            UpdateValues();
        }
        public void ChangeListPosition(int listPosition)
        {
            if (listPosition < 0)
                gameObject.SetActive(false);
            this.listPosition = listPosition;
            UpdateValues();
        }
        protected virtual void UpdateValues() => UpdateValues(GameDataInit.data.cardsData);
        protected virtual void UpdateValues(List<CardData> cardDatas)
        {
            if (id == -1) return;
            id = cardDatas[listPosition].id;
            cardInfo = PrefabsData.instance.cardPrefabs[id];
            OnValuesUpdate?.Invoke(cardDatas, listPosition, id);
        }
        public void HideSelectedPanels()
        {
            statsPanel.SetActive(false);
            onSelect.SetActive(false);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            onSelect.SetActive(false);
            statsPanel.SetActive(false);
        }
        protected void IncTutorial()
        {
            if (GameDataInit.data.tutorialProgress != 4 && GameDataInit.data.tutorialProgress != 2) return;
            GameDataInit.data.tutorialProgress++;
            GameMenu.Tutorial.GameMenuTutorialInit.instance.tutorialProgresses.Find(x => x.id == GameDataInit.data.tutorialProgress).Init();
        }

        public void OnListUpdate(int param) => ChangeListPosition(param);
        #endregion methods
    }
    public enum CardPlaceType { Inventory, Trash }
}