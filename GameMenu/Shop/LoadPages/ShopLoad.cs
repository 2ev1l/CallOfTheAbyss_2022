using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Universal;

namespace GameMenu.Shop
{
    public class ShopLoad : MonoBehaviour
    {
        #region fields & properties
        [field: SerializeField] public List<ShopObject> positions { get; private set; }
        #endregion fields & properties

        #region methods
        private IEnumerator Start()
        {
            yield return CustomMath.WaitAFrame();
            UpdateTab();
        }
        public virtual void UpdateTab() { }
        protected void DefaultTabs(List<ShopData> list, int c = 0)
        {
            if (list.Count == 0)
            {
                RemoveShopData(c);
                return;
            }
            for (int i = c; i < list.Count; i++)
            {
                DefaultTab(list, i);
                c++;
            }
            RemoveShopData(c);
        }
        protected void DefaultTab(List<ShopData> list, int index)
        {
            positions[index].shopData = list[index];
            positions[index].indexPosition = GameDataInit.data.shopData.IndexOf(list[index]);
            if (positions[index].CanInit())
                positions[index].Init();
        }
        protected void DefaultTab(ShopData element, int index)
        {
            positions[index].shopData = element;
            positions[index].indexPosition = GameDataInit.data.shopData.IndexOf(element);
            if (positions[index].CanInit())
                positions[index].Init();
        }
        protected void RemoveShopData(int startIndex)
        {
            for (int i = startIndex; i < positions.Count; i++)
            {
                positions[i].shopData = new ShopData();
                positions[i].indexPosition = 0;
                if (positions[i].CanInit())
                    positions[i].Init();
            }
        }
        #endregion methods
    }
}