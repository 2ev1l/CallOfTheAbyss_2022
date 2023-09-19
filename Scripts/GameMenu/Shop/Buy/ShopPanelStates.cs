using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace GameMenu.Shop
{
    public class ShopPanelStates : StateChange
    {
        [SerializeField] private Text txt;
        [SerializeField] private GameObject panel;
        public virtual string stateNameNormalized => gameObject.name.Remove(gameObject.name.Length - 5);

        public override void SetActive(bool active)
        {
            txt.color = active ? new Color(240 / 255f, 199 / 255f, 57 / 255f) : Color.white;
            txt.raycastTarget = !active;
            panel.SetActive(active);
        }
    }
}