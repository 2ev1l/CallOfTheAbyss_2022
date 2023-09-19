using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Universal
{
    public abstract class ItemList : MonoBehaviour
    {
        #region fields & properties
        private List<IListUpdater> _items = new List<IListUpdater>();
        private List<IListUpdater> _currentPageItems = new List<IListUpdater>();
        public IEnumerable<IListUpdater> items => _items;
        [SerializeField] private GameObject[] positions;
        [SerializeField] private GameObject arrowNext;
        [SerializeField] private GameObject arrowPrev;
        [SerializeField] private Component prefabRoot;
        [SerializeField] private Transform parentForSpawn;
        private int pageCounter;
        #endregion fields & properties

        #region methods
        public void Remove(IListUpdater listUpdater, bool init = false, bool initAtLastPage = false)
        {
            if (DisableFunction()) return;
            Destroy(listUpdater.rootObject);
            int index = _items.IndexOf(listUpdater);
            _items.RemoveAt(index);
            if (init)
                Init(initAtLastPage);
        }
        public void RemoveAt(int id, bool init = false, bool initAtLastPage = false, bool destroyObject = true)
        {
            if (DisableFunction()) return;
            if (destroyObject)
                Destroy(_items[id].rootObject);
            _items.RemoveAt(id);
            if (init)
                Init(initAtLastPage);
        }
        public void RemoveAtListParam(int listParam, bool init = false, bool initAtLastPage = false, bool destroyObject = false) => RemoveAt(_items.FindIndex(obj => obj.listParam == listParam), init, initAtLastPage, destroyObject);
        public void RemoveAtLastListParam(int listParam, bool init = false, bool initAtLastPage = false, bool destroyObject = false) => RemoveAt(_items.FindLastIndex(obj => obj.listParam == listParam), init, initAtLastPage, destroyObject);
        public void RemoveRange(int start, int end, bool init = false, bool initAtLastPage = false)
        {
            if (DisableFunction() || start > _items.Count - 1) return;
            end = Mathf.Min(end, _items.Count - 1);
            int removeCount = end - start + 1;
            while (true)
            {
                try
                {
                    RemoveAt(start);
                    removeCount--;
                    if (removeCount <= 0) break;
                }
                catch { Debug.LogError("index was out of range"); print(_items.Count); print(end); break; }
            }

            if (init)
                Init(initAtLastPage);
        }
        public void RemoveAll(bool init = false, bool initAtLastPage = false)
        {
            if (DisableFunction()) return;
            DestroyObjects(_items);
            _items = new List<IListUpdater>();
            if (init)
                Init(initAtLastPage);
        }
        public void Add(IListUpdater listUpdater, bool init = false, bool initAtLastPage = false)
        {
            if (DisableFunction()) return;
            _items.Add(listUpdater);
            if (init)
                Init(initAtLastPage);
        }
        public void AddAt(IListUpdater listUpdater, int index, bool init = false, bool initAtLastPage = false)
        {
            if (DisableFunction()) return;
            _items.Insert(index, listUpdater);
            if (init)
                Init(initAtLastPage);
        }
        public void SwitchPage(bool isNext)
        {
            if (DisableFunction()) return;
            PageSwitch(isNext);
        }
        public void UpdateCurrentPage()
        {
            foreach (var el in _currentPageItems)
                el.OnListUpdate(el.listParam);
        }

        private void Init(bool atLastPage = false)
        {
            if (DisableFunction()) return;
            if (!atLastPage)
                pageCounter = -1;
            else
            {
                int oversizeCount = _items.Count - positions.Length * (pageCounter + 1);
                if (oversizeCount <= 0)
                    while (oversizeCount <= 0)
                    {
                        pageCounter--;
                        oversizeCount = _items.Count - positions.Length * (pageCounter + 1);
                    }
                else if (pageCounter >= 0)
                    pageCounter -= 1;
            }
            TryInitArrowsUI();
            SwitchPage(true);
        }
        private void TryInitArrowsUI()
        {
            if (!IsArrowsEnabled()) return;
            arrowPrev.SetActive(false);
            arrowNext.SetActive(false);
        }
        private void DestroyObjects(List<IListUpdater> list)
        {
            foreach (IListUpdater el in list)
                Destroy(el.rootObject);
        }
        private void ClearPositions()
        {
            foreach (var el in positions)
                for (int i = 0; i < el.transform.childCount; i++)
                    el.transform.GetChild(i).gameObject.SetActive(false);
        }
        private void TrySetPositions(int count)
        {
            if (_items.Count == 0) return;
            int endValue = positions.Length * pageCounter + count - 1;
            _currentPageItems = new List<IListUpdater>();
            for (int i = positions.Length * pageCounter; i <= endValue; i++)
            {
                SetItemVisible(i);
                _currentPageItems.Add(_items[i]);
            }
            AfterPositionsSet(_currentPageItems);
        }
        private void SetItemVisible(int id)
        {
            Transform itemTransform = _items[id].rootObject.transform;
            Vector3 oldScale = itemTransform.localScale;
            itemTransform.SetParent(positions[id % positions.Length].transform);
            itemTransform.localScale = oldScale;
            itemTransform.localPosition = Vector3.zero;
            _items[id].rootObject.SetActive(true);
        }
        protected virtual void AfterPositionsSet(List<IListUpdater> currentPositions) { }
        private void PageSwitch(bool isNext)
        {
            ClearPositions();
            int oversizeCount = 0;
            oversizeCount = isNext ? TryIncreasePage(oversizeCount) : TryDecreasePage(oversizeCount);
            TrySetArrowsUI(oversizeCount);
        }
        private int TryIncreasePage(int oversizeCount)
        {
            oversizeCount = _items.Count - positions.Length * (pageCounter + 1);
            pageCounter++;
            if (oversizeCount <= positions.Length)
            {
                for (int i = 1 + Mathf.Abs(oversizeCount); i < positions.Length; i++)
                    positions[i].SetActive(false);
                for (int i = 0; i < Mathf.Abs(oversizeCount); i++)
                    positions[i].SetActive(true);
                TrySetPositions(Mathf.Abs(oversizeCount));
            }
            else
            {
                foreach (var el in positions)
                    el.SetActive(true);
                TrySetPositions(positions.Length);
            }
            return oversizeCount;
        }
        private int TryDecreasePage(int oversizeCount)
        {
            oversizeCount = _items.Count - positions.Length * (pageCounter - 1);
            foreach (var el in positions)
                el.SetActive(true);
            pageCounter--;
            TrySetPositions(positions.Length);
            return oversizeCount;
        }
        private void TrySetArrowsUI(int oversizeCount)
        {
            if (!IsArrowsEnabled()) return;
            arrowPrev.SetActive(pageCounter > 0);
            arrowNext.SetActive(oversizeCount > positions.Length);

            if (arrowNext.transform.childCount > 0 && arrowPrev.transform.childCount > 0)
            {
                arrowNext.transform.GetChild(0).GetComponent<Text>().text = $"{pageCounter + 1}";
                arrowPrev.transform.GetChild(0).GetComponent<Text>().text = $"{pageCounter - 1}";
            }
        }
        
        private GameObject UpdateObject(int param, IListUpdater listUpdater)
        {
            bool currentObjectState = listUpdater.rootObject.activeSelf;
            listUpdater.rootObject.SetActive(true);
            listUpdater.OnListUpdate(param);
            listUpdater.rootObject.SetActive(currentObjectState);
            return listUpdater.rootObject;
        }
        public GameObject GetUpdatedObject(int param, out IListUpdater iListUpdater)
        {
            GameObject obj = GetDefaultObject(prefabRoot, out IListUpdater listUpdater);
            obj = UpdateObject(param, listUpdater);
            iListUpdater = listUpdater;
            return obj;
        }
        private GameObject GetDefaultObject(Component prefabRoot, out IListUpdater listUpdater)
        {
            listUpdater = Instantiate(prefabRoot, Vector3.one * 100, Quaternion.identity, parentForSpawn) as IListUpdater;
            listUpdater.rootObject.hideFlags = HideFlags.HideInHierarchy;
            return listUpdater.rootObject;
        }
        public abstract void UpdateListData();
        protected void UpdateListDefault<T>(List<T> newList, System.Func<T, int> updateMatch)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                if (i < _items.Count)
                    UpdateObject(updateMatch.Invoke(newList[i]), _items[i]);
                else
                {
                    GetUpdatedObject(updateMatch.Invoke(newList[i]), out IListUpdater listUpdater);
                    Add(listUpdater);
                }
            }
            RemoveRange(newList.Count, _items.Count - 1);
            Init(true);
        }
        private bool DisableFunction()
        {
            return (!gameObject.activeSelf || !gameObject.activeInHierarchy);
        }
        private bool IsArrowsEnabled() => (arrowNext.gameObject != null && arrowPrev.gameObject != null);
        #endregion methods

        public interface IListUpdater
        {
            public GameObject rootObject { get; }
            public int listParam { get; }
            public void OnListUpdate(int param);
        }
    }
}