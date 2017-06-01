using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour
{
	[Header("Tutorial Buttons")]
    [SerializeField]
    private Button _playGameButton;
    [SerializeField]
    private Button _skipTutorialButton;

    [SerializeField]
    private Image _handDeployHook;
    [SerializeField]
    private Image _handSwipe;
    [SerializeField]
    private Image _playExplode;
    /*[SerializeField]
    private Image _playButtonImage;*/
    [SerializeField]
    private Image _replayExplode;
    /*[SerializeField]
    private Image _replayButtonImage;*/
    [SerializeField]
    private Button _deployHookButton;
    [SerializeField]
    private Button _reelUpHook;

    [Header("Timer")]
    [SerializeField]
    private Text gameTimerText;

    [HideInInspector]
    public bool InTutorial = true;
    [HideInInspector]
    public bool DropHookCompleted = false;
    [HideInInspector]
    public bool ReelUpHookCompleted = false;
    [HideInInspector]
    public bool MoveBoatCompleted = false;
    [HideInInspector]
    public bool SwipehandCompleted = false;
    

    //Ocean Clean Up Bar
    /*private float barDisplay;
    private Vector2 oceanCleanUpBarPosition;
    private Vector2 oceanCleanUpBarSize;
    [SerializeField]
    private Texture2D oceanCleanUpBarEmpty;
    [SerializeField]
    private Texture2D oceanCleanUpBarFull;*/
    [Header("Ocean Bar")]
    [SerializeField]
    private Slider oceanCleanUpProgressBar;
    [SerializeField]
    GameObject oceanCleanUpBarChildFill;
    [SerializeField]
    GameObject oceanCleanUpBarChildBackground;
    [SerializeField]
    Image oceanCleanUpBarChildStripe;
    [SerializeField]
    Image oceanCleanUpBarChildFrameBackground;
    [SerializeField]
    GameObject oceanCleanUpBarChildText;
    [SerializeField]
    private float timeOceanBarIsShown;
    [SerializeField]
    private float oceanBarFadeInSpeed;
    [SerializeField]
    private float oceanBarFadeOutSpeed;
    [SerializeField]
    private float oceanBarMovementSpeed;

    private GameTimer gameTimer;

    [Header("HighScore")]
    [SerializeField] private GameObject _totalScore;
    [SerializeField] private GameObject _currency;

    void Start()
    {
        _totalScore.SetActive(false);
        _currency.SetActive(false);

        gameTimer = GameObject.Find("Manager").GetComponent<GameTimer>();        
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = 0 + "%";
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildFrameBackground.CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildStripe.CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(0.0f, 0.0f, false);

        //Warnings
        if (!_deployHookButton) Debug.Log("Warning: You need to assign DeployHookButton to GlobalUI.");
        if (!_reelUpHook) Debug.Log("Warning: You need to assign ReelUpButton to GlobalUI.");

        DeployHookButton(false);
        ReelUpHookButton(false);
        _playExplode.gameObject.SetActive(false);
        _replayExplode.gameObject.SetActive(false);
        //_playButtonImage.gameObject.SetActive(true);
        //_replayButtonImage.gameObject.SetActive(true);
		
        
        ShowHandHookButton(false);
        //_handDeployHook.transform.position = new Vector2 (_deployHookButton.transform.position.x + 15, _deployHookButton.transform.position.y - 15);
        
        ShowHandSwipe(false);
    }
    public void OnPlayGameClick()
    {
        StartCoroutine(PlayGameAnim());
    }
    public void OnSkipTutorialClick()
    {
        StartCoroutine(ReplayGameAnim());
    }
    public void DeployHookButton(bool pBool) { _deployHookButton.gameObject.SetActive(pBool); }
    public void ReelUpHookButton(bool pBool) { _reelUpHook.gameObject.SetActive(pBool); }

    public void ShowHandHookButton(bool pBool) { _handDeployHook.gameObject.SetActive(pBool); }
    public void ShowHandSwipe(bool pBool) { _handSwipe.gameObject.SetActive(pBool); }

    public void DeployHook()
    {
        basic.Boat.SetState(boat.BoatState.Fish);
        basic.Scorehandler.ToggleHookScoreUI(true);
        DeployHookButton(false);
        if (!InTutorial)
        {
            ReelUpHookButton(true);
            GameObject.Find("Manager").GetComponent<Combo>().CreateNewCombo();
        }
        else
        {
            if (DropHookCompleted)
            {
                ReelUpHookButton(true);
                ShowHandHookButton(true);
            }
            else
            {
                DropHookCompleted = true;
                if (!ReelUpHookCompleted) { ShowHandHookButton(false); }
            }                     
            StartCoroutine(ShowHookHand());
        }
    }

    public void ReelUpHook()
    {
        basic.Hook.SetState(hook.HookState.Reel);
        if (!InTutorial)
        {
            basic.combo.ClearPreviousCombo(false);
        }
        else
        {
            //Stop all new fish spawning while we are in this part of the tutorial as the ocean should be empty
            basic.Tempfishspawn._boatSetUp = false;
            //basic.Tempfishspawn.ClearAllFish(); //gives shit ton of errors
            if (DropHookCompleted)
            {
                basic.Tempfishspawn._boatSetUp = true;
                basic.Seafloorspawning.SpawnTrash();
                basic.Seafloorspawning.SpawnSpecialItems();
                ReelUpHookCompleted = true;
                ShowHandHookButton(false);
                WaitForBoatMove();
            }
        }
    }
    public void WaitForBoatMove()
    {
        DeployHookButton(false);
        ReelUpHookButton(false);
    }
    public void SwitchHookButtons()
    {
        DeployHookButton(true);
        ReelUpHookButton(false);
    }

    private void DisableButton(Button buttonToDisable)
    {
        buttonToDisable.interactable = false;
    }

    private void EnableButton(Button buttonToEnable)
    {
        buttonToEnable.interactable = true;
    }

    public void UpdateOceanProgressBar(bool pFirstTimeAnim)
    {
        //Get the percentage, set the bar value and the helper text
        int percentage = basic.Scorehandler.CalculatePercentageOceanCleaned(true);
        oceanCleanUpProgressBar.GetComponent<Slider>().value = 100 - percentage;
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = percentage + "%";

        //Start a coroutine to disable after a while     
        StartCoroutine(ShowThenFadeOceanBar());        
        if (pFirstTimeAnim)
        {
            oceanCleanUpProgressBar.GetComponent<OceanCleanUpUIAnimation>().AnimateFirstTimeMovement();
        }
    }
    private IEnumerator PlayGameAnim()
    {
		
        _playGameButton.gameObject.SetActive(false);
        _playExplode.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.6f);
        gameTimer.BeginCountdown();
        _playExplode.gameObject.SetActive(false);

        basic.Camerahandler.SetViewPoint(CameraHandler.CameraFocus.Ocean);
        basic.Boat.SetState(boat.BoatState.SetUp);
        _skipTutorialButton.gameObject.SetActive(false);

        _totalScore.SetActive(true);
        _currency.SetActive(true);
        gameTimer.BeginCountdown();
    }


    private IEnumerator ReplayGameAnim()
    {
        _skipTutorialButton.gameObject.SetActive(false);
        _replayExplode.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);
        gameTimer.BeginCountdown();

        _replayExplode.gameObject.SetActive(false);

        InTutorial = false;
        basic.Camerahandler.SetViewPoint(CameraHandler.CameraFocus.Ocean);
        basic.Boat.SetState(boat.BoatState.SetUp);
        _playGameButton.gameObject.SetActive(false);

        _totalScore.SetActive(true);
        _currency.SetActive(true);
        basic.Seafloorspawning.SpawnTrash();
        basic.Seafloorspawning.SpawnSpecialItems();
    }
    private IEnumerator ShowThenFadeOceanBar()
    {
        //Immediately show the bar
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildFrameBackground.CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildStripe.CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);

        //Show for a small time
        yield return new WaitForSeconds(timeOceanBarIsShown);

        //Fade out
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildFrameBackground.CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildStripe.CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
    }

    private IEnumerator ShowHookHand()
    {
        yield return new WaitForSeconds(4);
        if(!SwipehandCompleted) ShowHandSwipe(true);
    }

    void Update()
    {
        //temp testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool firstTime = basic.Scorehandler.CollectATrashPiece();
            UpdateOceanProgressBar(firstTime);
        }
        gameTimerText.text = gameTimer.GetFormattedTimeLeftAsString();
    }

    public float GetOceanBarMovementSpeed()
    {
        return oceanBarMovementSpeed;
    }

    public Slider GetOceanCleanUpBar()
    {
        return oceanCleanUpProgressBar;
    }
}
