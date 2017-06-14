using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoatState : AbstractBoatState
{

    private float _acceleration;
    private float _maxVelocity;
    private float _deceleration;
    private float _velocity;
    private Vector3 _destination;
    private float _halfDestination;

    private int _prevPolarity = 1;
    private int _polarity = 1;
    private bool _rotate = false;
    private float _direction;

    public MoveBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
        
    }
    public override void FinalizeInitialization()
    {

    }

    public override void Start()
    {
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Ocean);
        /*if (basic.GlobalUI.InTutorial && !basic.GlobalUI.MoveBoatCompleted)
        {
            basic.GlobalUI.MoveBoatCompleted = true;
            basic.GlobalUI.ShowHandSwipe(false);
            basic.GlobalUI.SwitchHookButtons();
            basic.GlobalUI.InTutorial = false;
        }*/
    }

    public float GetBoatVelocity()
    {
        return _velocity;
    }
    public override void Update()
    {
        _direction = (_velocity > 0) ? 1 : ((_velocity < 0) ? -1 : 0);
        float xDiff = GetXDifference();
        if (!Accelerate(xDiff)) Decelerate(xDiff);
        ApplyVelocity();

        if (_velocity == 0)
        {
            GameManager.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.Stationary);
        }
    }
    private float GetXDifference()
    {
        float xDifference = 0;
        if (!mouse.Touching()) return xDifference;
        xDifference = mouse.GetWorldPoint().x - _boat.transform.position.x;
        xDifference = Mathf.Clamp(xDifference, -1.0f, 1.0f);
        return xDifference;
    }
    private bool Accelerate(float pXDifference)
    {
        DetectRotation(pXDifference);
        if (pXDifference == 0) return false;
        _velocity += _acceleration * pXDifference;
        if (_velocity > _maxVelocity) _velocity = _maxVelocity;
        return true;
    }
    private void Decelerate(float pXDifference)
    {
        if (_velocity > 0)
        {
            _velocity -= _deceleration;
            if (_velocity < 0)
            {
                _velocity = 0;
            }
        }
        else if (_velocity < 0)
        {
            _velocity += _deceleration;
            if (_velocity > 0)
            {
                _velocity = 0;
            }
        }
    }
    private void ApplyVelocity()
    {
        _boat.gameObject.transform.Translate(new Vector3(_velocity, 0.0f, 0.0f));
    }
    
    public override void Refresh()
    {
        _velocity = 0;
    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.Move;
    }

    public override void OnTriggerEnter(Collider other)
    {
        //Fishing Area
        /*if (other.gameObject.tag == "FishingArea")
        {
            GameObject.Find("Manager").GetComponent<TempFishSpawn>().CalculateNewSpawnDensity();
        }*/

        //Level Boundary
        if (other.gameObject.tag == "LeftDetector")
        {
            Debug.Log("Left");
            _polarity = 1;
            GetState(boat.BoatState.EnterBoundary).SetVelocity(_velocity);
            GetState(boat.BoatState.LeaveBoundary).LeftOrRight(true);
            SetState(boat.BoatState.EnterBoundary);
        }
        if (other.gameObject.tag == "RightDetector")
        {
            Debug.Log("Right");
            _polarity = -1;
            GetState(boat.BoatState.EnterBoundary).SetVelocity(_velocity);
            GetState(boat.BoatState.LeaveBoundary).LeftOrRight(false);
            SetState(boat.BoatState.EnterBoundary);
        }
    }
    private void DetectRotation(float pXDifference)
    {
        if (_polarity > 0 && pXDifference < 0)
        {
            _polarity = -1;
            SetState(boat.BoatState.Rotate);
        }
        if (_polarity < 0 && pXDifference > 0)
        {
            _polarity = 1;
            SetState(boat.BoatState.Rotate);
        }
    }
    private void Rotate()
    {
        if (_rotate)
        {
            _prevPolarity = _polarity;
            _rotate = false;
            SetState(boat.BoatState.Rotate);
        }
    }
}