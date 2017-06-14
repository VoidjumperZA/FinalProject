using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shark : general {

    private Dictionary<SharkState, AbstractSharkState> _stateCache = new Dictionary<SharkState, AbstractSharkState>();
    private AbstractSharkState _abstractState = null;
    public enum SharkState { None, Approach, Steer, Hunt }
    [SerializeField] private SharkState _sharkState = SharkState.None;

    [SerializeField] private Transform _sharkSpawn;
    [SerializeField] private Transform _approachTransform;
    [SerializeField] private Transform _steerTransform;
    [SerializeField] private float _approachDuration;
    [SerializeField] private float _steerDuration;
    [SerializeField] private float _swimSpeed;



    public override void Start () {
        InitializeStateMachine();
	}
	public override void FixedUpdate () {
        _abstractState.Update();
        //Debug.Log(_abstractState.StateType());
	}
    public void SetState(SharkState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }
    private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[SharkState.None] = new NoneSharkState(this);
        _stateCache[SharkState.Approach] = new ApproachSharkState(this, _sharkSpawn, _approachTransform, _approachDuration);
        _stateCache[SharkState.Steer] = new SteerSharkState(this, _approachTransform, _steerTransform, _steerDuration);
        _stateCache[SharkState.Hunt] = new HuntSharkState(this, _swimSpeed);
        SetState(_sharkState);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }
}
