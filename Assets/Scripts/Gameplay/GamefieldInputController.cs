using System;
using UnityEngine;

namespace Gameplay
{
    public class GamefieldInputController : MonoBehaviour
    {
        public event Action StartAim;
        public event Action ExitAim;
        public event Action StopAim;
        public bool IsActive { get; private set; }

        public void SetActive(bool active)
        {
            IsActive = active;
        }
        
        private void OnMouseDown()
        {
            if (!IsActive) return;
            StartAim?.Invoke();
        }

        private void OnMouseUp()
        {
            if (!IsActive) return;
            StopAim?.Invoke();
        }

        private void OnMouseExit()
        {
            if (!IsActive) return;
            ExitAim?.Invoke();
        }
    }
}
