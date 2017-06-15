using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveBoundaryBoatState : AbstractBoatState
{
    private Vector3 _leftDetector;
    private Vector3 _rightDetector;
    private Vector3 _toReachDetector;

    private float _acceleration;
    private float _maxVelocity;
    private float _deceleration;
    private float _velocity = 0;
    private float _halfDestination;

    public LeaveBoundaryBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
    }

    public override void Start()
    {
        _halfDestination = (_toReachDetector - _boat.transform.position).magnitude / 2;
    }

    public override void Update()
    {
        ApplyVelocity();

    }
    public override void Refresh()
    {
        _toReachDetector = Vector3.zero;
        _halfDestination = 0;
        _velocity = 0;
    }
    private void ApplyVelocity()
    {
        Vector3 differenceVector = _toReachDetector - _boat.transform.position;
        if (differenceVector.magnitude > _halfDestination)
        {
            _velocity += _acceleration;
            if (_velocity >= _maxVelocity) _velocity = _maxVelocity;
        }
        else if (differenceVector.magnitude <= _halfDestination)
        {
            _velocity -= _deceleration;
            if (_velocity < 0) _velocity = 0;
        }
        if (differenceVector.magnitude < _deceleration || _velocity == 0)
        {
            _boat.transform.position = _toReachDetector;
            SetState(boat.BoatState.Stationary);
        }
        _boat.gameObject.transform.Translate(differenceVector.normalized * _velocity);
    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.LeaveBoundary;
    }
    public override void SetBoundaries(Vector3[] pBoundaries)
    {
        _leftDetector = pBoundaries[0];
        _rightDetector = pBoundaries[1];
    }
    public override void LeftOrRight(bool pLeftTrueRightFalse)
    {
        _toReachDetector = (pLeftTrueRightFalse) ? _leftDetector : _rightDetector;
    }
}
