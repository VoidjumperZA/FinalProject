using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hook : general
{
    // Class references
    private GameObject _manager;
    private InputTimer _inputTimer;
    private GameplayValues _gameplayValues;
    private GlobalUI _globalUI;
    private Camera _mainCamera;
    // Components
    [SerializeField] private Rigidbody _rigidBody;
    
    // Fishing
    private boat _boat;
    private bool _fishing = false;
    List<GameObject> fishAttachedToHook = new List<GameObject>();
    float fishRotationAngle = 25.0f;

    // Movement
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    private Vector3 _xyOffset;
    private Vector3 _velocity;
    // X Velocity damping
    [SerializeField] private float _xOffsetDamping;
    // Rotation
    private float hookRotationAmount;
    private float maxHookRotation;
    private float currentHookRotation;
    // States
    public enum HookState { None, Fish, Reel, SetFree }
    private HookState _hookState = HookState.None;

    [SerializeField] private Button _reelButton;

    //Screen shake
    private bool camShaking;
    private int screenShakeDuration;
    private int screenShakeCounter;

    private bool valid;
    public override void Start()
    {
        valid = true;
        base.Start();
        // Assign class references
        _manager = GameObject.Find("Manager"); if (!_manager) { Debug.Log("ERROR: Manager can not be found."); valid = false; }
        _inputTimer = _manager.GetComponent<InputTimer>(); if (!_inputTimer) { Debug.Log("ERROR: Manager is missing the InputTimer script."); valid = false; }
        _gameplayValues = _manager.GetComponent<GameplayValues>(); if (!_gameplayValues) { Debug.Log("ERROR: Manager is missing the GameplayValues script."); valid = false; }
        _globalUI = _manager.GetComponent<GlobalUI>(); if (!_globalUI) { Debug.Log("ERROR: Manager is missing the GlobalUI script."); valid = false; }
        _mainCamera = Camera.main; if (!_mainCamera) { Debug.Log("ERROR: No main camera in the scene."); valid = false; }

        hookRotationAmount = 1.0f;
        currentHookRotation = 0.0f;
        maxHookRotation = 25.0f;

        camShaking = false;
        screenShakeCounter = 0;

        screenShakeDuration = _gameplayValues.GetScreenShakeDuration();
    }

    //
    public override void Update()
    {
        if (valid == true)
        {
            /*Debug.Log(_hookState + " Hook");
            Debug.Log(_fishing + " :Fishing");*/
            if (_selected)
            {
                StateNoneUpdate();

            }
            else
            {
                DampXVelocityAfterDeselected();
                StateSetFreeStateUpdate();
            }
            StateReelUpdate();
            StateFishUpdate();
            ApplyVelocity();
            if (camShaking == true)
            {
                //shakeCameraOnCollect();
            }
           // SetCameraAndHookAngle();
        }
        else
        {
            Debug.Log("ERROR: Hook.cs is missing integral script references, cannot continue.");
        }
        //Debug.Log(_xyOffset.ToString() + " :Offset");    
    }

    //
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

    //
    private void StateNoneUpdate()
    {
        if (_hookState == HookState.None)
        {
            _fishing = false;
            _xyOffset = Vector2.zero;
            currentHookRotation = 0;
        }
    }

    //Fishing state
    private void StateFishUpdate()
    {
        if (_hookState == HookState.Fish && _fishing)
        {
            if (Input.GetMouseButton(0))
            {
                 SetXYAxisOffset(mouse.GetWorldPoint());
                _inputTimer.ResetClock();
            }
        }
    }

    //Set free state, to do any operation once returning from fish, before
    //automatically switching to the None state
    private void StateSetFreeStateUpdate()
    {
        if (_hookState == HookState.SetFree)
        {
            Debug.Log("Entered SetFree Update");
            GameObject.Find("Manager").GetComponent<ScoreHandler>().BankScore();
            for (int i = 0; i < fishAttachedToHook.Count; i++)
            {
                fishAttachedToHook[i].GetComponent<fish>().Release();
                Rigidbody fishRigidBody = fishAttachedToHook[i].GetComponent<Rigidbody>();
                fishRigidBody.isKinematic = false;                
                fishRigidBody.AddForceAtPosition(new Vector3(/*Random.Range(-5.0f, 5.0f)*/0.0f, 50.0f, 0.0f), gameObject.transform.position - (Vector3.down * 2.0f), ForceMode.Impulse);
                //fishRigidBod.AddExplosionForce(100000.0f, gameObject.transform.position, 1000.0f, 1.0f);
            }
            fishAttachedToHook.Clear();
           
            _hookState = HookState.None;
            basic.Boat.SetState(boat.BoatState.None);
            _globalUI.SwitchHookButtons();

            CameraHandler.SetCameraFocusPoint(CameraHandler.CameraFocus.FocusBoat, true);
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
        //Debug.Log(_velocity.ToString() + " :Velocity");
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
                gameObject.transform.Rotate(0.0f, 0.0f, currentHookRotation);
                Camera.main.transform.Rotate(0.0f, 0.0f, -currentHookRotation);
            }
        }
        else if (_xyOffset.x > 0)
        {
            if (currentHookRotation > -maxHookRotation)
            {
                currentHookRotation -= hookRotationAmount;
                gameObject.transform.Rotate(0.0f, 0.0f, -currentHookRotation);
                Camera.main.transform.Rotate(0.0f, 0.0f, currentHookRotation);
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
        if (_xyOffset.magnitude > 0)_xyOffset *= _xOffsetDamping;
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
    public void ReelUpTheHook()
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
                //other.gameObject.GetComponent<fish>().Catch(gameObject);
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
                    //ReelUpTheHook();
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
