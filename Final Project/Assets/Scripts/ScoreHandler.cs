﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {
    [SerializeField]
    private GameObject scoreUI; //the appearing score ui
    [SerializeField]
    private Text totalScore; //the counter listing our total score
    [SerializeField]
    Color flashColour;  //which colour the text flashes when it updates
    [SerializeField]
    float colourFlashTime; //how long does it flash that colour
    [SerializeField]
    private GameObject UISpawnPosition; //where are we spawning that ui
    [SerializeField]
    float minimumUIScale; //our size is random, what is the minimum bound for scaling
    [SerializeField]
    float maximumUIScale; //maximum bound for scaling
    [SerializeField]
    float UIRotationAngle; //rotating our ui a little for affect
    private Transform UIPosition;
    private int playerCurrentScore; 
    private float timeColourHasBeenFlashing;
    private Color originalTotalScoreColour;
    private bool colourFlashing;

    // Use this for initialization
    void Start ()
    {
        UIPosition = UISpawnPosition.transform;
        playerCurrentScore = 0;
        totalScore.text = playerCurrentScore + "";
        timeColourHasBeenFlashing = 0.0f;
        originalTotalScoreColour = totalScore.color;
        colourFlashing = false;
	}
	
	// Update is called once per frame
	void Update () {

        //Temporarily to simulate hitting different fish types
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddScore(10, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddScore(50, true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddScore(500, true);
        }

        //if our colour is in flash mode
        if (colourFlashing == true)
        {
            //count down
            timeColourHasBeenFlashing -= Time.deltaTime;
            //if time is up
            if (timeColourHasBeenFlashing <= 0)
            {
                colourFlashing = false;
                //reset the text back to it's original colour
                totalScore.color = originalTotalScoreColour;
            }
        }
    }

    public int GetScore()
    {
        return playerCurrentScore;
    }
    /// <summary>
    /// Set score will completely overwrite the current score, not increment it. Please use AddScore() for that.
    /// </summary>
    /// <param name="pNewOverridingScore"></param>
    public void SetScore(int pNewOverridingScore)
    {
        playerCurrentScore = pNewOverridingScore;
    }

    /// <summary>
    /// Increments our current score. The boolean controls whether UI appears or not.
    /// </summary>
    /// <param name="pAddedScore"></param>
    /// <param name="pCreatUIAnnouncement"></param>
    public void AddScore(int pAddedScore, bool pCreatUIAnnouncement)
    {
        playerCurrentScore += pAddedScore;
        if (pCreatUIAnnouncement == true)
        {
            createScoreUI(pAddedScore);
        }
        totalScore.text = playerCurrentScore + "";

        //Briefly switch the colour and start a counter to switch it back for visual feedback
        timeColourHasBeenFlashing = colourFlashTime;
        colourFlashing = true;
        totalScore.color = flashColour;
    }

    //Instantiate a UI instance
    private void createScoreUI(int pScore)
    {
        GameObject newScoreInstance = Instantiate(scoreUI, UIPosition);

        //Activate it because our instantiated object is in world, but deactivated. 
        newScoreInstance.SetActive(true);

        //create our random angles and scale and send it to UI
        float angle = Random.Range(-UIRotationAngle, UIRotationAngle);
        float scale = Random.Range(minimumUIScale, maximumUIScale);
        newScoreInstance.GetComponent<ScoreUIAnimation>().SetSpawnParametres(angle, scale);
        newScoreInstance.GetComponent<ScoreUIAnimation>().SetScoreText(pScore);
    }
}