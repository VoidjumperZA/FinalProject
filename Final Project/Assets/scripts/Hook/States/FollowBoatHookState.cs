using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoatHookState : AbstractHookState
{
    private boat _boat;
    public FollowBoatHookState(hook pHook, boat pBoat) : base(pHook)
    {
        _boat = pBoat;
    }
    public override void Start()
    {
    }
    public override void Update()
    {
        _hook.gameObject.transform.position = _boat.gameObject.transform.position;
    }
    public override void Refresh()
    {

    }
    public override hook.HookState StateType()
    {
        return hook.HookState.FollowBoat;
    }
}
