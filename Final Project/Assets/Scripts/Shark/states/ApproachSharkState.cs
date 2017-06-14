using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachSharkState : AbstractSharkState
{
    private Transform _sharkSpawn;
    private Transform _approachTransform;
    private float _approachCounter = 0;
    private float _approachDuration;
    public ApproachSharkState(shark pShark, Transform pSharkSpawn, Transform pApproachTransform, float pApproachDuration) : base(pShark)
    {
        _sharkSpawn = pSharkSpawn;
        _approachTransform = pApproachTransform;
        _approachDuration = pApproachDuration;
    }
    public override void Start()
    {
        _shark.transform.position = _sharkSpawn.position;
        _shark.transform.rotation = _sharkSpawn.rotation;
        _approachCounter = 0;
    }
    public override void Update()
    {
        _approachCounter += Time.deltaTime;
        if (_approachCounter < _approachDuration)
        {
            float lerp = _approachCounter / _approachDuration;
            _shark.transform.position = Vector3.Lerp(_sharkSpawn.position, _approachTransform.position, lerp);
        }
        else
        {
            _shark.transform.position = _approachTransform.position;
            SetState(shark.SharkState.Steer);
        }
        if (!basic.Hook.IsInState(hook.HookState.Fish)) SetState(shark.SharkState.None);
    }
    public override void Refresh()
    {

    }
    public override shark.SharkState StateType()
    {
        return shark.SharkState.Approach;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
