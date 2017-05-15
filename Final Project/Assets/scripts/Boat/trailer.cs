using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailer : general
{
    // States
    /*private Dictionary<BoatState, AbstractBoatState> _stateCache = new Dictionary<BoatState, AbstractBoatState>();
    private AbstractBoatState _abstractState = null;*/
    public enum TrailerState { None, Move }
    [SerializeField] private TrailerState _trailerState = TrailerState.None;
    [HideInInspector] public List<general> StuffOnTrailer = new List<general>();
    [SerializeField] private Transform[] _storagePoints;
    public override void Start()
    {
        //InitializeStateMachine();
    }
    public override void Update()
    {
        for (int i = 0; i < StuffOnTrailer.Count; i++) StuffOnTrailer[i].gameObject.transform.position = gameObject.transform.position + new Vector3(0, i, 0);
        //_abstractState.Update();

    }
    /*public void SetState(BoatState pState)
    {
        if (_abstractState != null) _abstractState.Refresh();
        _abstractState = _stateCache[pState];
        _abstractState.Start();
    }*/
    /*private void InitializeStateMachine()
    {
        _stateCache.Clear();
        _stateCache[BoatState.None] = new NoneBoatState(this);
        _stateCache[BoatState.Move] = new MoveBoatState(this, _acceleration);
        _stateCache[BoatState.Fish] = new FishBoatState(this);
        SetState(_boatState);
    }*/
    public void OnTriggerEnter(Collider other)
    {
        //if (other && _abstractState != null) _abstractState.OnTriggerEnter(other);
    }
}
