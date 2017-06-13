using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanOriginFollow : MonoBehaviour {

    //[SerializeField]
    //private GameObject scanPlane;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position;
        if (GameManager.Boat.GetAbstractState().StateType() != (boat.BoatState.Fish))
        {
            position = GameManager.Boat.transform.position;
        }
        else
        {
            position = GameManager.Hook.transform.position;
        }        
        //position.z = scanPlane.transform.position.z;
        gameObject.transform.position = position;
	}
}
