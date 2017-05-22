using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : general
{
    // States
    private Dictionary<BoatState, AbstractBoatState> _stateCache = new Dictionary<BoatState, AbstractBoatState>();
    private AbstractBoatState _abstractState = null;
    public enum BoatState { None, SetUp, Stationary, Move, Fish }
    [SerializeField] private BoatState _boatState = BoatState.None;
    // Radar
    private radar _radar = null;
    // Fishing
    private hook _hook = null;
    private trailer _trailer = null;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _rotationLerpSpeed;
    private Vector3 _setUpPosition;
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
        _stateCache[BoatState.SetUp] = new SetUpBoatState(this, _acceleration, _maxVelocity, _deceleration, _setUpPosition);
        _stateCache[BoatState.Stationary] = new StationaryBoatState(this);
        _stateCache[BoatState.Move] = new MoveBoatState(this, _acceleration, _maxVelocity, _deceleration, _rotationLerpSpeed);
        _stateCache[BoatState.Fish] = new FishBoatState(this);
        SetState(_boatState);
    }
    public void AssignHook(hook pHook)
    {
        _hook = pHook;
    }
    public void AssignRadar(radar pRadar)
    {
        _radar = pRadar;
        _radar.gameObject.transform.SetParent(gameObject.transform);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }
    public void SetSetUpPosition(Vector3 pPosition)
    {
        _setUpPosition = pPosition;
    }
}
