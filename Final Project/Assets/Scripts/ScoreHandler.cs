using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {
    [Header("UI Pieces")]
    [SerializeField] private GameObject scoreUI; //the appearing score ui
    [SerializeField] private Text totalScore; //the counter listing our total score
    [SerializeField] private Text currentHookScore;
    [SerializeField] private GameObject comboScoreUI;
    [SerializeField] private Text currencyText;


    [Header("Flashing")]
    [SerializeField] Color flashColour;  //which colour the text flashes when it updates
    [SerializeField] private float colourFlashTime; //how long does it flash that colour
    [Header("Values")]
    [SerializeField] private GameObject UISpawnPosition; //where are we spawning that ui
    [SerializeField] private GameObject ConboUISpawnPosition; //where are we spawning the combo notifier ui
    [SerializeField] private int comboScoreValue;
    [SerializeField] private float minimumUIScale; //our size is random, what is the minimum bound for scaling
    [SerializeField] private float maximumUIScale; //maximum bound for scaling
    [SerializeField] private float UIRotationAngle; //rotating our ui a little for effect
    [SerializeField] private float hookScoreXOffset;
    [SerializeField] private float hookScoreYOffset;
    private Transform UIPosition;
    private int playerCurrentScore;
    private int caughtFishCurrecy;
    private int bankedScore;
    private float timeColourHasBeenFlashing;
    private Color originalHookScoreColour;
    private Color originalTotalScoreColour;
    private bool colourFlashing;
    private Text flashTextHolder;
    private Color originalColourHolder;

    // Use this for initialization
    void Start()
    {
        UIPosition = UISpawnPosition.transform;
        playerCurrentScore = 0;
        bankedScore = 0;
        caughtFishCurrecy = 0;
        currentHookScore.text = playerCurrentScore + "";
        currentHookScore.enabled = false;
        totalScore.text = bankedScore + "";
        currencyText.text = caughtFishCurrecy + "";
        timeColourHasBeenFlashing = 0.0f;
        originalHookScoreColour = currentHookScore.color;
        originalTotalScoreColour = totalScore.color;
        colourFlashing = false;   
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 hookPosOnScreen = Camera.main.WorldToScreenPoint(basic.Hook.transform.position);
        Vector3 offsetPosition = new Vector3(hookPosOnScreen.x + hookScoreXOffset, hookPosOnScreen.y + hookScoreYOffset, 0.0f);
        currentHookScore.transform.position = offsetPosition;       

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
                flashTextHolder.color = originalColourHolder;
                flashTextHolder = null;
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
    public void AddScore(int pAddedScore, bool pCreatUIAnnouncement, bool pCaughtAFish)
    {
        playerCurrentScore += pAddedScore;
        if (pCreatUIAnnouncement == true)
        {
            createScoreUI(pAddedScore);
        }
        currentHookScore.text = playerCurrentScore + "";
        if (pCaughtAFish == true)
        {
            addCurrency();
        }

        //Briefly switch the colour and start a counter to switch it back for visual feedback
        timeColourHasBeenFlashing = colourFlashTime;
        colourFlashing = true;
        currentHookScore.color = flashColour;
        flashTextHolder = currentHookScore;
        originalColourHolder = originalHookScoreColour;
    }

    public void AddComboScore()
    {
        createComboScoreUI();
    }

    public void BankScore()
    {
        //Add our score to the bank
        bankedScore += playerCurrentScore;

        //Empty it from the hook
        playerCurrentScore = 0;

        //Display on the UI
        currentHookScore.text = playerCurrentScore + "";
        totalScore.text = bankedScore + "";

        //Briefly switch the colour and start a counter to switch it back for visual feedback
        timeColourHasBeenFlashing = colourFlashTime;
        colourFlashing = true;
        totalScore.color = flashColour;
        flashTextHolder = totalScore;
        originalColourHolder = originalTotalScoreColour;
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

    private void createComboScoreUI()
    {
        GameObject newComboScoreInstance = Instantiate(comboScoreUI, ConboUISpawnPosition.transform);

        //Activate it because our instantiated object is in world, but deactivated. 
        newComboScoreInstance.SetActive(true);

        //create our random angles and scale and send it to UI
        //float angle = Random.Range(-UIRotationAngle, UIRotationAngle);
        //float scale = Random.Range(minimumUIScale, maximumUIScale);
        //newScoreInstance.GetComponent<ScoreUIAnimation>().SetSpawnParametres(angle, scale);
        newComboScoreInstance.GetComponent<ComboScoreUIAnimation>().SetScoreText(comboScoreValue);
    }

    public int GetComboScoreValue()
    {
        return comboScoreValue;
    }

    private void addCurrency()
    {
        caughtFishCurrecy++;
        currencyText.text = "" + caughtFishCurrecy;
    }

    public void ToggleHookScoreUI(bool pState)
    {
        currentHookScore.enabled = pState;
    }
}
