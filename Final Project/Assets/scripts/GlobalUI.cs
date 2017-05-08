using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeployHook()
    {
        boat b = GameObject.FindGameObjectWithTag("Boat").GetComponent<boat>();
        b.EnableFishing();
    }

    public void SendRadarPulse()
    {
        Radar r = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();
        r.SendPulse();
    }
}
