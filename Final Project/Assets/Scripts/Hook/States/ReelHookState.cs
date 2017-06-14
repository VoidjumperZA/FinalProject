using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHookState : AbstractHookState {
    private boat _boat;
    private float _reelSpeed;

	public ReelHookState(hook pHook, boat pBoat, float pReelSpeed) : base(pHook)
    {
        _boat = pBoat;
        _reelSpeed = pReelSpeed;
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        float step = _reelSpeed;
        Vector3 differenceVector = (_boat.gameObject.transform.position - _hook.gameObject.transform.position);
        if (differenceVector.magnitude >= step) _hook.gameObject.transform.Translate(differenceVector.normalized * step);
        if (differenceVector.magnitude < step)
        {
            _hook.gameObject.transform.position = _boat.gameObject.transform.position;
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
