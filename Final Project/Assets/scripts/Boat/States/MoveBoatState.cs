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
    private float polarity;
    private float direction;

    public MoveBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
        polarity = 0.0f;
        direction = 0.0f;
    }

    public override void Start()
    {
        if (!basic.GlobalUi.MoveBoatCompleted)
        {
            basic.GlobalUi.MoveBoatCompleted = true;
            basic.GlobalUi.SwitchHookButtons();
        }
    }
    public override void Update()
    {
        setPolarity();
        if (!MoveToDestination())
        {
            basic.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.Stationary);
            
        }
    }
    private void setPolarity()
    {
        polarity = Mathf.Sign(mouse.GetWorldPoint().x - _boat.gameObject.transform.position.x);
        if (direction == 0.0f)
        {
            direction = polarity;
        }
    }
    private void SetDestination(Vector3 pPosition)
    {
        if (!Input.GetMouseButton(0)) return;
        _destination = new Vector3(pPosition.x - _boat.gameObject.transform.position.x, _boat.gameObject.transform.position.y, _boat.gameObject.transform.position.z);
    }
    private bool MoveToDestination()
    {
        if (Input.GetMouseButton(0) && _velocity < _maxVelocity)
        {
            _velocity += _acceleration;
        }
        else if (_velocity > 0 || _velocity >= _maxVelocity || Mathf.Sign(_velocity) != polarity)
        {
            _velocity -= _deceleration;
            if (_velocity <= 0)
            {
                direction = polarity;
            }
        }
        if (!Input.GetMouseButton(0) && _velocity < 0)
        {
            _velocity = 0;
        }

        Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
        Vector2 m = new Vector3((direction * _velocity), 0.0f, 0.0f);
        _boat.gameObject.transform.Translate(m);
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
