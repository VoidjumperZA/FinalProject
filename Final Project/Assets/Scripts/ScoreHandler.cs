using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {
    [SerializeField] private Canvas _canvas;

    [Header("UI Pieces")]
    [SerializeField] private GameObject comboScoreUI;

    [Header("Flashing")]
    [SerializeField] Color flashColour;  //which colour the text flashes when it updates
    [SerializeField] private float colourFlashTime; //how long does it flash that colour
    [Header("Visual Values")]
    [SerializeField] private GameObject ComboUISpawnPosition; //where are we spawning the combo notifier ui
    [SerializeField] private float minimumUIScale; //our size is random, what is the minimum bound for scaling
    [SerializeField] private float maximumUIScale; //maximum bound for scaling
    [SerializeField] private float UIRotationAngle; //rotating our ui a little for effect
    [SerializeField] private float hookScoreXOffset;
    [SerializeField] private float hookScoreYOffset;
    [Header("Score Values")]
    [SerializeField] private int comboScoreValue;
    [SerializeField]
    private int smallFishScoreValue;
    [SerializeField]
    private int mediumFishScoreValue;
    [SerializeField]
    private int largeFishScoreValue;
    [SerializeField]
    private int trashPercentageModifier;
    [SerializeField]
    private float jellyfishPenaltyPercentage = 0.25f;
    private Transform ComboUIPosition;
    [HideInInspector]public float HookScore;
    [HideInInspector] public float BankedScore;
    private float timeColourHasBeenFlashing;
    private Color originalHookScoreColour;
    private Color originalTotalScoreColour;
    private bool colourFlashing;
    private Text flashTextHolder;
    private Color originalColourHolder;
    private int totalNumberOfTrashPieces;
    private int collectedTrash;

    // Use this for initialization
    void Start()
    {
        ComboUIPosition = ComboUISpawnPosition.transform;
        HookScore = 0;
        BankedScore = 0;
        //timeColourHasBeenFlashing = 0.0f;
        //originalHookScoreColour = currentHookScore.color;
        //colourFlashing = false;   
	}
	public Vector3 HookScorePosition()
    {
        Vector3 hookPosOnScreen = Camera.main.WorldToScreenPoint(GameManager.Hook.transform.position);
        Vector3 offsetPosition = new Vector3(hookPosOnScreen.x + hookScoreXOffset, hookPosOnScreen.y + hookScoreYOffset, 0.0f);
        return offsetPosition;
    }
	// Update is called once per frame
	void Update () 
    {
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

    public float GetScore()
    {
        return HookScore;
    }

    public float GetBankedScore()
    {
        return BankedScore;
    }
    /// <summary>
    /// Set score will completely overwrite the current score, not increment it. Please use AddScore() for that.
    /// </summary>
    /// <param name="pNewOverridingScore"></param>
    public void SetScore(int pNewOverridingScore)
    {
        HookScore = pNewOverridingScore;
    }

    /// <summary>
    /// Increments our current score. The boolean controls whether UI appears or not.
    /// </summary>
    /// <param name="pFishType"></param>
    /// <param name="pCreatUIAnnouncement"></param>
    public void AddScore(fish.FishType pFishType, bool pCreatUIAnnouncement, bool pCaughtAFish)
    {
        int pScore = GetFishScore(pFishType);
        HookScore += pScore;
        if (pCreatUIAnnouncement)
        {
            createScoreUI(pScore, false);
        }
        if (pCaughtAFish == true)
        {

        }

        //Briefly switch the colour and start a counter to switch it back for visual feedback
        //timeColourHasBeenFlashing = colourFlashTime;
        //colourFlashing = true;
        //currentHookScore.color = flashColour;
        //flashTextHolder = currentHookScore;
        //originalColourHolder = originalHookScoreColour;
    }
    public void AddScore(int pScore, bool pCreatUIAnnouncement, bool pCaughtAFish)
    {
        HookScore += pScore;
        if (pCreatUIAnnouncement == true)
        {
            createScoreUI(pScore, false);
        }
        if (pCaughtAFish == true)
        {

        }

        //Briefly switch the colour and start a counter to switch it back for visual feedback
        //timeColourHasBeenFlashing = colourFlashTime;
        //colourFlashing = true;
        //currentHookScore.color = flashColour;
        //flashTextHolder = currentHookScore;
        //originalColourHolder = originalHookScoreColour;
    }

    public void AddComboScore()
    {
        //createComboScoreUI();
    }

    public void BankScore()
    {
        //Add our score to the bank
        BankedScore += HookScore;

        //Empty it from the hook
        HookScore = 0;
        //totalScore.text = BankedScore + "";

        //Briefly switch the colour and start a counter to switch it back for visual feedback
        //timeColourHasBeenFlashing = colourFlashTime;
        //colourFlashing = true;
        //totalScore.color = flashColour;
        //flashTextHolder = totalScore;
        //originalColourHolder = originalTotalScoreColour;
    }

    //Instantiate a UI instance
    private void createScoreUI(float pScore, bool pJellyMinPercent)
    {
        GameObject newScoreInstance = Instantiate(GameManager.Levelmanager._baseUI.ScoreUI,
                                                    GameManager.Levelmanager._baseUI.ScoreUIPosition.transform.position, Quaternion.identity);
        newScoreInstance.transform.SetParent(GameManager.Levelmanager.Canvas().transform);

        //Activate it because our instantiated object is in world, but deactivated. 
        newScoreInstance.SetActive(true);

        //create our random angles and scale and send it to UI
        float angle = Random.Range(-UIRotationAngle, UIRotationAngle);
        float scale = Random.Range(minimumUIScale, maximumUIScale);
        newScoreInstance.GetComponent<ScoreUIAnimation>().SetSpawnParametres(angle, scale);
        if (pJellyMinPercent == true)
        {
            newScoreInstance.GetComponent<ScoreUIAnimation>().SetScoreText("-" + pScore);
            newScoreInstance.GetComponent<ScoreUIAnimation>().SetScoreTextColour(Color.red);
        }
        else
        {
            newScoreInstance.GetComponent<ScoreUIAnimation>().SetScoreText(pScore);
        }
    }

    private void createComboScoreUI()
    {
        GameObject newComboScoreInstance = Instantiate(comboScoreUI, ComboUIPosition);

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

    public int GetFishScore(fish.FishType pType)
    {
        switch(pType)
        {
            case fish.FishType.Small:
                return smallFishScoreValue;
                break;
            case fish.FishType.Medium:
                return mediumFishScoreValue;
                break;
            case fish.FishType.Large:
                return largeFishScoreValue;
                break;
            default:
                return -999999;
                break;
        }
    }

    public int GetTrashScoreModifier()
    {
        return trashPercentageModifier;
    }

    public float GetJellyfishPenalty()
    {
        return jellyfishPenaltyPercentage;
    }

    /// <summary>
    /// Remove score by a set value. If the value is greater than the available score, the score will be set to zero and not go into negative values.
    /// </summary>
    /// <param name="pHardValue"></param>
    public void RemoveScore(int pHardValue)
    {
        if (pHardValue >= HookScore)
        {
            Debug.Log("WARNING: The removed score is greater than or equal to the player's current score. Capping the score to zero.");
            HookScore = 0;
        }
        else
        {
            HookScore -= pHardValue;
        }
    }

    /// <summary>
    /// Remove score by a percentage. Percentage will be internally converted to an integer as well as an absolute value.
    /// </summary>
    /// <param name="pPercentage"></param>
    public void RemoveScore(bool pCreatUIAnnouncement)
    {
        float scoreRemoved = HookScore * jellyfishPenaltyPercentage;
        Debug.Log(scoreRemoved + " scoreRemoved");
        HookScore -= scoreRemoved;
        if (pCreatUIAnnouncement == true)
        {
            createScoreUI(scoreRemoved, true);
        }
    }

    public void SetTotalNumberOfTrashPieces(int pNumber)
    {
        totalNumberOfTrashPieces = pNumber;
    }

    public bool CollectATrashPiece()
    {
        bool firstTime = false;
        if (collectedTrash == 0)
        {
            firstTime = true;
        }
        if (collectedTrash < totalNumberOfTrashPieces)
        {
            collectedTrash++;
            Debug.Log("Collected: " + collectedTrash + " out of " + totalNumberOfTrashPieces + " pieces of trash.");
        }
        else
        {
            Debug.Log("Already have " + collectedTrash + "/" + totalNumberOfTrashPieces + " trash pieces collected. This message should not be appearing.");
        }
        return firstTime;
    }

    /// <summary>
    /// Returns either the pure percentage number of trash you have collected or the percentage timesed by the reward modifier, i.e. 86% x 200 points.
    /// </summary>
    /// <param name="pReturnOnlyPercentage"></param>
    /// <returns></returns>
    public int CalculatePercentageOceanCleaned(bool pReturnOnlyPercentage)
    {
        float percentage = 100.0f * ((float)collectedTrash / (float)totalNumberOfTrashPieces);
        int intPercentage = Mathf.FloorToInt(percentage);
        Debug.Log("Cleaned " + intPercentage + "% of the ocean.");

        if (pReturnOnlyPercentage == true)
        {
            return intPercentage;
        }
        else
        {
            return intPercentage * trashPercentageModifier;
        }

    }
}
