using UnityEngine;
using Universal;

namespace GameMenu.House
{
    public class HouseEmptyItem : MonoBehaviour, ItemList.IListUpdater
    {
        public GameObject rootObject => gameObject;

        public int listParam => 0;

        public void OnListUpdate(int param) { }
    }
}