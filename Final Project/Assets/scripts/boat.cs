using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : general {
    // Fishing
    [SerializeField] private GameObject _hookPrefab;
    private hook _hook = null;
    // Movement
    [SerializeField] private float _speed;
    private Vector3 _destination;
    // Action recognizion
    private counter _counter;
    // States
    public enum BoatState { None, Move, Fish }
    private BoatState _boatState = BoatState.None;
    public override void Start()
    {
        base.Start();
        _counter = new counter(0.3f);
        // After inicialization
    }
    public override void Update()
    {
        if (_selected)
        {
            StateNoneUpdate();
            StateMoveUpdate();
            StateFishUpdate();
        }
    }
    public general SpawnHook()
    {
        _hook = Instantiate(_hookPrefab, gameObject.transform.position, Quaternion.identity).GetComponent<hook>();
        return _hook;
    }
    // -------- State Machine --------
    private void StateNoneUpdate()
    {
        if (_boatState == BoatState.None)
        {
            _counter.Count();
            if (_counter.Done())
            {
                _boatState = SidewaysOrDownwards() ? BoatState.Move : BoatState.Fish;
            }
        }
    }
    private void StateMoveUpdate()
    {
        if (_boatState == BoatState.Move)
        {
            if (Input.GetMouseButton(0)) SetDestination(mouse.Instance.GetWorldPoint());
            MoveToDestination();
        }
    }
    private void StateFishUpdate()
    {
        if (_boatState == BoatState.Fish)
        {
            _hook.DeployHook();
            Debug.Log("Fishing");
        }
    }
    // -------- Action Recognizion --------
    private bool SidewaysOrDownwards()
    {
        Vector3 mouseWorldPoint = mouse.Instance.GetWorldPoint();
        return Mathf.Abs(mouseWorldPoint.x - gameObject.transform.position.x) >= Mathf.Abs(mouseWorldPoint.y - gameObject.transform.position.y);
    }
    // -------- Movement --------
    private void MoveToDestination()
    {
        if (_destination == Vector3.zero) return;
        Vector3 differenceVector = _destination - gameObject.transform.position;
        if (differenceVector.magnitude >= _speed)
        {
            gameObject.transform.Translate(differenceVector.normalized * _speed);
            _hook.gameObject.transform.Translate(differenceVector.normalized * _speed);
        }

    }
    private void SetDestination(Vector3 pPosition)
    { 
        // Debug.Log(pPosition.ToString() + " mouseWorldPoint");
        _destination = new Vector3(pPosition.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    // -------- Fishing --------
    // -------- General Script Override --------
    public override void Select()
    {
        base.Select();
        Debug.Log("boat - Select() " + _selected);
        _boatState = BoatState.None;
        _counter.Reset();

    }
    public override void Deselect()
    {
        base.Deselect();
        Debug.Log("boat - Deselect() " + _selected);
        _boatState = BoatState.None;
        _counter.SetActive(false);
    }
}
