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
