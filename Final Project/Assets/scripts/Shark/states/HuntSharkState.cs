using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntSharkState : AbstractSharkState
{
    public HuntSharkState(shark pShark) : base(pShark)
    {

    }
    public override void Start()
    {

    }
    public override void Update()
    {

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
