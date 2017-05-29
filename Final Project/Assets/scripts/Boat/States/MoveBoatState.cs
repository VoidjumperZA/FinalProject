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

    private Vector3 lerpTarget;
    private Transform previousCamHolder;
    private float polarity;
    private float direction;
    private bool turning;

    private int currentRot;
    private int targetRot;
    private int rotSpeed;
    private Quaternion targetQua;
    private GameObject boatModel;

    public MoveBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration, float pRotationLerpSpeed) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
        _rotationLerpSpeed = pRotationLerpSpeed;
        SetUpState();
    }

    private void SetUpState()
    {
        boatModel = GameObject.FindGameObjectWithTag("BoatModel");
        basic.Camerahandler.SetViewPoint(CameraHandler.CameraFocus.Boat);
        _velocity = 0.0f;
        polarity = 0.0f;
        direction = 1.0f;
        turning = false;

        if (boatModel.gameObject.transform.rotation == basic.Boat.GetBoatEndRotations(true))
        {
            direction = 1.0f;
            targetRot = 180;
            targetQua = basic.Boat.GetBoatEndRotations(true);
            //Debug.Log("Set Up State. Direction: " + direction + "  |  targetQua: Right");
        }
        else
        {
            direction = -1.0f;
            targetRot = -180;
            targetQua = basic.Boat.GetBoatEndRotations(false);
            //Debug.Log("Set Up State. Direction: " + direction + "  |  targetQua: Left");
        }
        currentRot = 0;
        rotSpeed = GameObject.Find("Manager").GetComponent<GameplayValues>().GetBoatRotationSpeed();
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

    public float GetBoatVelocity()
    {
        return _velocity;
    }
    public override void Update()
    {
        setPolarity();

        //Debug.Log("Direction: " + direction);
        if (!MoveToDestination() && turning == false)
        {
            //Debug.Log("Switching to another state");
            basic.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.Stationary);          
        }

        if (turning == false)
        {
        //Debug.Log("Polarity: " + polarity + "\t|\tDirection: " + direction);

        }
    }
    private void setPolarity()
    {
        polarity = Mathf.Sign(mouse.GetWorldPoint().x - _boat.gameObject.transform.position.x);
        if (direction != polarity && Input.GetMouseButton(0) == true && _velocity == 0.0f)
        {
            Debug.Log("One.");
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
        prepareToRotate();
    }

    private bool MoveToDestination()
    {
        //don't move the boat while it is turning as it will then traverse along incorrect axes
        if (turning == false)
        {         
            //while clicking, under max velocity and heading where directed 
            if (Input.GetMouseButton(0) && _velocity < _maxVelocity && direction == polarity)
            {
                //accelerate
                _velocity += _acceleration;
            }
            //if moving OR at max velocity
            else if (_velocity > 0 || _velocity >= _maxVelocity /*|| direction != polarity*/)
            {
                //decelerate
                _velocity -= _deceleration;
                //if we're at zero check where we're being told to turn around
                if (_velocity <= 0 && direction != polarity)
                {
                    Debug.Log("Two.");
                    matchDirectionToPolarity();
                }
            }
            //if not input or min vel
            if (!Input.GetMouseButton(0) && _velocity < 0)
            {
                _velocity = 0;
            }

            Vector2 m = new Vector3(_velocity * direction, 0.0f, 0.0f);
            _boat.gameObject.transform.Translate(m);
        }
        else
        {
            rotate();
        }
        return (/*_velocity > 0 || */Input.GetMouseButton(0));
        
    }

    //
    private void rotate()
    {
        //Debug.Log("Rotating.");
        boatModel.gameObject.transform.rotation = Quaternion.RotateTowards(boatModel.gameObject.transform.rotation, targetQua, rotSpeed * Time.deltaTime);

        if (boatModel.gameObject.transform.rotation == targetQua)
        {
            Camera.main.transform.SetParent(previousCamHolder);
            turning = false;
            Debug.Log("Turning is false");
        }
    }

    //
    private void prepareToRotate()
    {
        //Debug.Log("Preparing to rotate.");
        turning = true;
        targetQua = targetQua == _boat.GetBoatEndRotations(true) ? _boat.GetBoatEndRotations(false) : _boat.GetBoatEndRotations(true);
        previousCamHolder = Camera.main.transform.parent.transform;
        Camera.main.transform.SetParent(GameObject.FindGameObjectWithTag("TopLevelCamHolder").transform);
    }


    public override void Refresh()
    {
        SetUpState();
    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.Move;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FishingArea")
        {
            GameObject.Find("Manager").GetComponent<TempFishSpawn>().CalculateNewSpawnDensity();
        }          
    }
}
