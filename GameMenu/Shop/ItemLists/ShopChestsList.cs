using UnityEngine;
using Universal;

namespace GameMenu.Shop
{
    public class ShopChestsList : ItemList
    {
        public override void UpdateListData()
        {
            UpdateListDefault(GameDataInit.data.chestsData, x => x);
        }
    }
}