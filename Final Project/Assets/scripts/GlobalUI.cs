using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour
{
    [SerializeField]
    private Button _playGameButton;
    [SerializeField]
    private Button _skipTutorialButton;


    [SerializeField]
    private Button _deployHookButton;
    [SerializeField]
    private Button _reelUpHook;
    [SerializeField]
    private Button _radarButton;

    [HideInInspector]
    public bool InTutorial = true;
    [HideInInspector]
    public bool DropHookCompleted = false;
    [HideInInspector]
    public bool ReelUpHookCompleted = false;
    [HideInInspector]
    public bool MoveBoatCompleted = false;

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
    GameObject oceanCleanUpBarChildText;
    [SerializeField]
    private float timeOceanBarIsShown;
    [SerializeField]
    private float oceanBarFadeInSpeed;
    [SerializeField]
    private float oceanBarFadeOutSpeed;
    [SerializeField]
    private float oceanBarMovementSpeed;

    void Start()
    {
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = 0 + "%";
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(0.0f, 0.0f, false);

        //Warnings
        if (!_deployHookButton) Debug.Log("Warning: You need to assign DeployHookButton to GlobalUI.");
        if (!_reelUpHook) Debug.Log("Warning: You need to assign ReelUpButton to GlobalUI.");
        if (!_radarButton) Debug.Log("Warning: You need to assign RadarButton to GlobalUI.");

        DeployHookButton(false);
        RadarButton(false);
        ReelUpHookButton(false);
    }
    public void OnPlayGameClick()
    {
        CameraHandler.SetViewPoint(CameraHandler.CameraFocus.Ocean);
        basic.Boat.SetState(boat.BoatState.SetUp);
        _playGameButton.gameObject.SetActive(false);
        _skipTutorialButton.gameObject.SetActive(false);
    }
    public void OnSkipTutorialClick()
    {
        InTutorial = false;
        CameraHandler.SetViewPoint(CameraHandler.CameraFocus.Ocean);
        basic.Boat.SetState(boat.BoatState.SetUp);
        _playGameButton.gameObject.SetActive(false);
        _skipTutorialButton.gameObject.SetActive(false);
    }
    public void DeployHookButton(bool pBool) { _deployHookButton.gameObject.SetActive(pBool); }
    public void ReelUpHookButton(bool pBool) { _reelUpHook.gameObject.SetActive(pBool); }
    public void RadarButton(bool pBool) { _radarButton.gameObject.SetActive(pBool); }

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
            if (DropHookCompleted) ReelUpHookButton(true);
            DropHookCompleted = true;
        }
        //RadarButton(false);
        //ReelUpHookButton(true);


    }

    public void ReelUpHook()
    {
        basic.Hook.SetState(hook.HookState.Reel);
        if (!InTutorial)
        {
            GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
        }
        else
        {
            if (DropHookCompleted)
            {
                ReelUpHookCompleted = true;
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
        //RadarButton(true);
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
        oceanCleanUpProgressBar.GetComponent<Slider>().value = percentage;
        oceanCleanUpProgressBar.GetComponentInChildren<Text>().text = percentage + "%";

        //Start a coroutine to disable after a while     
        StartCoroutine(ShowThenFadeOceanBar());        
        if (pFirstTimeAnim)
        {
            oceanCleanUpProgressBar.GetComponent<OceanCleanUpUIAnimation>().AnimateFirstTimeMovement();
        }
    }

    private IEnumerator ShowThenFadeOceanBar()
    {
        //Immediately show the bar
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(1.0f, oceanBarFadeInSpeed, false);

        //Show for a small time
        yield return new WaitForSeconds(timeOceanBarIsShown);

        //Fade out
        oceanCleanUpBarChildFill.GetComponent<Image>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildBackground.GetComponent<Image>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
        oceanCleanUpBarChildText.GetComponent<Text>().CrossFadeAlpha(0.0f, oceanBarFadeOutSpeed, false);
    }

    void Update()
    {
        //temp testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool firstTime = basic.Scorehandler.CollectATrashPiece();
            UpdateOceanProgressBar(firstTime);
        }
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
