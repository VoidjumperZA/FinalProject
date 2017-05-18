using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpBoatState : AbstractBoatState
{
    private float _acceleration;
    private float _velocity = 0;
    private float _maxVelocity;
    private float _deceleration;
    private Vector3 _destination;
    private float _halfDestination;

    public SetUpBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration, Vector3 pDestination) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
        _destination = pDestination;
    }

    public override void Start()
    {
        _halfDestination = (_destination - _boat.gameObject.transform.position).magnitude / 2;
    }

    public override void Update()
    {
        Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
        if (differenceVector.magnitude > _halfDestination) _velocity += _acceleration;
        else _velocity -= _acceleration;
        if (_velocity > _maxVelocity) _velocity = _maxVelocity;
        if (_velocity < 0 || differenceVector.magnitude <= _velocity)
        {
            _velocity = 0;
            _boat.gameObject.transform.position = _destination;
            SetState(boat.BoatState.Stationary);
        }
        _boat.transform.Translate(differenceVector.normalized * _velocity);
    }
    public override void Refresh()
    {
        basic.Tempfishspawn._boatSetUp = true;
        basic.GlobalUI.DeployHookButton(true);
    }

    public override boat.BoatState StateType()
    {
        return boat.BoatState.SetUp;
    }
}
