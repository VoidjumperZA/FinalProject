using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : general
{
    // States
    private Dictionary<BoatState, AbstractBoatState> _stateCache = new Dictionary<BoatState, AbstractBoatState>();
    private AbstractBoatState _abstractState = null;
    public enum BoatState { None, Move, Fish }
    [SerializeField] private BoatState _boatState = BoatState.None;
    // Radar
    private Radar _radar = null;
    // Fishing
    private hook _hook = null;
    [SerializeField] private float _acceleration;
    public override void Start()
    {
        InitializeStateMachine();
    }
    public override void Update()
    {
        _abstractState.Update();
        
    }
    public void SetState(BoatState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[BoatState.None] = new NoneBoatState(this);
        _stateCache[BoatState.Move] = new MoveBoatState(this, _acceleration);
        _stateCache[BoatState.Fish] = new FishBoatState(this);
        SetState(_boatState);
    }
    public void AssignHook(hook pHook)
    {
        _hook = pHook;
    }
    public void AssignRadar(Radar pRadar)
    {
        _radar = pRadar;
        _radar.gameObject.transform.SetParent(gameObject.transform);
    }
}
