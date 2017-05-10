using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFreeHookState : AbstractHookState {

    public SetFreeHookState(hook pHook) : base(pHook)
    {
    }
    public override void Start()
    {
    }
    public override void Update()
    {
        for (int i = 0; i < _hook.FishOnHook.Count; i++)
        {
            _hook.FishOnHook[i].Release();
            basic.Generals.Remove(_hook.FishOnHook[i]);
            GameObject.Destroy(_hook.FishOnHook[i].gameObject);
        }
        _hook.FishOnHook.Clear();
        basic.GlobalUi.SwitchHookButtons();
        basic.Boat.SetState(boat.BoatState.None);
        SetState(hook.HookState.None);
        CameraHandler.SetCameraFocusPoint(CameraHandler.CameraFocus.FocusBoat, true);
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
