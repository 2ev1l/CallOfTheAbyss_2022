using UnityEngine;
using UnityEngine.UI;
using Universal;
using Data;
using System.Collections;

namespace GameMenu.Inventory
{
    public class InventoryRemoveAllButtonUpdater : DefaultUpdater
    {
        [SerializeField] private Text mainText;
        [SerializeField] private LanguageLoad textLoad;

        protected override void OnEnable()
        {
            InventoryPanelInit.instance.OnDeskSizeChanged += ValidateButton;
        }

        protected override void OnDisable()
        {
            InventoryPanelInit.instance.OnDeskSizeChanged -= ValidateButton;
        }
        private void ValidateButton()
        {
            bool isDeskCardsEmpty = GameDataInit.deskCards.Count == 0;
            mainText.enabled = !isDeskCardsEmpty;
            if (!isDeskCardsEmpty)
                StartCoroutine(LoadText());
        }
        private IEnumerator LoadText()
        {
            yield return CustomMath.WaitAFrame();
            textLoad.Load();
        }
    }
}