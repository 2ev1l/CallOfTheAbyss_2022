using Data;
using System.Collections.Generic;
using System.Linq;
using Universal;

namespace GameMenu.Blacksmith
{
    public class BlacksmithItemList : ItemList
    {
        #region methods
        public override void UpdateListData()
        {
            List<CardData> cardDatas = GameDataInit.data.cardsData.Where(x => !x.onDesk && !x.onHeal).ToList();
            UpdateListDefault(cardDatas, x => x.listPosition);
        }
        #endregion methods
    }
}