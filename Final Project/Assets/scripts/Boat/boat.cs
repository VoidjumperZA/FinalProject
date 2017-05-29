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
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _rotationLerpSpeed;
    private Vector3 _setUpPosition;
    private Quaternion rightFacingRotation;
    private Quaternion leftFacingRotation;


    public Transform ContainerSpawner;
    public override void Start()
    {
        rightFacingRotation = gameObject.transform.rotation;
        GameObject goTemp = new GameObject();
        goTemp.transform.rotation = rightFacingRotation;
        goTemp.transform.Rotate(0.0f, 180, 0.0f);
        leftFacingRotation = goTemp.transform.rotation;
        InitializeStateMachine();
        basic.Trailer = GetComponent<trailer>();
    }
    public override void Update()
    {
        _abstractState.Update();
        
    }

    public Dictionary<BoatState, AbstractBoatState> GetStateCache()
    {
        return _stateCache;
    }

    public AbstractBoatState GetAbstractState()
    {
        return _abstractState;
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

    public Quaternion GetBoatEndRotations(bool pTrueRightFalseLeft)
    {
        if (pTrueRightFalseLeft == true)
        {
            Debug.Log("Returning targetQua Right");
            return rightFacingRotation;
        }
        else
        {
            Debug.Log("Returning targetQua Left");
            return leftFacingRotation;
        }
    }
}
