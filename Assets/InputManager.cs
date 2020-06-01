using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : GenericSingleton<InputManager>
{
    public event Action<Vector3> OnTap;
    // Start is called before the first frame update
    void Start()
    {
        LeanTouch.OnFingerTap += LeanTouch_OnFingerTap;
    }

    private void LeanTouch_OnFingerTap(LeanFinger finger)
    {
        OnTap?.Invoke(finger.GetWorldPosition(10f));

    }
}
