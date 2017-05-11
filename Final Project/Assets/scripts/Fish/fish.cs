using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : general
{
    // States
    private Dictionary<FishState, AbstractFishState> _stateCache = new Dictionary<FishState, AbstractFishState>();
    private AbstractFishState _abstractState = null;
    public enum FishState { None, Swim, FollowHook, PiledUp }
    [SerializeField] private FishState _fishState = FishState.None;
    // Fish type related
    [SerializeField] private int _score;
    [SerializeField] private float _speed;
    public enum FishType { Small, Medium, Large};
    [SerializeField]
    [Range(0, 100)]
    private int _rarity;
    public FishType fishType;
    // Radar related
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private cakeslice.Outline _outliner;
    [SerializeField] private float _revealDuration;
    [HideInInspector] public Animator Animator;

    // Use this for initialization
    public override void Start()
    {
        Animator = GetComponent<Animator>();
        InitializeStateMachine();
    }

    // Update is called once per frame
    public override void Update()
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
        _stateCache[FishState.Swim] = new SwimFishState(this, _speed, _revealDuration);
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

    public int GetFishRarity()
    {
        return _rarity;
    }
    public void SetDirection(float pPolarity)
    {
        _speed *= pPolarity;
    }
    public override void ToggleOutliner(bool pBool)
    {
        _outliner.enabled = pBool;
    }
    public override void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
        _renderer.enabled = pBool;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }
    public override void Reveal()
    {
        if (Revealed) return;

        ToggleOutliner(true);
        ToggleRenderer(true);
        Revealed = true;
    }
    public override void Hide()
    {
        ToggleOutliner(false);
        ToggleRenderer(false);
        Revealed = false;
    }
    public int GetScore()
    {
        return _score;
    }
}
