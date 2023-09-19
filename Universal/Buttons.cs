using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Universal
{
    public class Buttons : CursorChange, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        #region methods
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!CanEnterPoint(eventData)) return;
            CursorSettings.instance.DoClickSound();
        }
        protected void CursorEnterEvent(PointerEventData eventData) => base.OnPointerEnter(eventData);
        protected void CursorExitEvent(PointerEventData eventData) => base.OnPointerExit(eventData);
        protected virtual bool CanEnterPoint(PointerEventData eventData) => eventData.button == PointerEventData.InputButton.Left;
        public void PressedExit()
        {
            Application.Quit();
        }
        public void PressedSettings()
        {
            SceneLoader.instance.LoadSceneFade("Menu", SceneLoader.screenFadeTime);
        }
        #endregion methods
    }
}