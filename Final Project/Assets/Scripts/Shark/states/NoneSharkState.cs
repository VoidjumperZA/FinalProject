using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneSharkState : AbstractSharkState
{
    public NoneSharkState(shark pShark) : base(pShark)
    {

    }
	public override void Start () {
		
	}
	public override void Update () {
        if (basic.Hook.IsInState(hook.HookState.Fish))
        {
            SetState(shark.SharkState.Approach);
        }
	}
    public override void Refresh()
    {

    }
    public override shark.SharkState StateType()
    {
        return shark.SharkState.None;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
