using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : general
{
    // States
    private Dictionary<BoatState, AbstractBoatState> _stateCache = new Dictionary<BoatState, AbstractBoatState>();
    private AbstractBoatState _abstractState = null;
    public enum BoatState { None, LeaveScene, EnterScene, EnterBoundary, LeaveBoundary, RotateInBoundary, Stationary, Move, Rotate, Fish}
    [SerializeField] private BoatState _boatState = BoatState.None;
    // Radar
    private radar _radar = null;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _rotationDuration;
    [SerializeField] private GameObject _boatModel;
    private Vector3 _setUpPosition;
    private Quaternion rightFacingRotation;
    private Quaternion leftFacingRotation;


    public Transform ContainerSpawner;
    public override void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitializeStateMachine();
        //basic.Trailer = GetComponent<trailer>();
        //Debug.Log("Boat - Start();");
    }
    public override void Update()
    {
        _abstractState.Update();
        //Debug.Log(_abstractState.StateType());
    }

    public Dictionary<BoatState, AbstractBoatState> GetStateCache()
    {
        return _stateCache;
    }

    public AbstractBoatState GetAbstractState()
    {
        return _abstractState;
    }
    public AbstractBoatState GetState(BoatState pState)
    {
        return _stateCache[pState];
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
        _stateCache[BoatState.Stationary] = new StationaryBoatState(this);
        _stateCache[BoatState.Move] = new MoveBoatState(this, _acceleration, _maxVelocity, _deceleration);
        _stateCache[BoatState.Rotate] = new RotateBoatState(this, _rotationDuration, _boatModel);
        _stateCache[BoatState.LeaveScene] = new LeaveSceneBoatState(this, _acceleration, _maxVelocity, _deceleration);
        _stateCache[BoatState.EnterScene] = new EnterSceneBoatState(this, _acceleration, _maxVelocity, _deceleration);
        _stateCache[BoatState.EnterBoundary] = new EnterBoundaryBoatState(this, _acceleration, _maxVelocity, _deceleration);
        _stateCache[BoatState.LeaveBoundary] = new LeaveBoundaryBoatState(this, _acceleration, _maxVelocity, _deceleration);
        _stateCache[BoatState.RotateInBoundary] = new RotateInBoundaryBoatState(this, _rotationDuration, _boatModel);
        _stateCache[BoatState.Fish] = new FishBoatState(this);
        SetState(_boatState);
    }
    public bool CanDropHook()
    {
        return (_abstractState is StationaryBoatState || _abstractState is MoveBoatState);
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
    
    public void SetEnterStateDestination(Vector3 pDestination)
    {
        GetState(BoatState.EnterScene).SetDestination(pDestination);
    }
    public void SetLeaveStateDestination(Vector3 pDestination)
    {
        GetState(BoatState.LeaveScene).SetDestination(pDestination);
    }
    public override void FinalizeInitialization()
    {
        GetState(BoatState.Move).FinalizeInitialization();
    }
    public void SetMoveBoatStateBoundaries(Vector3[] pBoundaries)
    {
        GetState(BoatState.LeaveBoundary).SetBoundaries(pBoundaries);
    }
}
