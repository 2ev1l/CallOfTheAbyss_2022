using UnityEngine;
using UnityEngine.EventSystems;

namespace Universal
{
    public abstract class ShowObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region fields
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2 offset;
        [SerializeField] private bool ignoreCoordinates = false;
        [SerializeField] private Vector2 customCoordinates = Vector2.one;
        #endregion fields

        #region methods
        public virtual GameObject SpawnObject()
        {
            GameObject obj = Instantiate(prefab, ObjectsUpdate.instance.transform);
            obj.transform.localScale *= CustomMath.GetOptimalScreenScale();
            ObjectsUpdate.ignoreCoordinates = ignoreCoordinates;
            ObjectsUpdate.customOffset = customCoordinates;
            ObjectsUpdate.offset = offset * CustomMath.GetOptimalScreenScale();
            return obj;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!SceneLoader.IsBlackScreenFade())
                SpawnObject();
            else
                DestroyChild();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            DestroyChild();
        }
        protected void DestroyChild()
        {
            Transform objectsUpdateTransform = ObjectsUpdate.instance.transform;
            for (int i = 0; i < objectsUpdateTransform.childCount; i++)
            {
                GameObject childObject = objectsUpdateTransform.GetChild(i).gameObject;
                if (childObject != null)
                    Destroy(childObject);
            }
        }
        #endregion methods
    }
}