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
        private bool _inField;
        private bool _aiming;

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        private void OnMouseEnter()
        {
            _inField = true;
        }

        private void OnMouseDown()
        {
            _aiming = true;
            if (!IsActive) return;
            StartAim?.Invoke();
        }

        private void OnMouseUp()
        {
            if (!IsActive) return;
            if (_inField && _aiming)
            {
                _aiming = false;
                StopAim?.Invoke();
            }
        }

        private void OnMouseExit()
        {
            _inField = false;
            _aiming = false;
            if (!IsActive) return;
            ExitAim?.Invoke();
        }
    }
}
