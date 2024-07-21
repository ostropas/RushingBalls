using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour
    {
        public virtual Task Show()
        {
            return Task.CompletedTask;
        }

        public virtual Task Hide()
        {
           Destroy(gameObject);
           return Task.CompletedTask;
        }
    }
}
