using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;

public class CameraHandler : MonoBehaviour
{
    private bool _isAboveWater = false; public bool IsAboveWater { get { return _isAboveWater; } }
    [SerializeField] private Transform _seaSurface;
    [SerializeField] private PostProcessingProfile _aboveWaterProfile;
    [SerializeField] private PostProcessingProfile _underWaterProfile;
    private PostProcessingBehaviour _cameraPostProcessing { get { return _camera.GetComponent<PostProcessingBehaviour>(); } }
    private PostEffectsBase _globalFog { get { return _camera.GetComponent<PostEffectsBase>(); } }

    private bool _initialized = false;
    private Camera _camera { get { return Camera.main; } }

    //Camera zoom levels and focus points
    public enum CameraFocus { Boat, Ocean, Hook, TopLevel };
    public CameraFocus _focusObject;
    public CameraFocus _previousFocusObject;
    private Dictionary<CameraFocus, Transform> _parentPoints;
    private Dictionary<CameraFocus, Transform> _lookAtPoints;
    private float _shakeSpeed;
    private float _currentShakeTime;
    private bool _viewPointReached = true;

    private float _currentSlerpTime = 0;
    private float _totalSlerpTime;
    private Vector3 _fromPosition;
    private Quaternion _fromRotation;
    // ----------------------------
    private List<Vector3> _shakePoints = new List<Vector3>();

    public void InitializeCameraHandler()
    {
        _shakeSpeed = basic.Gameplayvalues.GetShakeSpeed();
        _totalSlerpTime = basic.Gameplayvalues.MenuToOcean();

        _parentPoints = new Dictionary<CameraFocus, Transform>();
        _parentPoints[CameraFocus.Boat] = GameObject.FindGameObjectWithTag("BoatCamHolder").transform;
        _parentPoints[CameraFocus.Ocean] = GameObject.FindGameObjectWithTag("OceanCamHolder").transform;
        _parentPoints[CameraFocus.Hook] = GameObject.FindGameObjectWithTag("HookCamHolder").transform;
        _parentPoints[CameraFocus.TopLevel] = _parentPoints[CameraFocus.Ocean];

        _lookAtPoints = new Dictionary<CameraFocus, Transform>();
        _lookAtPoints[CameraFocus.Boat] = basic.Boat.transform;
        _lookAtPoints[CameraFocus.Ocean] = basic.Boat.transform;
        _lookAtPoints[CameraFocus.Hook] = basic.Hook.transform;
        _lookAtPoints[CameraFocus.TopLevel] = _lookAtPoints[CameraFocus.Ocean];

        _shakePoints = new List<Vector3>();
        _initialized = true;
        Debug.Log("CameraHandeler initialized: " + _initialized);
    }
    public void ClassUpdate()
    {
        if (!_initialized)
        {
            Debug.Log("CameraHandler: Can not run update, static class was not initialized!");
            return;
        }
        ReachViewPoint();
        ReachShakePoint();
        IfCrossedSurface();
    }
    private void IfCrossedSurface()
    {
        if (!_seaSurface) return;

        float val = _seaSurface.position.y + 0.5f;
        if (_isAboveWater && _camera.transform.position.y <= val)
        {
            _globalFog.enabled = true;
            RenderSettings.fog = true;
            if (_underWaterProfile) _cameraPostProcessing.profile = _underWaterProfile;
            _isAboveWater = false;
        }
        else if (!_isAboveWater && _camera.transform.position.y >= val)
        {
            _globalFog.enabled = false;
            RenderSettings.fog = false;
            if (_aboveWaterProfile) _cameraPostProcessing.profile = _aboveWaterProfile;
            _isAboveWater = true;
        }
    }
    public void SetViewPoint(CameraFocus pFocusObject, bool pFirstTime = false)
    {
        //Debug.Log("Calling camera to set to " + pFocusObject.ToString());
        if (!_initialized)
        {
            Debug.Log("CameraHandler: Can not run update, static class was not initialized!");
            return;
        }
        if (_focusObject == CameraFocus.Boat && pFocusObject == CameraFocus.Ocean) _totalSlerpTime = basic.Gameplayvalues.MenuToOcean();
        if (_focusObject == CameraFocus.Ocean && pFocusObject == CameraFocus.Hook) _totalSlerpTime = basic.Gameplayvalues.OceanToHook();
        if (_focusObject == CameraFocus.Hook && pFocusObject == CameraFocus.Ocean) _totalSlerpTime = basic.Gameplayvalues.HookToOcean();



        _previousFocusObject = _focusObject;
        if (pFirstTime == true)
        {
            _previousFocusObject = pFocusObject;
        }
        _focusObject = pFocusObject;

        _fromPosition = _camera.transform.position;
        _fromRotation = _camera.transform.rotation;

        _camera.transform.SetParent(_parentPoints[_focusObject]);
        _viewPointReached = false;
        if (pFirstTime)
        {
            _camera.transform.position = _parentPoints[_focusObject].position;
            _camera.transform.rotation = _parentPoints[_focusObject].rotation;
            _fromPosition = _camera.transform.position;
            _fromRotation = _camera.transform.rotation;
            _viewPointReached = true;
        }
        _currentSlerpTime = 0;
    }
    public void ReachViewPoint()
    {
        if (!_viewPointReached)
        {
            _currentSlerpTime += Time.deltaTime;
            if (_currentSlerpTime <= _totalSlerpTime)
            {
                float lerp = _currentSlerpTime / _totalSlerpTime;
                _camera.transform.position = Vector3.Lerp(_fromPosition, _parentPoints[_focusObject].position, lerp);
                _camera.transform.rotation = Quaternion.Lerp(_fromRotation, _parentPoints[_focusObject].rotation, lerp);
            }
            else
            {
                _viewPointReached = true;
                _currentSlerpTime = 0;
             //   Debug.Log("Just Reached it");
            }
        }
    }
    private void ReachShakePoint()
    {
        if (_viewPointReached && _shakePoints.Count > 0)
        {
            Vector3 destination = _parentPoints[_focusObject].position + _shakePoints[0];
            Vector3 differenceVector = destination - _camera.transform.position;
            if (differenceVector.magnitude >= _shakeSpeed) _camera.transform.Translate(differenceVector.normalized * _shakeSpeed);
            else
            {
                _shakePoints.RemoveAt(0);
                _camera.transform.position = destination;
                if (_shakePoints.Count == 0) _viewPointReached = false;
                Debug.Log(_shakePoints.Count + " PointsAmount");
            }
        }
    }
    public void CreateShakePoint()
    {
        return;
        _shakePoints.Add(Vector3.zero);
        for (int i = 0; i < basic.Gameplayvalues.GetMaxShakePoints(); i++)
        {
            float slider = basic.Gameplayvalues.GetShakePointDistance();
            Vector3 offset = new Vector3(Random.Range(-slider, slider), Random.Range(-slider, slider), 0);
            _shakePoints.Add(offset);
        }
    }
}