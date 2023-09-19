using System.Linq;
using UnityEngine;
using Universal;
using System.Collections.Generic;
using Data;

namespace GameMenu.House
{
    public class HouseCardsHealList : ItemList
    {
        #region fields
        [SerializeField] private Component healEmptyRoot;
        #endregion fields

        #region methods
        private void OnEnable()
        {
            GameDataInit.instance.OnCardRemoved += StopUpdate;
        }
        private void OnDisable()
        {
            GameDataInit.instance.OnCardRemoved -= StopUpdate;
        }
        private void StopUpdate() => UpdateListDefault(new List<CardData>(), x => x.listPosition);
        public override void UpdateListData()
        {
            List<CardData> cardsData = GameDataInit.data.cardsData.Where(card => card.onHeal && !card.onDesk).ToList();
            UpdateListDefault(cardsData, x => x.listPosition);
            TryAddHealPrefab();
            HousePanelInit.instance.OnHouseSizeChanged?.Invoke();
        }
        public void TryAddHealPrefab()
        {
            if (items.Count() < GameDataInit.data.maxHouseSize)
            {
                GetHouseHealEmptyPrefab(out IListUpdater listUpdater);
                Add(listUpdater, true, true);
            }
        }
        public GameObject GetHouseHealEmptyPrefab(out IListUpdater listUpdater)
        {
            listUpdater = Instantiate(healEmptyRoot) as IListUpdater;
            listUpdater.rootObject.hideFlags = HideFlags.HideInHierarchy;
            return listUpdater.rootObject;
        }
        protected override void AfterPositionsSet(List<IListUpdater> currentPositions)
        {
            foreach (IListUpdater el in currentPositions)
            {
                el.OnListUpdate(el.listParam);

                if (el.rootObject.TryGetComponent(out HouseCardMenuInit cardMenuInit))
                    cardMenuInit.UpdateHouseTime();
            }
        }
        #endregion methods
    }
}