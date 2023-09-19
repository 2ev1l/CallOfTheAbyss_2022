using UnityEngine;

namespace Universal
{
    public class InvokeNext : MonoBehaviour
    {
        #region fields
        [SerializeField] private GameObject next;
        [SerializeField] private float time;
        #endregion fields

        #region methods
        private void Start() => Invoke(nameof(Next), time);
        private void Next()
        {
            next.SetActive(true);
            gameObject.SetActive(false);
        }
        #endregion methods
    }
}