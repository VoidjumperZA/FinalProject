using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : general
{
    // States
    private Dictionary<FishState, AbstractFishState> _stateCache = new Dictionary<FishState, AbstractFishState>();
    private AbstractFishState _abstractState = null;
    public enum FishState { None, Swim, FollowHook, PiledUp }
    [SerializeField]
    private FishState _fishState = FishState.None;
    // Fish type related
    [SerializeField]
    private int _score;
    [SerializeField]
    private float _speed;
    public enum FishType { Small, Medium, Large };
    public FishType fishType;
    // Radar related
    [SerializeField] public SkinnedMeshRenderer _renderer;
    [HideInInspector]
    public Material _material;
    [HideInInspector]
    public Color _color;
    // [SerializeField] private cakeslice.Outline _outliner;

    [SerializeField]
    private ParticleSystem _bubbles;
    [HideInInspector]
    public Animator Animator;
    [SerializeField]
    public GameObject[] Joints;

    private float _revealDuration;

    // Use this for initialization
    public override void Start()
    {
        _material = _renderer.material;
        _color = _material.color;

       // _bubbles.gameObject.SetActive(false);
        Animator = GetComponent<Animator>();
        InitializeStateMachine();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        _abstractState.Update();
    }
    public void SetState(FishState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[FishState.None] = new NoneFishState(this);
        _stateCache[FishState.Swim] = new SwimFishState(this, _speed);
        _stateCache[FishState.FollowHook] = new FollowHookFishState(this);
        _stateCache[FishState.PiledUp] = new PiledUpFishState(this);
        SetState(_fishState);
    }

    public void SetFishType(FishType pType)
    {
        fishType = pType;
    }

    public void SetFishType(int pType)
    {
        fishType = (FishType)pType;
    }

    public FishType GetFishType()
    {
        return fishType;
    }
    /*public override void ToggleOutliner(bool pBool)
    {
        _outliner.enabled = pBool;
    }
    public override void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
        _renderer.enabled = pBool;
    }*/
    public void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }
    public override void Reveal(float pFadeOutDuration, int pCollectableStaysVisibleRange)
    {
        if (Revealed) return;
        Revealed = true;

        SwimFishState swimFishState = _stateCache[FishState.Swim] as SwimFishState;
        //if (swimFishState is SwimFishState) //Debug.Log("SWIMFISHSTATE !NULL");
        //swimFishState.ResetOutLineCounter(pFadeOutDuration, pCollectableStaysVisibleRange);

        ToggleBubbles(true);
        //ToggleOutliner(true);
        //ToggleRenderer(true);
    }
    public override void Hide()
    {
        GameManager.Fishspawner.QueueFishAgain(this, true, true, true);
    }
    public int GetScore()
    {
        return _score;
    }
    private void ToggleBubbles(bool pBool)
    {
        /*if (!_bubbles) return;
        _bubbles.gameObject.SetActive(pBool);*/
    }
}
