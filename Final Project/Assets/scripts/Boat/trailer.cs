using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct tempLowPolyFish
{
    private fish.FishType _type;
    public tempLowPolyFish(fish.FishType pType)
    {
        _type = pType;
    }
}

public class trailer : general
{
    [HideInInspector] public List<fish.FishType> _lowPolyFish;
    [SerializeField]
    private float _interval = 2.0f;
    private counter _spawnInterval;
    // States
    /*private Dictionary<BoatState, AbstractBoatState> _stateCache = new Dictionary<BoatState, AbstractBoatState>();
    private AbstractBoatState _abstractState = null;*/
    /*public enum TrailerState { None, Move }
    [SerializeField] private TrailerState _trailerState = TrailerState.None;
    [HideInInspector] public List<general> StuffOnTrailer = new List<general>();
    [SerializeField] private Transform[] _storagePoints;*/
    public override void Start()
    {
        _lowPolyFish = new List<fish.FishType>();
        _spawnInterval = new counter(_interval);
        _spawnInterval.SetActive(true);
        //InitializeStateMachine();
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) Instantiate(basic.LowPolyFish, basic.Boat.ContainerSpawner.position, basic.Boat.ContainerSpawner.rotation);
        if (_lowPolyFish.Count > 0)
        {
            _spawnInterval.Count();
            if (_spawnInterval.Done())
            {
                Instantiate(basic.LowPolyFish, basic.Boat.ContainerSpawner.position, basic.Boat.ContainerSpawner.rotation);

                Debug.Log("Spawned: " + basic.LowPolyFish.name);
                _lowPolyFish.RemoveAt(0);
                _spawnInterval.Reset();
            }
        }
        //_abstractState.Update();
    }
    public void Add(fish.FishType pType)
    {
        _lowPolyFish.Add(pType);
        Debug.Log("Added: " + pType);
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
