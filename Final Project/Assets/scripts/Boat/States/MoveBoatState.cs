using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoatState : AbstractBoatState {
    private float _acceleration;
    private float _maxVelocity;
    private float _deceleration;
    private float _velocity;

    private Vector3 _destination = Vector3.zero;

    public MoveBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
    }

    public override void Start()
    {
    }
    public override void Update()
    {
        SetDestination(mouse.GetWorldPoint());
        if (!MoveToDestination())
        {
            basic.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.Stationary);
            
        }
    }
    private void SetDestination(Vector3 pPosition)
    {
        if (!Input.GetMouseButton(0)) return;
        _destination = new Vector3(pPosition.x, _boat.gameObject.transform.position.y, _boat.gameObject.transform.position.z);
    }
    private bool MoveToDestination()
    {
        if (Input.GetMouseButton(0) && _velocity < _maxVelocity) _velocity += _acceleration;
        else if (_velocity > 0 || _velocity >= _maxVelocity) _velocity -= _deceleration;
        if (_velocity < 0) _velocity = 0;

        Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
        _boat.gameObject.transform.Translate(differenceVector.normalized * _velocity);
        return (differenceVector.magnitude > _velocity);
    }
    public override void Refresh()
    {
        _destination = _boat.gameObject.transform.position;
        _velocity = 0;
    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.Move;
    }

    public override void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Overriding");
        if (other.gameObject.tag == "FishingArea")
        {
            GameObject.Find("Manager").GetComponent<TempFishSpawn>().CalculateNewSpawnDensity();
        }
            
    }
}
