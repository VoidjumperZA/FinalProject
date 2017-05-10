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
        basic.Radar.SendPulse();
        if (Dragging())
        {
            SetState(boat.BoatState.Move);
            CameraHandler.SetCameraFocusPoint(CameraHandler.CameraFocus.FocusBoat, true);
        }
    }
    private bool Dragging()
    {
        if (!Input.GetMouseButton(0) || !mouse.GameObjectTagIs("Boat")) return false;
        Vector3 mouseWorldPoint = mouse.GetWorldPoint();
        return Mathf.Abs(mouseWorldPoint.x - _boat.gameObject.transform.position.x) > 0;
    }
    public override void Refresh()
    {

    }

    public override boat.BoatState StateType()
    {
        return boat.BoatState.None;
    }
}
