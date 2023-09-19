using UnityEngine;

namespace Universal
{
    public abstract class DefaultUpdater : MonoBehaviour
    {
        protected abstract void OnEnable();
        protected abstract void OnDisable();
    }
}