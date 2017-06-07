using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntSharkState : AbstractSharkState
{
    private float _swimSpeed;
    public HuntSharkState(shark pShark, float pSwimSpeed) : base(pShark)
    {
        _swimSpeed = pSwimSpeed;
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        Vector3 differenceVector = basic.Hook.transform.position - _shark.transform.position;
        if (differenceVector.magnitude >= _swimSpeed)
        {
            _shark.transform.Translate(differenceVector.normalized * _swimSpeed);
        }
        else
        {
            SetState(shark.SharkState.None);
        }
        if (!basic.Hook.IsInState(hook.HookState.Fish)) SetState(shark.SharkState.None);
    }
    public override void Refresh()
    {

    }
    public override shark.SharkState StateType()
    {
        return shark.SharkState.Hunt;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
