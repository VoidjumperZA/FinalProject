using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTimer : MonoBehaviour
{
    [SerializeField]
    private float timeUntilReturnToMenu = 10.0f;
    private float timeLeft;
	// Use this for initialization
	void Start () {
        timeLeft = timeUntilReturnToMenu;

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {        
        if (timeLeft < 0)
        {
            Debug.Log("Time up");
        }
        else
        {
            timeLeft -= Time.deltaTime;
            Debug.Log("time: " + timeLeft);
        }
	}

    public void ResetClock()
    {
        timeLeft = timeUntilReturnToMenu;
    }
}
