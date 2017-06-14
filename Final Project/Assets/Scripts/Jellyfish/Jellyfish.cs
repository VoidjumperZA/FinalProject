using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : general
{
    // States
    private Dictionary<JellyfishState, AbstractJellyfishState> _stateCache = new Dictionary<JellyfishState, AbstractJellyfishState>();
    private AbstractJellyfishState _abstractState = null;
    public enum JellyfishState { None, Swim}
    [SerializeField]
    private JellyfishState _jellyfishState = JellyfishState.None;

    //Movement
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _lerpSpeed;
    
    // Radar related
    [SerializeField]
    private SkinnedMeshRenderer _renderer;
    [SerializeField]
    private cakeslice.Outline _outliner;
    [SerializeField]
    private float _revealDuration;
    [HideInInspector]
    public Animator Animator;
    
    //Variable just for visualizign the target point
    //public GameObject _point;
    
    // Use this for initialization
    public override void Start ()
    {
        Animator = GetComponent<Animator>();
        InitializeStateMachine();
}
	
	// Update is called once per frame
	public override void FixedUpdate ()
    {
        _abstractState.Update();
    }
 
    public void SetState(JellyfishState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }

    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[JellyfishState.None] = new NoneJellyfishState(this);
        _stateCache[JellyfishState.Swim] = new SwimJellyfishState(this, _movementSpeed, _lerpSpeed ,_revealDuration);
        SetState(_jellyfishState);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }

   /* public override void ToggleOutliner(bool pBool)
    {
        _outliner.enabled = pBool;
    }
    public override void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
        _renderer.enabled = pBool;
    }*/

    public override void Reveal(float pRevealDuration, int pCollectableStaysVisibleRange)
    {
        /*if (Revealed) return;

        ToggleOutliner(true);
        ToggleRenderer(true);
        Revealed = true;*/
    }
    public override void Hide()
    {
        /*ToggleOutliner(false);
        ToggleRenderer(false);
        Revealed = false;*/
    }
}
