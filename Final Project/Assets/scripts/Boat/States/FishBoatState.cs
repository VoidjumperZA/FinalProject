using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBoatState : AbstractBoatState {

	public FishBoatState(boat pBoat) : base(pBoat)
    {

    }

    public override void Start()
    {
        basic.Hook.SetState(hook.HookState.Fish);
        basic.Radar.SetState(radar.RadarState.None);
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {

    }

    public override boat.BoatState StateType()
    {
        return boat.BoatState.Fish;
    }
}
