using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFollowMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = basic.Boat.gameObject.transform.position.z;
        gameObject.transform.position = pos;
	}
}
