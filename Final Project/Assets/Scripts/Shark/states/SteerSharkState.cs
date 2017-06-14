using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerSharkState : AbstractSharkState
{
    private Transform _approachTransform;
    private Transform _steerTransform;
    private float _steerCounter = 0;
    private float _steerDuration;
    public SteerSharkState(shark pShark, Transform pApproachTransform, Transform pSteerTransform, float pSteerDuration) : base(pShark)
    {
        _approachTransform = pApproachTransform;
        _steerTransform = pSteerTransform;
        _steerDuration = pSteerDuration;
    }
    public override void Start()
    {
        _steerTransform.SetParent(basic.Hook.transform);
        _steerCounter = 0;
    }
    public override void Update()
    {
        _steerTransform.position = new Vector3(_steerTransform.position.x, basic.Hook.transform.position.y, _steerTransform.position.z);
        _steerCounter += Time.deltaTime;
        if (_steerCounter < _steerDuration)
        {
            float lerp = _steerCounter / _steerDuration;
            _shark.transform.position = Vector3.Lerp(_approachTransform.position, _steerTransform.position, lerp);
        }
        else
        {
            _shark.transform.position = _steerTransform.position;
            SetState(shark.SharkState.Hunt);
        }
        if (!basic.Hook.IsInState(hook.HookState.Fish)) SetState(shark.SharkState.None);
    }
    public override void Refresh()
    {

    }
    public override shark.SharkState StateType()
    {
        return shark.SharkState.Steer;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
