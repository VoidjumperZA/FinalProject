using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour {

    private float totalTimePerLevel;
    private float timeLeft;
    private bool counting;
	// Use this for initialization
	void Start () {
        totalTimePerLevel = 3 * 60;
        timeLeft = totalTimePerLevel;
        counting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (counting == true)
        {
            timeLeft -= Time.deltaTime;
            //Debug.Log("Time: " + timeLeft);
            if (timeLeft <= 0)
            {
                basic.EndGame();
                //boat movement stop
                //controls disabled
                //fish stop spawning
                //hook reel if it's down
                //radar stop pulsing
            }
        }
	}

    public void BeginCountdown()
    {
        counting = true;
    }
}
