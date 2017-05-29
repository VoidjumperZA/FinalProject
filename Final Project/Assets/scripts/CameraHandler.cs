using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;

public class CameraHandler : MonoBehaviour
{
    private bool _isAboveWater = false;
    [SerializeField] private Transform _seaSurface;
    [SerializeField] private PostProcessingProfile _aboveWaterProfile;
    [SerializeField] private PostProcessingProfile _underWaterProfile;
    private PostProcessingBehaviour _cameraPostProcessing { get { return _camera.GetComponent<PostProcessingBehaviour>(); } }
    private PostEffectsBase _globalFog { get { return _camera.GetComponent<PostEffectsBase>(); } }

    private bool _initialized = false;
    private GameObject _manager { get { return GameObject.Find("Manager"); } }
    private Camera _camera { get { return Camera.main; } }

    //Camera zoom levels and focus points
    public enum CameraFocus { Boat, Ocean, Hook };
    public CameraFocus _focusObject;
    private Dictionary<CameraFocus, Transform> _parentPoints;
    private Dictionary<CameraFocus, Transform> _lookAtPoints;
    private float _cameraSpeed { get { return basic.Gameplayvalues.GetCameraSpeed(); } }
    private bool _viewPointReached = true;
    private bool _viewRotationReached = true;

    private float _currentSlerpTime = 0;
    private float _totalSlerpTime;
    // ----------------------------
    private List<Vector3> _shakePoints = new List<Vector3>();

    public void InitializeCameraHandler()
    {
        _totalSlerpTime = basic.Gameplayvalues.GetLerpDuration();

        _parentPoints = new Dictionary<CameraFocus, Transform>();
        _parentPoints[CameraFocus.Boat] = GameObject.FindGameObjectWithTag("BoatCamHolder").transform;
        _parentPoints[CameraFocus.Ocean] = GameObject.FindGameObjectWithTag("OceanCamHolder").transform;
        _parentPoints[CameraFocus.Hook] = GameObject.FindGameObjectWithTag("HookCamHolder").transform;

        _lookAtPoints = new Dictionary<CameraFocus, Transform>();
        _lookAtPoints[CameraFocus.Boat] = basic.Boat.transform;
        _lookAtPoints[CameraFocus.Ocean] = basic.Boat.transform;
        _lookAtPoints[CameraFocus.Hook] = basic.Hook.transform;

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
        if (!_initialized)
        {
            Debug.Log("CameraHandler: Can not run update, static class was not initialized!");
            return;
        }
        _focusObject = pFocusObject;
        _camera.transform.SetParent(_parentPoints[_focusObject]);
        _viewPointReached = false;
        _viewRotationReached = false;
        if (pFirstTime)
        {
            _camera.transform.position = _parentPoints[_focusObject].position;
            _viewPointReached = true;
            _viewRotationReached = true;
        }
        _currentSlerpTime = 0;
    }
    public void ReachViewPoint()
    {
        if (!_viewPointReached || !_viewRotationReached)
        {
            _currentSlerpTime += Time.deltaTime;
            if (_currentSlerpTime <= _totalSlerpTime)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, _parentPoints[_focusObject].position, _currentSlerpTime / _totalSlerpTime);
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, _parentPoints[_focusObject].rotation, _currentSlerpTime / _totalSlerpTime);
            }
            else
            {
                _viewPointReached = true;
                _viewRotationReached = true;
            }
        }
    }
    private void ReachShakePoint()
    {
        if (_viewPointReached && _viewRotationReached)
        {
            if (_shakePoints.Count > 0)
            {
                Vector3 directionVector = (_parentPoints[_focusObject].position + _shakePoints[0]) - _camera.transform.position;
                if (directionVector.magnitude > _cameraSpeed)
                {
                    _camera.transform.Translate(directionVector.normalized * _cameraSpeed);
                    if (basic.Gameplayvalues.GetApplyJellyFeel()) _camera.transform.LookAt(_lookAtPoints[_focusObject]);
                }
                else
                {
                    _camera.transform.position = (_parentPoints[_focusObject].position + _shakePoints[0]);
                    _shakePoints.RemoveAt(0);
                    if (_shakePoints.Count == 0)
                    {
                        _viewPointReached = false;
                        _viewRotationReached = false;
                    }
                }
            }
        }
    }
    public void CreateShakePoint()
    {
        for (int i = 0; i < basic.Gameplayvalues.GetMaxShakePoints(); i++)
        {
            float slider = basic.Gameplayvalues.GetShakePointDistance();
            Vector3 offset = new Vector3(Random.Range(-slider, slider), Random.Range(-slider, slider), 0);
            _shakePoints.Add(offset);
        }
    }
}