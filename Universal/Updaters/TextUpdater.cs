using UnityEngine;
using UnityEngine.UI;

namespace Universal
{
    public abstract class TextUpdater : DefaultUpdater
    {
        [SerializeField] protected string defaultText;
        [SerializeField] protected TextPosition textPosition;
        [SerializeField] protected Text txt;
        [SerializeField] protected bool isEventsEnabled = true;

        protected void SetDefaultText<T>(T item)
        {
            ResetText();
            if (textPosition == TextPosition.Before)
                txt.text += $"{item}";
            txt.text += $"{defaultText}";
            if (textPosition == TextPosition.After)
                txt.text += $"{item}";
        }
        public void AddText(string text) => txt.text += text;
        protected void ResetText() => txt.text = "";

        public enum TextPosition { Before, After }
    }
}