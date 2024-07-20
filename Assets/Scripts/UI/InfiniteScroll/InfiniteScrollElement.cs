using UnityEngine;

namespace UI.InfiniteScroll
{
    public abstract class InfiniteScrollElement : MonoBehaviour
    {
        public RectTransform RectTransform;
        
        public float GetHeight()
        {
            return RectTransform.sizeDelta.y;
        }
        
        public abstract void ResetView();
    }
}