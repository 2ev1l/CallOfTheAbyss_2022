using UnityEngine;
using UnityEngine.EventSystems;

namespace Universal
{
    public class CursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region methods
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (SceneLoader.IsBlackScreenFade())
                return;
            CursorSettings.instance.SetPointCursor();
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            CursorSettings.instance.SetDefaultCursor();
        }
        protected virtual void OnDisable()
        {
            CursorSettings.instance.SetDefaultCursor();
        }
        #endregion methods
    }
}