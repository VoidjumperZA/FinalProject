﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFreeHookState : AbstractHookState {

    public SetFreeHookState(hook pHook) : base(pHook)
    {
    }
    public override void Start()
    {
        for (int i = 0; i < _hook.FishOnHook.Count; i++)
        {
            _hook.FishOnHook[i].SetState(fish.FishState.PiledUp);
        }
        _hook.FishOnHook.Clear();
        basic.GlobalUi.SwitchHookButtons();
        basic.Boat.SetState(boat.BoatState.None);
        SetState(hook.HookState.None);
        CameraHandler.SetCameraFocusPoint(CameraHandler.CameraFocus.FocusBoat, true);
    }
    public override void Update()
    {
    }
    public override void Refresh()
    {
        _hook.FishOnHook = null;
        _hook.FishOnHook = new List<fish>();
    }
    public override hook.HookState StateType()
    {
        return hook.HookState.SetFree;
    }
}
