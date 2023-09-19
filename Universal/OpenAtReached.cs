using System.Collections;
using UnityEngine;

namespace Universal
{
    public class OpenAtReached : MonoBehaviour
    {
        [SerializeField] private int location;
        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            if (location > GameDataInit.data.reachedLocation)
                gameObject.SetActive(false);
        }
    }
}