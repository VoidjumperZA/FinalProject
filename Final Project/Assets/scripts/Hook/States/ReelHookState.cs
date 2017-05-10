using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHookState : AbstractHookState {
    private float _speed;

	public ReelHookState(hook pHook, float pSpeed) : base(pHook)
    {
        _speed = pSpeed;
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        Vector3 differenceVector = (basic.Boat.gameObject.transform.position - _hook.gameObject.transform.position);
        if (differenceVector.magnitude >= _speed) _hook.gameObject.transform.Translate(differenceVector.normalized * _speed);
        if (differenceVector.magnitude < _speed)
        {
            _hook.gameObject.transform.position = basic.Boat.transform.position;
            SetState(hook.HookState.SetFree);
            GameObject.Find("Manager").GetComponent<ScoreHandler>().BankScore();
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
