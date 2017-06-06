using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBoatState : AbstractBoatState {

    public StationaryBoatState(boat pBoat) : base(pBoat)
    {

    }
	public override void Start ()
    {
        basic.Radar.SetState(radar.RadarState.Pulse);
        basic.Camerahandler.SetViewPoint(CameraHandler.CameraFocus.Ocean);

        if (basic.GlobalUI.InTutorial && basic.GlobalUI.ReelUpHookCompleted)
        {
            basic.GlobalUI.ShowHandSwipe(true);
        }
    }
	
	public override void Update ()
    {
        if (Dragging())
        {
            SetState(boat.BoatState.Move);
        }
    }
    public override void Refresh()
    {

    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.Stationary;
    }
    private bool Dragging()
    {
        if (basic.GlobalUI.InTutorial && !basic.GlobalUI.ReelUpHookCompleted) return false;
        if ((!Input.GetMouseButton(0) && !mouse.Touching()) || !mouse.GameObjectTagIs("Boat")) return false;
        Vector3 mouseWorldPoint = mouse.GetWorldPoint();
        return Mathf.Abs(mouseWorldPoint.x - _boat.gameObject.transform.position.x) > 0;
    }
}
