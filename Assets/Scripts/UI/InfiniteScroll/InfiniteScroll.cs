using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfiniteScroll
{
    public class InfiniteScroll<T, TData> : MonoBehaviour where T : InfiniteScrollElement<TData>
    {
        [SerializeField] private T _elementPrefab;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private float _space;

        private readonly Stack<T> _pull = new();
        private float _height;

        private List<ScrollData> _dataList = new();

        public void Init(List<TData> dataList)
        {
            _dataList = dataList.Select(x => new ScrollData()
            {
                Data = x
            }).ToList();
            _height = _elementPrefab.GetHeight();
            GenerateViewport();
            InstantiateElements(); 
            _scroll.onValueChanged.AddListener(OnScrollChanged);
        }

        private void OnScrollChanged(Vector2 arg0)
        {
            UpdateVisual();
        }

        private void GenerateViewport()
        {
            Vector2 size = _scroll.content.sizeDelta;
            size.y = ((_height + _space) * _dataList.Count) - _space;
            _scroll.content.sizeDelta = size;
        }

        private void InstantiateElements()
        {
            for (int i = 0; i < _dataList.Count; i++)
            {
                float targetYPos = -i * (_space + _height) - (_height / 2);
                _dataList[i].Pos = targetYPos;
            }
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            float maxPos = (_height / 2) - _scroll.content.anchoredPosition.y;
            float minPos = -(_height / 2) - _scroll.viewport.rect.height - _scroll.content.anchoredPosition.y;
            
            for (int i = 0; i < _dataList.Count; i++)
            {
                ScrollData data = _dataList[i];
                if ((data.Pos > maxPos || data.Pos < minPos) && data.IsEnabled && data.Presenter != null)
                {
                    ReturnElement(data.Presenter);
                    data.Presenter = null;
                    data.IsEnabled = false;
                }
                else if ((data.Pos < maxPos && data.Pos > minPos) && !data.IsEnabled)
                {
                    data.Presenter = GetElement();
                    data.IsEnabled = true;
                    Vector2 pos = data.Presenter.RectTransform.anchoredPosition;
                    pos.y = data.Pos;
                    data.Presenter.RectTransform.anchoredPosition = pos;
                    data.Presenter.Init(data.Data);
                }
            }
        }

        private void ReturnElement(T element)
        {
           element.ResetView(); 
           element.gameObject.SetActive(false);
           _pull.Push(element);
        }

        private T GetElement()
        {
            T res;
            if (_pull.Count == 0)
            {
                res = Instantiate(_elementPrefab, _scroll.content);
            }
            else
            {
                res = _pull.Pop();
                res.gameObject.SetActive(true);
            }

            return res;
        }

        private class ScrollData
        {
            public T Presenter;
            public TData Data;
            public bool IsEnabled;
            public float Pos;
        }
    }
}
