using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHookState : AbstractHookState {
    private float _reelSpeed;

	public ReelHookState(hook pHook, float pReelSpeed) : base(pHook)
    {
        _reelSpeed = pReelSpeed;
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        float step = _reelSpeed;
        Vector3 differenceVector = (basic.Boat.gameObject.transform.position - _hook.gameObject.transform.position);
        if (differenceVector.magnitude >= step) _hook.gameObject.transform.Translate(differenceVector.normalized * step);
        if (differenceVector.magnitude < step)
        {
            _hook.transform.position = basic.Boat.transform.position;
            SetState(hook.HookState.SetFree);
        }
    }
    public override void Refresh()
    {

    }
    public override hook.HookState StateType()
    {
        return hook.HookState.Reel;
    }
}
