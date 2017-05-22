using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoatState : AbstractBoatState {
    private float _acceleration;
    private float _maxVelocity;
    private float _deceleration;
    private float _velocity;
    private float _rotationLerpSpeed;

    private Vector3 _destination = Vector3.zero;
    private float polarity;
    private float direction;
    private bool turning;

    public MoveBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration, float pRotationLerpSpeed) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
        polarity = 0.0f;
        direction = 0.0f;
        _rotationLerpSpeed = pRotationLerpSpeed;
        turning = false;
    }


    public override void Start()
    {
        if (basic.GlobalUI.InTutorial && !basic.GlobalUI.MoveBoatCompleted)
        {
            basic.GlobalUI.MoveBoatCompleted = true;
            basic.GlobalUI.ShowHandSwipe(false);
            basic.GlobalUI.SwitchHookButtons();
            basic.GlobalUI.InTutorial = false;
        }
    }
    public override void Update()
    {
        setPolarity();
        if (!MoveToDestination())
        {
            Debug.Log("Switching to another state");
            basic.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.Stationary);          
        }
    }
    private void setPolarity()
    {
        polarity = Mathf.Sign(mouse.GetWorldPoint().x - _boat.gameObject.transform.position.x);
        if (direction == 0.0f)
        {
            matchDirectionToPolarity();
        }
    }

    private IEnumerator RotateToOppositeDirection()
    {
        return null;
    }

    private void matchDirectionToPolarity()
    {
        direction = polarity;
        turning = true;
    }
    private void SetDestination(Vector3 pPosition)
    {
        if (!Input.GetMouseButton(0)) return;
        _destination = new Vector3(pPosition.x - _boat.gameObject.transform.position.x, _boat.gameObject.transform.position.y, _boat.gameObject.transform.position.z);
    }
    private bool MoveToDestination()
    {
        if (turning == false)
        {
            Vector3 target = mouse.GetWorldPoint();
            target.x = basic.Boat.gameObject.transform.position.x;
            target.y = basic.Boat.gameObject.transform.position.y;
            basic.Boat.gameObject.transform.rotation = Quaternion.Slerp(basic.Boat.gameObject.gameObject.transform.rotation, Quaternion.LookRotation(target - basic.Boat.gameObject.gameObject.transform.position), _rotationLerpSpeed * Time.deltaTime);
            if (Input.GetMouseButton(0) && _velocity < _maxVelocity && direction == polarity)
            {
                _velocity += _acceleration;
            }
            else if (_velocity > 0 || _velocity >= _maxVelocity /*|| direction != polarity*/)
            {
                _velocity -= _deceleration;
                if (_velocity <= 0)
                {
                    matchDirectionToPolarity();
                }
            }
            if (!Input.GetMouseButton(0) && _velocity < 0)
            {
                _velocity = 0;
            }

            Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
            Vector2 m = new Vector3((direction * _velocity), 0.0f, 0.0f);
            _boat.gameObject.transform.Translate(m);
        }
        return (_velocity > 0 || Input.GetMouseButton(0));
        
    }
    public override void Refresh()
    {
        _destination = _boat.gameObject.transform.position;
        _velocity = 0;
        polarity = 0.0f;
        direction = 0.0f;
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
