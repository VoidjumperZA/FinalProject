using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : BaseUI {
    private bool _onEnterScene = true;
    private bool _onLeaveScene = true;
    [Header("Controls")]
    [SerializeField] private Button _dropHook;
    [SerializeField] private Button _reelHook;
    [Header("Game Time")]
    [SerializeField] private Image _gameTimerBoard;
    [SerializeField] private Text _gameTimerText;
    [Header("Score")]
    [SerializeField] private Image _totalScoreBoard;
    [SerializeField] private Text _totalScoreText;
    [SerializeField] private Text _hookScoreText;
    [Header("Shopping List")]
    [SerializeField] private Image _shoppingList;

    public override void Start () {
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
    public override void Update()
    {
        // Game Time
        _gameTimerText.text = GameManager.Gametimer.GetFormattedTimeLeftAsString();
        // Score
        _totalScoreText.text = GameManager.Scorehandler.BankedScore + "";

        _hookScoreText.text = GameManager.Scorehandler.HookScore + "";
        _hookScoreText.transform.position = GameManager.Scorehandler.HookScorePosition();
    }
    public void OnDropHook()
    {
        if (!GameManager.Boat.CanDropHook()) return;

        SetActive(false, _dropHook.gameObject);
        SetActive(true, _reelHook.gameObject, _hookScoreText.gameObject);
        GameManager.Boat.SetState(boat.BoatState.Fish);
    }
    public void OnReelHook()
    {
        SetActive(true, _dropHook.gameObject);
        SetActive(false, _reelHook.gameObject, _hookScoreText.gameObject);
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
        // Game Time
        GameManager.Gametimer.BeginCountdown();
        SetActive(true, _gameTimerBoard.gameObject, _gameTimerText.gameObject);

        // Score
        SetActive(true, _totalScoreBoard.gameObject, _totalScoreText.gameObject);
        // Shopping List
        SetActive(true, _shoppingList.gameObject);



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

}
