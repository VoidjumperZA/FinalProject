using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropeller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float speed = 0.0f;
        if (basic.Boat.GetAbstractState().StateType() == (boat.BoatState.Move))
        {
            MoveBoatState type = (MoveBoatState)basic.Boat.GetAbstractState();
            speed = type.GetBoatVelocity();
        }
        
        gameObject.transform.Rotate(speed, 0.0f, 0.0f);
	}
}
