using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {
    [SerializeField]
    private GameObject scoreUI;
    [SerializeField]
    private Text totalScore;
    [SerializeField]
    Color flashColour;
    [SerializeField]
    float colourFlashTime;
    [SerializeField]
    private GameObject UISpawnPosition;
    [SerializeField]
    float minimumUIScale;
    [SerializeField]
    float maximumUIScale;
    [SerializeField]
    float UIRotationAngle;
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
        timeColourHasBeenFlashing = 0.0f;
        originalTotalScoreColour = totalScore.color;
        colourFlashing = false;
	}
	
	// Update is called once per frame
	void Update () {
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

        if (colourFlashing == true)
        {
            timeColourHasBeenFlashing -= Time.deltaTime;
            if (timeColourHasBeenFlashing <= 0)
            {
                colourFlashing = false;
                totalScore.color = originalTotalScoreColour;
            }
        }
    }

    public int GetScore()
    {
        return playerCurrentScore;
    }

    public void SetScore(int pNewOverridingScore)
    {
        playerCurrentScore = pNewOverridingScore;
    }

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
    private void createScoreUI(int pScore)
    {
        GameObject newScoreInstance = Instantiate(scoreUI, UIPosition);
        newScoreInstance.SetActive(true);
        float angle = Random.Range(-UIRotationAngle, UIRotationAngle);
        float scale = Random.Range(minimumUIScale, maximumUIScale);
        newScoreInstance.GetComponent<ScoreUIAnimation>().SetSpawnParametres(angle, scale);
        newScoreInstance.GetComponent<ScoreUIAnimation>().SetScoreText(pScore);
    }
}
