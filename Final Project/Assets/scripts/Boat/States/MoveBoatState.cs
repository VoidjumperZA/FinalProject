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
    private Vector3 lerpTarget;
    private Transform previousCamHolder;
    private float polarity;
    private float direction;
    private bool turning;

    private int currentRot;
    private int targetRot;
    private int rotSpeed;
    private Quaternion targetQua;

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
        _velocity = 0.0f;
        polarity = 0.0f;
        direction = 1.0f;
        turning = false;

        if (_boat.gameObject.transform.rotation == basic.Boat.GetBoatEndRotations(true))
        {
            direction = 1.0f;
            targetRot = 180;
            targetQua = basic.Boat.GetBoatEndRotations(false);
        }
        else
        {
            direction = -1.0f;
            targetRot = -180;
            targetQua = basic.Boat.GetBoatEndRotations(true);
        }
        currentRot = 0;
        rotSpeed = 1;
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
        //Debug.Log("Direction: " + direction);
        if (!MoveToDestination() && turning == false)
        {
            //Debug.Log("Switching to another state");
            basic.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.Stationary);          
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
        Debug.Log("Direction: " + direction);
    }
    private void SetDestination(Vector3 pPosition)
    {
        if (!Input.GetMouseButton(0)) return;
        _destination = new Vector3(pPosition.x - _boat.gameObject.transform.position.x, _boat.gameObject.transform.position.y, _boat.gameObject.transform.position.z);
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
                if (_velocity <= 0)
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

            Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
            Vector2 m = new Vector3(_velocity, 0.0f, 0.0f);
            _boat.gameObject.transform.Translate(m);
        }
        else
        {
            rotate();
        }
        return (_velocity > 0 || Input.GetMouseButton(0));
        
    }

    //
    private void rotate()
    {
        //currentRot += rotSpeed * (int)Mathf.Sign(targetRot);
        _boat.gameObject.transform.Rotate(new Vector3(0.0f, rotSpeed /*.* Mathf.Sign(targetRot)*/, 0.0f));
        /*
        basic.Boat.gameObject.transform.rotation = Quaternion.Slerp(basic.Boat.gameObject.gameObject.transform.rotation, Quaternion.LookRotation(lerpTarget - basic.Boat.gameObject.gameObject.transform.position), _rotationLerpSpeed * Time.deltaTime);
        //slerp to there

        //if we are back set our camera back to it's previous owner*/
        if (/*basic.Boat.gameObject.transform.rotation == Quaternion.LookRotation(lerpTarget)*/ _boat.gameObject.transform.rotation == targetQua)
        {
            Camera.main.transform.SetParent(previousCamHolder);

            //targetQua = targetQua == _boat.GetBoatEndRotations(true) ? _boat.GetBoatEndRotations(false) : _boat.GetBoatEndRotations(true);
            turning = false;
        }
    }

    //
    private void prepareToRotate()
    {
        //shitty rotation
        turning = true;
        targetQua = targetQua == _boat.GetBoatEndRotations(true) ? _boat.GetBoatEndRotations(false) : _boat.GetBoatEndRotations(true);
        previousCamHolder = Camera.main.transform.parent.transform;
        Camera.main.transform.SetParent(GameObject.FindGameObjectWithTag("TopLevelCamHolder").transform);
    }


    public override void Refresh()
    {
        _destination = _boat.gameObject.transform.position;
        SetUpState();
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
