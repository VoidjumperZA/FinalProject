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
       // GameManager.Radar.SetState(radar.RadarState.Pulse);
        /*basic.Camerahandler.SetViewPoint(CameraHandler.CameraFocus.Ocean);

        if (basic.GlobalUI.InTutorial && basic.GlobalUI.ReelUpHookCompleted)
        {
            basic.GlobalUI.SetHandSwipePosition(basic.Boat.gameObject, new Vector3(30, -20, 0));
            basic.GlobalUI.ShowHandSwipe(true);
        }*/
    }
	
	public override void Update ()
    {

        if (Dragging() && mouseAboveHalf() == true) SetState(boat.BoatState.Move);

        if (Dragging())
        {
            if (TutorialUI.GetTouchedReelUp()) SetState(boat.BoatState.Move);
        }

    }

    private bool mouseAboveHalf()
    {
        if (Input.mousePosition.y > (Screen.height / 2)) //|| mouse.GetTouch().y > (Screen.height / 2))
        {
            return true;
        }
        else
        {
            return false;
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
        //if (basic.GlobalUI.InTutorial && !basic.GlobalUI.ReelUpHookCompleted) return false;
        if ((!Input.GetMouseButton(0) && !mouse.Touching())) return false;
        Vector3 mouseWorldPoint = mouse.GetWorldPoint();
        return Mathf.Abs(mouseWorldPoint.x - _boat.gameObject.transform.position.x) > 0;
    }
}
