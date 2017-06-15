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
    private Image _Arrows;
    [SerializeField]
    private Image _handMove;

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

    public override void Start()
    {
        Debug.Log("TutorialUiI Start");
        // Controls
        SetActive(false, _dropHook.gameObject, _reelHook.gameObject);
        // Game Time
        SetActive(false, _gameTimerBoard.gameObject, _gameTimerText.gameObject);
        // Score
        SetActive(false, _totalScoreBoard.gameObject, _totalScoreText.gameObject, _hookScoreText.gameObject);
        // Shopping List
        SetActive(false, _shoppingList.gameObject);
        //Debug.Log("LevelUI - Start();");
    }
    public void OnDropHook()
    {
        if (!GameManager.Boat.CanDropHook()) return;

        SetActive(false, _dropHook.gameObject);
        SetActive(true, _reelHook.gameObject);
        GameManager.Boat.SetState(boat.BoatState.Fish);
    }
    public void OnReelHook()
    {
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
        // Controls
        SetActive(true, _dropHook.gameObject);
        //Add animation
        // Game Time
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
}