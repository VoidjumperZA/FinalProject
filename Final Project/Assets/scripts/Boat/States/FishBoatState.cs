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
        GameManager.Hook.SetState(hook.HookState.Fish);
       // GameManager.Radar.SetState(radar.RadarState.None);
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
