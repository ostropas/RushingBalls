using UnityEngine;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour
    {
        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
           Destroy(gameObject);
        }
    }
}
