using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputTimer : MonoBehaviour
{
    [SerializeField]
    private float timeUntilReturnToMenu = 10.0f;

    [SerializeField]
    private float timeOutWarning = 10.0f;
    [SerializeField]
    private GameObject timeOutWarningUI;
    [SerializeField]
    private Text timeOutWarningCountdownText;
    private float timeLeft;
    private bool valid; 
	// Use this for initialization
	private void Start()
    {
        timeOutWarningUI.SetActive(false);
        timeLeft = timeUntilReturnToMenu;
        valid = true;
        if (timeOutWarning >= timeUntilReturnToMenu)
        {
            Debug.Log("ERROR: Time out warning time is larger than or equal to the time until return to main menu.\n\tMARKING SCRIPT AS INVALID");
            valid = false;
        }
    }
	
	// Update is called once per frame
	private void FixedUpdate ()
    {
        if (valid == true)
        {
            //Force load the menu scene if time is up
            if (timeLeft < 0)
            {
                Debug.Log("Time up");
                SceneManager.LoadScene(0);
            }
            //Count down time
            else
            {
                timeLeft -= Time.deltaTime;
                //Debug.Log("time: " + timeLeft);
            }

            //Display a warning with x seconds left until timeout
            if (timeLeft < timeOutWarning)
            {
                if (timeOutWarningUI.activeSelf == false) { timeOutWarningUI.SetActive(true); }
                timeOutWarningCountdownText.text = ": " + (int)timeLeft;
            }
        }
        
	}

    public void ResetClock()
    {
        //Disable warning UI if active
        if (timeOutWarningUI.activeSelf == true)
        {
            timeOutWarningUI.SetActive(false);
        }

        //Reset the clock
        timeLeft = timeUntilReturnToMenu;
    }
}
