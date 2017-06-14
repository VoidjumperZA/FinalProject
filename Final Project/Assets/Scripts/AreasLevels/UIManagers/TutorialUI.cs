using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : BaseUI
{
    private bool _onEnterScene = true;
    private bool _onLeaveScene = true;

    [Header("Animations")]
    [SerializeField]
    private Image _handClick;
    [SerializeField]
    private Image _arrows;
    [SerializeField]
    private Image _handMove;
    [SerializeField]
    private Image _handClickNoDrops;

    [Header("Controls")]
    [SerializeField]
    private Button _dropHook;
    [SerializeField]
    private Button _reelHook;

    [Header("Game Time")]
    [SerializeField]
    private Image _gameTimerBoard;
    [SerializeField]
    private Text _gameTimerText;

    [Header("Score")]
    [SerializeField]
    private Image _totalScoreBoard;
    [SerializeField]
    private Text _totalScoreText;
    [SerializeField]
    private Text _hookScoreText;

    [Header("Shopping List")]
    [SerializeField]
    private Image _shoppingList;

    //Conditions for showing UI in order
    private bool touchedScreen = true;
    private bool touchedDeployHook = false;
    private bool firstTimeReelUp = true;
    private static bool touchedReelUpHook = false;
    private bool movedBoat = false;

    public override void Start()
    {
        // Controls
        SetActive(false, _dropHook.gameObject, _reelHook.gameObject);
        // Game Time
        SetActive(false, _gameTimerBoard.gameObject, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, _hookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        //Animations
        SetActive(false, _handMove.gameObject,_handClick.gameObject, _arrows.gameObject,_handClickNoDrops.gameObject);
       
        //Debug.Log("TutorialUI - Start();");
    }
    public override void Update()
    {
        // Game Time
        _gameTimerText.text = GameManager.Gametimer.GetFormattedTimeLeftAsString();
        // Score
        _totalScoreText.text = GameManager.Scorehandler.BankedScore + "";

        _hookScoreText.text = GameManager.Scorehandler.HookScore + "";
        _hookScoreText.transform.position = GameManager.Scorehandler.HookScorePosition();

        //Steps
        if (touchedScreen && (!touchedDeployHook))
        {
            Debug.Log("Step 2");
            SetActive(false, _handClickNoDrops.gameObject);
            SetActive(true, _dropHook.gameObject);
            _handClick.transform.position = _dropHook.transform.position + new Vector3(10,-10,0);
            SetActive(true, _handClick.gameObject);
    
        }
        else if (firstTimeReelUp)
        {
            Debug.Log("Step 3");
            SetActive(false, _handClick.gameObject);
        }
        else if (!touchedReelUpHook)
        {
            Debug.Log("Step 4");
            SetActive(true, _handClick.gameObject);
        }
    }
    public void OnDropHook()
    {
        if (!GameManager.Boat.CanDropHook()) return;
        touchedDeployHook = true;

        SetActive(false, _dropHook.gameObject);
        SetActive(true, _reelHook.gameObject);
        GameManager.Boat.SetState(boat.BoatState.Fish);
    }
    public void OnReelHook()
    {
        if (!firstTimeReelUp) { firstTimeReelUp = false; }
        else { touchedReelUpHook = true; }
            
        SetActive(true, _dropHook.gameObject);
        SetActive(false, _reelHook.gameObject);
        GameManager.Hook.SetState(hook.HookState.Reel);
    }
    
    public override void OnEnterScene()
    {
        if (!_onEnterScene)
        {
            Debug.Log("OnEnterScene was already called once for current instance");
            return;
        }
        //Show hand for activating the radar
        SetScreenPosition(_handClickNoDrops.gameObject, GameManager.Boat.gameObject, new Vector3(0, 0, 0));
        SetActive(true, _handClickNoDrops.gameObject);

        // Game Time
        GameManager.Gametimer.BeginCountdown();
        SetActive(true, _gameTimerBoard.gameObject, _gameTimerText.gameObject);
        // Score
        SetActive(true, _totalScoreBoard.gameObject, _totalScoreText.gameObject, _hookScoreText.gameObject);
        // Shopping List
        //SetActive(true, _shoppingList.gameObject);

        _onEnterScene = false;
    }
    public override void OnLeaveScene()
    {
        if (!_onLeaveScene)
        {
            Debug.Log("OnLeaveScene was already called once for current instance");
            return;
        }
        // Controls
        SetActive(false, _dropHook.gameObject);
        // Game Time
        SetActive(false, _gameTimerBoard.gameObject, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, _hookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);

        _onEnterScene = false;
    }
    public override void HookScoreToggle(bool pBool)
    {
        _hookScoreText.gameObject.SetActive(pBool);
    }

    public void SetScreenPosition(GameObject pTheObject, GameObject pAccordingTo, Vector3 pOffset)
    {
        Vector3 accordingTo = Camera.main.WorldToScreenPoint(pAccordingTo.transform.position);
        Vector3 position = accordingTo;// + pOffset;
        pTheObject.transform.position = position;
    }

    public static bool GetTouchedReelUp()
    {
        return touchedReelUpHook;
    }
}