using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;

public class CameraHandler : MonoBehaviour
{
    private bool _isAboveWater = true; public bool IsAboveWater { get { return _isAboveWater; } }
    [Header("PostProcessingProfiles")]
    public Transform SeaSurface;
    [SerializeField] private PostProcessingProfile _aboveWaterProfile;
    [SerializeField] private PostProcessingProfile _underWaterProfile;
    private PostProcessingBehaviour _cameraPostProcessing { get { return _camera.GetComponent<PostProcessingBehaviour>(); } }
    // Scene Transition Camera Holders
    private Transform _startCamHolder;
    private Transform _middleCamHolder;
    private Transform _endCamHolder;


    private PostEffectsBase _globalFog { get { return _camera.GetComponent<PostEffectsBase>(); } }

    private bool _initialized = false;
    private bool _play = false;
    private Camera _camera { get { return Camera.main; } }

    //Camera zoom levels and focus points
    public enum FocusPoint { Start, Middle, End, BoatCloseUp, Ocean, Hook, TopLevel };
    public FocusPoint _focusPoint = FocusPoint.Ocean;
    public FocusPoint _previousFocusObject;
    private Dictionary<FocusPoint, Transform> _parentPoints;
    private Dictionary<FocusPoint, Transform> _lookAtPoints;
    private bool _focusPointReached = true;

    private float _currentLerpTime = 0;
    private float _totalLerpTime;
    private Vector3 _fromPointPosition;
    private Quaternion _fromPointRotation;
    // ----------------------------
    private List<Vector3> _shakePoints = new List<Vector3>();
    [Header("Screen shake")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float _shakePointDistance;
    [SerializeField]
    [Range(1, 10)]
    private int _maxShakePoints;
    [SerializeField]
    private float _shakeSpeed;
    private float _currentShakeTime;
    [SerializeField]
    private bool _applyJellyFeel;
    [SerializeField]
    private float _menuToOceanDuration = 3;
    [SerializeField]
    private float _oceanToHookDuration = 1;
    [SerializeField]
    private float _hookToOceanDuration = 1;

    public void Play()
    {
        if (!_play && _initialized) _play = true;
    }
    public void InitializeCameraHandler()
    {
        DontDestroyOnLoad(_camera.gameObject);
        _totalLerpTime = _menuToOceanDuration;

        _parentPoints = new Dictionary<FocusPoint, Transform>();
        _parentPoints[FocusPoint.Start] = _startCamHolder;
        _parentPoints[FocusPoint.Middle] = _middleCamHolder;
        _parentPoints[FocusPoint.End] = _endCamHolder;
        _parentPoints[FocusPoint.BoatCloseUp] = GameObject.FindGameObjectWithTag("BoatCamHolder").transform;
        _parentPoints[FocusPoint.Ocean] = GameObject.FindGameObjectWithTag("OceanCamHolder").transform;
        _parentPoints[FocusPoint.Hook] = GameObject.FindGameObjectWithTag("HookCamHolder").transform;
        _parentPoints[FocusPoint.TopLevel] = GameObject.FindGameObjectWithTag("TopLevelCamHolder").transform;
        DontDestroyOnLoad(_parentPoints[FocusPoint.TopLevel].gameObject);  

        _lookAtPoints = new Dictionary<FocusPoint, Transform>();
        _lookAtPoints[FocusPoint.BoatCloseUp] = GameManager.Boat.transform;
        _lookAtPoints[FocusPoint.Ocean] = GameManager.Boat.transform;
        _lookAtPoints[FocusPoint.Hook] = GameManager.Hook.transform;
        _lookAtPoints[FocusPoint.TopLevel] = _lookAtPoints[FocusPoint.Ocean];

        _shakePoints = new List<Vector3>();
        _initialized = true;
        Debug.Log("CameraHandeler initialized: " + _initialized);
    }
    public void ClassUpdate()
    {
        IfCrossedSurface();
        if (!_play) return;
        if (!_initialized)
        {
            Debug.Log("CameraHandler: Can not run update, static class was not initialized!");
            return;
        }
        ReachFocusPoint();
        ReachShakePoint();
    }
    private void IfCrossedSurface()
    {
        if (!SeaSurface) return;

        float val = SeaSurface.position.y + 1.5f;
        if (_isAboveWater && _camera.transform.position.y <= val)
        {
            ToggleBelowWater(true);
            ToggleHookScoreText(true);
        }
        else if (!_isAboveWater && _camera.transform.position.y >= val)
        {
            ToggleBelowWater(false);
            ToggleHookScoreText(false);
        }
    }
    public void ToggleBelowWater(bool pBool)
    {
        // Fog
        _globalFog.enabled = pBool;
        //RenderSettings.fog = pBool;
        // Color Correction Profile ?
        if (_aboveWaterProfile) _cameraPostProcessing.profile = _aboveWaterProfile;
        // HookScoreText UI
        _isAboveWater = !pBool;
    }
    public void ToggleHookScoreText(bool pBool)
    {

        if (GameManager.Levelmanager._baseUI) GameManager.Levelmanager._baseUI.HookScoreToggle(pBool);
    }
    public void SetViewPoint(FocusPoint pFocusPoint, bool pOverrideTransform = false)
    {
        //Debug.Log("Calling camera to set to " + pFocusObject.ToString());
        if (!_initialized)
        {
            Debug.Log("CameraHandler: Can not run update, static class was not initialized!");
            return;
        }
        SetLerpTime(pFocusPoint);
        if (pFocusPoint == FocusPoint.TopLevel)
        {
            _parentPoints[FocusPoint.TopLevel].position = _parentPoints[_focusPoint].position;
            _parentPoints[FocusPoint.TopLevel].rotation = _parentPoints[_focusPoint].rotation;
        }


        SetFocusPoint(pFocusPoint);
        // Set _focusPoint as parent
        SetCameraParent(_parentPoints[_focusPoint]);
        // Set _fromPoint
        SetFromPoint(_camera.transform);
        _focusPointReached = false;
        if (pOverrideTransform)
        {
            _camera.transform.position = _parentPoints[_focusPoint].position;
            _camera.transform.rotation = _parentPoints[_focusPoint].rotation;
            SetFromPoint(_camera.transform);
            _focusPointReached = true;
        }
        _currentLerpTime = 0;
    }
    public void ReachFocusPoint()
    {
        if (!_focusPointReached)
        {
            _currentLerpTime += Time.deltaTime;
            if (_currentLerpTime <= _totalLerpTime)
            {
                float lerp = _currentLerpTime / _totalLerpTime;
                _camera.transform.position = Vector3.Lerp(_fromPointPosition, _parentPoints[_focusPoint].position, lerp);
                _camera.transform.rotation = Quaternion.Lerp(_fromPointRotation, _parentPoints[_focusPoint].rotation, lerp);
            }
            else
            {
                _focusPointReached = true;
                _currentLerpTime = 0;
             //   Debug.Log("Just Reached it");
            }
        }
    }
    private void ReachShakePoint()
    {
        if (_focusPointReached && _shakePoints.Count > 0)
        {
            Vector3 destination = _parentPoints[_focusPoint].position + _shakePoints[0];
            Vector3 differenceVector = destination - _camera.transform.position;
            if (differenceVector.magnitude >= _shakeSpeed) _camera.transform.Translate(differenceVector.normalized * _shakeSpeed);
            else
            {
                _shakePoints.RemoveAt(0);
                _camera.transform.position = destination;
                if (_shakePoints.Count == 0) _focusPointReached = false;
                Debug.Log(_shakePoints.Count + " PointsAmount");
            }
        }
    }
    public void CreateShakePoint()
    {
        return;
        _shakePoints.Add(Vector3.zero);
        for (int i = 0; i < _maxShakePoints; i++)
        {
            float slider = _shakePointDistance;
            Vector3 offset = new Vector3(Random.Range(-slider, slider), Random.Range(-slider, slider), 0);
            _shakePoints.Add(offset);
        }
    }
    public void StartMiddleEndCameraHolder(Transform pStart, Transform pMiddle, Transform pEnd)
    {
        _startCamHolder = pStart; if (!_startCamHolder) Debug.Log("StartNUll");
        _middleCamHolder = pMiddle; if (!_middleCamHolder) Debug.Log("MiddleNUll");
        _endCamHolder = pEnd; if (!_endCamHolder) Debug.Log("EndNUll");



        _parentPoints[FocusPoint.Start] = _startCamHolder;
        _parentPoints[FocusPoint.Middle] = _middleCamHolder;
        _parentPoints[FocusPoint.End] = _endCamHolder;
    }
    private void SetFocusPoint(FocusPoint pFocusPoint)
    {
        _previousFocusObject = _focusPoint;
        _focusPoint = pFocusPoint;
    }
    private void SetCameraParent(Transform pTransform)
    {
        _camera.transform.SetParent(pTransform);
    }
    private void SetFromPoint(Transform pTransform)
    {
        _fromPointPosition = _camera.transform.position;
        _fromPointRotation = _camera.transform.rotation;
    }
    private void SetLerpTime(FocusPoint pFocusPoint)
    {
        // From Start to Ocean
        if (_focusPoint == FocusPoint.Start && pFocusPoint == FocusPoint.End) _totalLerpTime = _menuToOceanDuration;
        // From Ocean to Hook
        if (_focusPoint == FocusPoint.Ocean && pFocusPoint == FocusPoint.Hook) _totalLerpTime = _oceanToHookDuration;
        // From Hook to Ocean
        if (_focusPoint == FocusPoint.Hook && pFocusPoint == FocusPoint.Ocean) _totalLerpTime = _hookToOceanDuration;
        // From Ocean to End
        if (_focusPoint == FocusPoint.Ocean && pFocusPoint == FocusPoint.End) _totalLerpTime = _menuToOceanDuration;
    }
}