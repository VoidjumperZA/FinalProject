using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public Vector3 Position()
    {
        return gameObject.transform.position;
    }
    public Quaternion Rotation()
    {
        return gameObject.transform.rotation;
    }
}
