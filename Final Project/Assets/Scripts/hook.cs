﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hook : general
{
    Camera mainCam;
    // Fishing
    private boat _boat;
    private bool _fishing = false;
    List<GameObject> fishAttachedToHook = new List<GameObject>();
    float fishRotationAngle = 25.0f;

    // Movement
    [SerializeField] private float _ropeLength;
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    private Vector3 _xyOffset;
    private Vector3 _velocity;
    private float hookRotationAmount;
    private float maxHookRotation;
    private float currentHookRotation;
    private Vector2 shakePolarities;
    // X Velocity damping
    [SerializeField] private float _xOffsetDamping;
    // States
    public enum HookState { None, Fish, Reel, SetFree }
    private HookState _hookState = HookState.None;

    [SerializeField] private Button _reelButton;

    //Screen shake
    private bool camShaking;
    private float screenShakeIntensity;
    private int screenShakeDuration;
    private int screenShakeCounter;
    private GameObject manager;
    public override void Start()
    {
        mainCam = Camera.main;
        if (!GameObject.Find("Manager"))
        {
            Debug.Log("WARNING: Manager not found.");
        }
        manager = GameObject.Find("Manager");
        base.Start();
        hookRotationAmount = 1.0f;
        currentHookRotation = 0.0f;
        maxHookRotation = 25.0f;

        camShaking = false;
        screenShakeCounter = 0;
        screenShakeDuration = manager.GetComponent<GameplayValues>().GetScreenShakeDuration();
        screenShakeIntensity = manager.GetComponent<GameplayValues>().GetScreenShakeIntensity();
    }
    public override void Update()
    {
        //Debug.Log(_hookState + " Hook");
        if (_selected)
        {
            StateNoneUpdate();
            StateFishUpdate();
            
        }
        else
        {
            DampXVelocityAfterDeselected();
            StateReelUpdate();
            StateSetFreeStateUpdate();
        }
        ApplyVelocity();
        if (camShaking == true)
        {
            shakeCameraOnCollect();
        }
        SetCameraAndHookAngle();
    }
    private void shakeCameraOnCollect()
    {
        screenShakeCounter++;
        if (screenShakeCounter >= screenShakeDuration)
        {
            camShaking = false;
            screenShakeCounter = 0;
            CameraHandler.ResetScreenShake(true);
        }
    }
    private void StateNoneUpdate()
    {
        if (_hookState == HookState.None)
        {
        }
    }

    //Fishing state
    private void StateFishUpdate()
    {
        if (_hookState == HookState.Fish)
        {
            if (Input.GetMouseButton(0))
            {
                 SetXYAxisOffset(mouse.Instance.GetWorldPoint());
                GameObject.Find("Manager").GetComponent<InputTimer>().ResetClock();
            }
        }
    }

    //Set free state
    private void StateSetFreeStateUpdate()
    {
        if (_hookState == HookState.SetFree)
        {
            //Temporarily turn the boat into a trigger to stop collisions
            _boat.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;

            Debug.Log("Entered SetFree Update");
            for (int i = 0; i < fishAttachedToHook.Count; i++)
            {
                fishAttachedToHook[i].GetComponent<fish>().Release();
                Rigidbody fishRigidBod = fishAttachedToHook[i].GetComponent<Rigidbody>();
                fishRigidBod.isKinematic = false;                
                fishRigidBod.AddForceAtPosition(new Vector3(/*Random.Range(-5.0f, 5.0f)*/0.0f, 50.0f, 0.0f), gameObject.transform.position - (Vector3.down * 2.0f), ForceMode.Impulse);
                //fishRigidBod.AddExplosionForce(100000.0f, gameObject.transform.position, 1000.0f, 1.0f);
            }
            fishAttachedToHook.Clear();
            _boat.GetComponent<BoxCollider>().isTrigger = false;
            gameObject.GetComponent<Rigidbody>().detectCollisions = true;
            _hookState = HookState.None;
            _boat.SetState(boat.BoatState.None);
                //Debug.Log("Switching cam parent to boat.");
                CameraHandler.SetParent(GameObject.FindGameObjectWithTag("BoatCamHolder").transform);
        }          
    }

    private void StateReelUpdate()
    {
        if (_hookState == HookState.Reel)
        {
            Vector3 differenceVector = (_boat.gameObject.transform.position - gameObject.transform.position);
            if (differenceVector.magnitude >= _speed) gameObject.transform.Translate(differenceVector.normalized * _speed);
            if (differenceVector.magnitude < _speed)
            {
                gameObject.transform.position = _boat.transform.position;
                _hookState = HookState.SetFree;
            }
        }
    }
    // -------- Fishing --------
    public void DeployHook()
    {
        if (_hookState == HookState.None)
        {
            _fishing = true;
            _hookState = HookState.Fish;
        }
    }
    // -------- Movement --------
    private void ApplyVelocity()
    {
        if (!_fishing) return;

        _velocity = new Vector3(_xyOffset.x * _speed, Mathf.Min(_xyOffset.y * _speed / 2, -_fallSpeed), 0);
        gameObject.transform.Translate(_velocity);
       

    }
    private void SetCameraAndHookAngle()
    {
        if (_xyOffset.x < 0)
        {
            if (currentHookRotation < maxHookRotation)
            {
                currentHookRotation += hookRotationAmount;
                gameObject.transform.Rotate(0.0f, 0.0f, hookRotationAmount);
                Camera.main.transform.Rotate(0.0f, 0.0f, -hookRotationAmount);
            }
        }
        else if (_xyOffset.x > 0)
        {
            if (currentHookRotation > -maxHookRotation)
            {
                currentHookRotation -= hookRotationAmount;
                gameObject.transform.Rotate(0.0f, 0.0f, -hookRotationAmount);
                Camera.main.transform.Rotate(0.0f, 0.0f, hookRotationAmount);
            }
        }
    }
    private void SetXYAxisOffset(Vector3 pPosition)
    {
        _xyOffset = new Vector3(pPosition.x - gameObject.transform.position.x, pPosition.y - gameObject.transform.position.y, 0);
        _xyOffset.Normalize();
    }
    // X Velocity damping
    private void DampXVelocityAfterDeselected()
    {
        _xyOffset *= _xOffsetDamping;
    }
    // -------- General Script Override --------
    public override void Select()
    {
        if (!_fishing) return;
        base.Select();
        //Debug.Log("hook - Select() " + _selected);
        _hookState = HookState.Fish;
        _xyOffset = Vector3.zero;

    }
    public override void Deselect()
    {
        base.Deselect();
        //Debug.Log("hook - Deselect() " + _selected);
        if (_fishing) _hookState = HookState.Fish;
    }
    private void ReelUpTheHook()
    {
        _hookState = HookState.Reel;
        _fishing = false;
        Deselect();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_hookState == HookState.Fish)
        {            
            //Reel the hook in if you touch the floor
            if (other.gameObject.CompareTag("Floor"))
            {
                ReelUpTheHook();
            }

            //On contact with a fish
            if (other.gameObject.CompareTag("Fish"))
            {
                //Screen shake
                CameraHandler.ApplyScreenShake(true);
                camShaking = true;

                //ATTACH FISH TO HOOK
                //Rotate the fish by a small degree
                float fishAngle = Random.Range(-fishRotationAngle, fishRotationAngle);
                other.gameObject.transform.Rotate(fishAngle, 0.0f, 0.0f);
                other.gameObject.GetComponent<BoxCollider>().enabled = false;

                //The attached fish are now tracked in a list and also follow the hook
                other.gameObject.GetComponent<fish>().Catch(gameObject);
                fishAttachedToHook.Add(other.gameObject);

                //ADDING SCORE
                Debug.Log("Detecting Fish");
                fish.FishType type = other.gameObject.GetComponent<fish>().GetFishType();
                if (type == fish.FishType.Small)
                {
                    GameObject.Find("Manager").GetComponent<ScoreHandler>().AddScore(10, true);
                }
                if (type == fish.FishType.Medium)
                {
                    GameObject.Find("Manager").GetComponent<ScoreHandler>().AddScore(50, true);
                }
                if (type == fish.FishType.Large)
                {
                    GameObject.Find("Manager").GetComponent<ScoreHandler>().AddScore(150, true);
                    ReelUpTheHook();
                }
                if (type == fish.FishType.Hunted)
                {
                    GameObject.Find("Manager").GetComponent<ScoreHandler>().AddScore(500, true);
                }
                
            }
        }
    }
    public void AssignBoat(boat pBoat)
    {
        _boat = pBoat;
    }
    public void SetState(HookState pState)
    {
        _hookState = pState;
    }
}
