using UnityEngine;
using Data;
using Universal;

namespace GameFight.Card
{
    public sealed class CardFightLanguageUpdater : DefaultUpdater
    {
        [SerializeField] private CardFightInit cardFightInit;
        [SerializeField] private LanguageLoad nameText;
        [SerializeField] private LanguageLoad onSelectText;
        
        protected override void OnEnable()
        {
            cardFightInit.UpdateCardUI += UpdateLanguage;
        }
        protected override void OnDisable()
        {
            cardFightInit.UpdateCardUI -= UpdateLanguage;
        }
        private void UpdateLanguage(bool isEnemy)
        {
            onSelectText.ChangeID(cardFightInit.id);
            nameText.ChangeID(cardFightInit.id);
        }
    }
}