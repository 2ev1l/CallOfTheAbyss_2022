using UnityEngine.EventSystems;

namespace GameMenu.Inventory.Cards
{
    public class MainPreviewCardMenuInit : CardMenuInit
    {
        public static MainPreviewCardMenuInit instance { get; private set; }
        
        #region methods
        protected override void OnDisable()
        {
            onSelect.SetActive(false);
            PreviewCardMenuInit.OnChoosedCardChange -= UpdateAfterPreviewChange;
        }
        private void OnEnable()
        {
            PreviewCardMenuInit.OnChoosedCardChange += UpdateAfterPreviewChange;
        }
        private void UpdateAfterPreviewChange()
        {
            instance = this;
            ChangeListPosition(PreviewCardMenuInit.choosedCardToPreview.listPosition);
            InventoryCardsParticles.instance.SpawnChooseAltParticlesAt(transform.position);
        }
        public override void OnPointerClick(PointerEventData eventData) { }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData) || !HasDescription() || !eventData.pointerCurrentRaycast.gameObject.name.Equals("Picture")) return;
            onSelect.SetActive(true);
            OnCardEnter?.Invoke();
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            onSelect.SetActive(false);
            OnCardExit?.Invoke();
        }
        #endregion methods
    }
}