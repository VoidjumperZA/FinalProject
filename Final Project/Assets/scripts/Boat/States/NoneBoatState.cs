using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneBoatState : AbstractBoatState {

    public NoneBoatState(boat pBoat) : base(pBoat)
    {

    }

    public override void Start()
    {
        CameraHandler.SetViewPoint(CameraHandler.CameraFocus.Boat, true);
    }
    
    public override void Update ()
    {
    }
    public override void Refresh()
    {

    }

    public override boat.BoatState StateType()
    {
        return boat.BoatState.None;
    }
}
