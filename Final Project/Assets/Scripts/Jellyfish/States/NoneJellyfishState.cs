using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneJellyfishState : AbstractJellyfishState
{

    public NoneJellyfishState(Jellyfish pJellyfish) : base(pJellyfish)
    {

    }
    public override void Start()
    {
        SetState(Jellyfish.JellyfishState.Swim);
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {

    }
    public override Jellyfish.JellyfishState StateType()
    {
        return Jellyfish.JellyfishState.None;
    }
}
