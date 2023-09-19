using Data;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace GameEvent
{
    public class EventVariations : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private int defaultVariationID;
        [SerializeField] private LanguageLoad mainText;
        [SerializeField] private List<ButtonText> buttonsEvent;
        [SerializeField] private List<EventVariation> eventVariations;

        private int variationID = 0;
        #endregion fields & properties

        #region methods
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);

            if (state)
                Init();
        }
        private void Init()
        {
            variationID = defaultVariationID;
            mainText.ChangeID(eventVariations[defaultVariationID].textID);
            foreach (var el in buttonsEvent)
                el.LoadButton();
            OnInit(defaultVariationID);
        }
        public void SendAnswer(LanguageLoad buttonLanguage) => SendAnswer(GameEventInit.instance.answersLanguageInfo.Find(x => x.textID == buttonLanguage.ID).answer);
        protected void SendAnswer(VariationAnswer answer)
        {
            EventVariation currentVariation = eventVariations.Find(x => x.id == variationID);
            List<NextVariation> nextVariations = currentVariation.next.FindAll(x => x.answer == answer);
            NextVariation choosedVariation = nextVariations[Random.Range(0, nextVariations.Count)];

            if (eventVariations.Find(x => x.id == choosedVariation.id).next.Count > 0)
                ChangeEvent(choosedVariation, answer);
            else
                ChangeEventEnd(choosedVariation, answer);
        }
        protected void ChangeVariationID(int id)
        {
            variationID = id;
            EventVariation currentVariation = GetVariationByID();
            mainText.ChangeID(currentVariation.textID);
            if (currentVariation.next.Count > 0)
            {
                foreach (var el in buttonsEvent)
                    el.button.gameObject.SetActive(true);
                OnEventChanged(variationID, VariationAnswer.Yes);
            }
            else
                OnEventEnd(variationID, VariationAnswer.Yes);
        }
        private void ChangeEvent(NextVariation nextVariation, VariationAnswer answer)
        {
            variationID = nextVariation.id;
            mainText.ChangeID(GetVariationByID().textID);
            OnEventChanged(variationID, answer);
        }
        private void ChangeEventEnd(NextVariation nextVariation, VariationAnswer answer)
        {
            ChangeEvent(nextVariation, answer);
            foreach (var el in buttonsEvent)
                el.button.gameObject.SetActive(false);
            OnEventEnd(variationID, answer);
        }
        private EventVariation GetVariationByID(int id) => eventVariations.Find(x => x.id == id);
        private EventVariation GetVariationByID() => eventVariations.Find(x => x.id == variationID);
        protected virtual void OnInit(int defaultVariationID) { }
        protected virtual void OnEventChanged(int currentVaritationID, VariationAnswer answer) { }
        protected virtual void OnEventEnd(int finalVaritationID, VariationAnswer answer) { }
        protected void Leave()
        {
            SceneLoader.instance.LoadSceneFade("GameAdventure", SceneLoader.screenFadeTime);
        }
        protected VariationState<T> GetVariationState<T>(List<VariationState<T>> variationStates, int variationID) => variationStates.Find(x => x.id == variationID);
        protected VariationState<T, T1> GetVariationState<T, T1>(List<VariationState<T, T1>> variationStates, int variationID) => variationStates.Find(x => x.id == variationID);
        #endregion methods

        [System.Serializable]
        protected class EventVariation
        {
            [field: Header("Current variation")]
            [field: SerializeField] public int id { get; private set; }
            [field: SerializeField] public int textID { get; private set; }
            [SerializeField] private string note;
            [field: SerializeField] public List<NextVariation> next { get; private set; } = new List<NextVariation>();
        }

        [System.Serializable]
        protected class NextVariation
        {
            [field: SerializeField] public int id { get; private set; }
            [field: SerializeField] public VariationAnswer answer { get; private set; }
        }

        [System.Serializable]
        protected class ButtonText
        {
            [field: SerializeField] public VariationAnswer buttonAnswer { get; private set; }
            [field: SerializeField] public LanguageLoad button { get; private set; }
            public void LoadButton()
            {
                button.ChangeID(GameEventInit.instance.answersLanguageInfo.Find(x => x.answer == buttonAnswer).textID);
            }
        }

        [System.Serializable]
        protected class VariationState<T>
        {
            [field: SerializeField] public int id { get; private set; }
            [field: SerializeField] public T item1 { get; private set; }
        }
        [System.Serializable]
        protected class VariationState<T, T1> : VariationState<T>
        {
            [field: SerializeField] public T1 item2 { get; private set; }
        }
    }
    public enum VariationAnswer { Yes, No, Wait, Leave, Left, Straight, Right }
}