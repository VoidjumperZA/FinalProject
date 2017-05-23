using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private bool _initialized = false;
    private GameObject _manager { get { return GameObject.Find("Manager"); } }
    private Camera _camera { get { return Camera.main; } }

    //Camera zoom levels and focus points
    public enum CameraFocus { Boat, Ocean, Hook };
    public CameraFocus _focusObject;
    private Dictionary<CameraFocus, Transform> _parentPoints;
    private Dictionary<CameraFocus, Transform> _lookAtPoints;
    private Vector3 _destination;
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
        //SetViewPoint(CameraFocus.Boat, )

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
            /*Vector3 directionVector = _parentPoints[_focusObject].position - _camera.transform.position;
            if (directionVector.magnitude > _cameraSpeed)
            {
                //_camera.transform.Translate(directionVector.normalized * _cameraSpeed);
                _camera.transform.position = Vector3.Slerp(_camera.transform.position, _parentPoints[_focusObject].position, _currentSlerpTime/_totalSlerpTime);
            }
            else
            {
                _camera.transform.position = _parentPoints[_focusObject].position;
                _viewPointReached = true;
            }
            if (_camera.transform.rotation != _parentPoints[_focusObject].rotation) _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, _parentPoints[_focusObject].rotation, _currentSlerpTime / _totalSlerpTime);
            else _viewRotationReached = true;*/
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
    /// <summary>
    /// Must be called at least once from another script to set the class up.
    /// </summary>
    /*public static void ArtificialStart()
    {
        //Avoid duplicate calls
        if (hasStartBeenCalled == false)
        {
            //ARTIFICIAL UPDATE
            minKeyRange = 1000;
            maxKeyRange = 9999;
            updateKey = 1000;

            //SCREEN SHAKE
            gameplayValues = _manager.GetComponent<GameplayValues>();
            screenShakeLock = false;

            //CAMERA POSITIONS
            orthZoomLevel = new List<float>();
            // Focus Boat Level
            originalOrthSize = _camera.orthographicSize;
            orthZoomLevel.Add(originalOrthSize + gameplayValues.GetCamZoomFocusBoat());

            //Ocean Overview Level
            orthZoomLevel.Add(originalOrthSize + gameplayValues.GetCamZoomOceanOverview());

            //Zoomed Hook Level
            orthZoomLevel.Add(originalOrthSize + gameplayValues.GetCamZoomZoomedHook());
            //int enumSize = System.Enum.GetNames(typeof(CameraFocus)).Length;

            //CAMERA PARENTS
            _cameraParents = new List<Transform>();
            _cameraParents.Add(GameObject.FindGameObjectWithTag("BoatCamHolder").transform);
            _cameraParents.Add(GameObject.FindGameObjectWithTag("OceanCamHolder").transform);
            _cameraParents.Add(GameObject.FindGameObjectWithTag("HookCamHolder").transform);

            for (int i = 0; i < _cameraParents.Count; i++)
                if (_cameraParents[i] == null)
                    Debug.Log("ERROR: Camera parent object [" + i + "], (which is likely related to " + (CameraFocus)i + ") does not exist or is not tagged correctly.");

            zooming = false;

            hasStartBeenCalled = true;
        }
        else
        {
            Debug.Log("WARNING: Attempting to call ArtificialStart() for a second time. Unnecessary call.");
        }
    }*/

    /*public static void ArtificialUpdate(int pKey)
    {
        if (hasStartBeenCalled == true && updateStatus == UpdateStatus.Taken && pKey == updateKey)
        {           
            if (zooming == true)
            {
                //if our camera sized equals our target OR is greater than the target - step and smaller than the target + step
                if (_camera.orthographicSize > orthZoomLevel[(int)cameraFocus] - gameplayValues.GetCamZoomSpeed() && _camera.orthographicSize < orthZoomLevel[(int)cameraFocus] + gameplayValues.GetCamZoomSpeed())
                {
                    //if our step could mathematically never reach our target (e.g. step = 0.75, target = 10)
                    //then correct and set the camera to the final target
                    _camera.orthographicSize = orthZoomLevel[(int)cameraFocus];
                    zooming = false;
                }
                else
                {                   
                    float polarity = 1.0f;
                    if (_camera.orthographicSize > orthZoomLevel[(int)cameraFocus])
                    {
                        polarity = -1.0f;
                    }
                    _camera.orthographicSize += polarity * (gameplayValues.GetCamZoomSpeed());
                }
            }
        }
        else
        {
            Debug.Log("WARNING: Either ArtificialStart() has not been called or you are trying to access the ArtificialUpdate without requestion permission, or with an incorrect key. Please request a key using RequestUpdateCallPermission()");
        }
    }

    /// <summary>
    /// Gives permission as well as a unique key to a script wanting to manage the ArtificialUpdate. This is to stop multiple scripts over calling the ArtificialUpdate multiples times per frame and destroying any update sensitive information.
    /// </summary>
    /// <returns>A unique key.</returns>
    public static int RequestUpdateCallPermission()
    {
        int key = 666;
        if (updateStatus == UpdateStatus.Taken)
        {
            Debug.Log("WARNING: Access to the ArtificialUpdate denied. Update call is currently being used by another script.");
        }
        else
        {
            updateKey = Random.Range(minKeyRange, maxKeyRange);
            key = updateKey;
            updateStatus = UpdateStatus.Taken;
        }
        return key;
    }

    /// <summary>
    /// Relinquishes a script's hold on the ArtificialUpdate, allowing a new script to request a key and use the update.
    /// </summary>
    public static void ReleaseUpdateCallPermission()
    {
        updateStatus = UpdateStatus.Available;
    }

    /// <summary>
    /// Manually parent the GameObject to any GameObject. If you are planning to parent the camera to the Boat, Ocean or Hook Cam Holders, rather use SetCameraFocusPoint() instead.
    /// </summary>
    /// <param name="pTransform"></param>
    public static void SetParent(Transform pTransform, bool pCallingFromInternal = false)
    {
        if (hasStartBeenCalled == true)
        {
            if (pTransform != null)
            {
                //Don't do this check, if we're already calling from the internal SetFocus method
                if (pCallingFromInternal == false)
                    for (int i = 0; i < _cameraParents.Count; i++)
                        if (_cameraParents[i].transform == pTransform)
                            Debug.Log("WARNING: Given transform to parent is equal to one of the Cam Holders (Boat, Ocean or Hook) If you are attempting to focus the camera on that position, rather use SetCameraFocusPoint() instead.");   
                
                _camera.transform.SetParent(pTransform);
            }
            else
            {
                Debug.Log("ERROR: Cannot set parent as given transform is null.");
            }
        }
        else
        {
            displayArtificialStartWarning();
        }
    }
    /// <summary>
    /// Apply screenshake to the camera. Use pShowDebugLog = true to get a console report of the movement applied.
    /// </summary>
    /// <param name="pShakeIntensity"></param>
    /// <param name="pResetting"></param>
    public static void ApplyScreenShake(bool pShowDebugLog = false)
    {
        if (hasStartBeenCalled == true)
        {
            //stop the screen offsetting before it has been reset
            //therefore avoiding stacking shakes and eventually having a 
            //noticable and incorrect permanent offset
            if (screenShakeLock == false)
            {
                //Only create a new direction set on apply
                chooseRandomShakeDirections();
                internalShake(false, pShowDebugLog);
            }
            screenShakeLock = true;
        }
        else
        {
            displayArtificialStartWarning();
        }
    }

    /// <summary>
    /// Inversely apply screenshake. Use pShowDebugLog = true to get a console report of the movement applied.
    /// </summary>
    /// <param name="pShowDebugLog"></param>
    public static void ResetScreenShake(bool pShowDebugLog = false)
    {
        if (hasStartBeenCalled == true)
        {
            internalShake(true, pShowDebugLog);
            screenShakeLock = false;
        }
        else
        {
            displayArtificialStartWarning();
        }
    }

    //Avoid duplicate code
    private static void internalShake(bool pResetting, bool pShowDebugLog)
    {
        if (hasStartBeenCalled == true)
        {
            //whether the camera moves normally, or inversely
            float direction = -1.0f;
            if (pResetting == true)
            {
                direction = 1.0f;
            }

            //Create the vector separately so it allows for easy debugging
            Vector3 screenShakeMovement = new Vector3(_camera.transform.position.x - (direction * (gameplayValues.GetScreenShakeIntensity() * (float)shakePolarities.x)), _camera.transform.position.y - (direction * (gameplayValues.GetScreenShakeIntensity() * (float)shakePolarities.y)), _camera.transform.position.z);

            //Apply
            _camera.transform.position = screenShakeMovement;

            //Print, if requested by the user
            if (pShowDebugLog == true)
            {
                Debug.Log("Moving screen: [" + (direction * (gameplayValues.GetScreenShakeIntensity() * (float)shakePolarities.x)) + ", " + (direction * (gameplayValues.GetScreenShakeIntensity() * (float)shakePolarities.y)) + "," + screenShakeMovement.z + "] units.");
            }
        }
        else
        {
            displayArtificialStartWarning();
        }
    }

    private static void chooseRandomShakeDirections()
    {
        if (hasStartBeenCalled == true)
        {
            //Choose either a positive or negative direction for both the X and Y components
            int yPolarity = Random.Range(0, 2);
            if (yPolarity == 0)
            {
                //correct for the limitations of random.range by bumping back by one
                yPolarity = -1;
            }

            int xPolarity = Random.Range(0, 2);
            if (xPolarity == 0)
            {
                //correct for the limitations of random.range by bumping back by one
                xPolarity = -1;
            }

            //Save those to a vector
            shakePolarities.x = xPolarity;
            shakePolarities.y = yPolarity;
        }
        else
        {
            displayArtificialStartWarning();
        }
    }

    public static void SetCameraFocusPoint(CameraFocus pCameraPoint, bool pAttachToRelatedParent)
    {
        if (hasStartBeenCalled == true)
        {
            cameraFocus = pCameraPoint;
            //_camera.orthographicSize = orthZoomLevel[(int)cameraFocus];
            zooming = true;

            if (pAttachToRelatedParent == true)
            {
                //Explicit call to avoid accidentally overriding transform.setparent
                CameraHandler.SetParent(_cameraParents[(int)cameraFocus].transform, true);
                Vector3 zeroed = new Vector3(0.0f, 0.0f, Camera.main.transform.position.z);
                //Camera.main.transform.position = zeroed;
            }
        }
        else
        {
            displayArtificialStartWarning();
        }        
    }

    private static void displayArtificialStartWarning()
    {
        Debug.Log("ERROR: Camera operations cannot be commenced until the CameraHandler.ArtificialStart() method is called.");
    }*/
}