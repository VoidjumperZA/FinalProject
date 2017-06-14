using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBoundaryBoatState : AbstractBoatState
{
    private float _acceleration;
    private float _maxVelocity;
    private float _deceleration;
    private float _velocity = 0;

    public EnterBoundaryBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration) : base(pBoat)
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
        Decelerate();
        ApplyVelocity();
        
    }
    public override void Refresh()
    {

    }

    private void Decelerate()
    {
        if (_velocity > 0)
        {
            _velocity -= _deceleration;
            if (_velocity < 0) _velocity = 0;
        }
        else if (_velocity < 0)
        {
            _velocity += _deceleration;
            if (_velocity > 0) _velocity = 0;
        }
        if (_velocity == 0) SetState(boat.BoatState.RotateInBoundary);
    }
    private void ApplyVelocity()
    {
        _boat.gameObject.transform.Translate(new Vector3(_velocity, 0, 0));
    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.EnterBoundary;
    }
    public override void SetVelocity(float pVelocity)
    {
        _velocity = pVelocity;
    }
}