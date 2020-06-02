using System;
using HexagonAli.Helpers;
using Lean.Touch;
using UnityEngine;

namespace HexagonAli.Managers
{
    public class InputManager : GenericSingleton<InputManager>
    {
        public event Action<Vector3> OnTap;
        public event Action<Vector3, Vector2> OnSwipe;
        // Start is called before the first frame update
        void Start()
        {
            LeanTouch.OnFingerTap += LeanTouch_OnFingerTap;
            LeanTouch.OnFingerSwipe += LeanTouch_OnFingerSwipe;
        }

        private void LeanTouch_OnFingerSwipe(LeanFinger finger)
        {
            OnSwipe?.Invoke(finger.GetWorldPosition(10f), finger.SwipeScaledDelta);
        }

        private void LeanTouch_OnFingerTap(LeanFinger finger)
        {
            OnTap?.Invoke(finger.GetWorldPosition(10f));

        }
    }
}
