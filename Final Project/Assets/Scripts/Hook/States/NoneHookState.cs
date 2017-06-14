using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneHookState : AbstractHookState
{

    public NoneHookState(hook pHook) : base(pHook)
    {

    }
    public override void Start()
    {

    }
    public override void Update()
    {
        _hook.gameObject.transform.position = GameManager.Boat.gameObject.transform.position;
    }
    public override void Refresh()
    {

    }
    public override hook.HookState StateType()
    {
        return hook.HookState.None;
    }
}
