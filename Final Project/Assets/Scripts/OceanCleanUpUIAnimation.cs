using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanCleanUpUIAnimation : MonoBehaviour {
    private bool moving;
    private Vector3 originalPosition;
	// Use this for initialization
	void Start () {
        moving = false;
        originalPosition = gameObject.transform.position;
        gameObject.transform.position = Camera.main.WorldToScreenPoint(basic.Hook.transform.position); //
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (moving == true)
        {
            float speed = basic.GlobalUI.GetOceanbarMovemenetSpeed();

            Vector3 differenceVector = (originalPosition - gameObject.transform.position);
            if (differenceVector.magnitude >= speed) gameObject.transform.Translate(differenceVector.normalized * speed);
            if (differenceVector.magnitude < speed)
            {
                moving = false;
            }
        }

    }

    public void AnimateFirstTimeMovement()
    {
        moving = true;
    }
}
