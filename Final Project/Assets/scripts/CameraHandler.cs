using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraHandler
{
    //public static CameraHandler Instance;
    private static Camera _camera { get { return Camera.main; } }

    //Screen shake
    private static Vector2 shakePolarities;
    private static GameplayValues gameplayValues;
    private static bool screenShakeLock;

    //Vector3 cameraPosZoomedHook;
    private static Vector3 cameraPosFocusBoat;
    private static Vector3 cameraPosOceanOverview;
    private static Vector3 cameraPosZoomedHook;
    public enum CameraFocus { FocusBoat, OceanOverview, ZoomedHook };
    public static CameraFocus cameraFocus;
    private static List<Vector3> cameraPositions;
    private static List<GameObject> cameraParents;
    private static bool hasStartBeenCalled = false;

    /// <summary>
    /// Must be called at least once from another script to set the class up.
    /// </summary>
    public static void ArtificialStart()
    {
        //Avoid duplicate calls
        if (hasStartBeenCalled == false)
        {
            //SCREEN SHAKE
            gameplayValues = GameObject.Find("Manager").GetComponent<GameplayValues>();
            screenShakeLock = false;

            //CAMERA POSITIONS
            cameraPositions = new List<Vector3>();
            // Focus Boat Level
            cameraPosFocusBoat = _camera.transform.position;
            cameraPosFocusBoat.z += gameplayValues.GetCamZoomFocusBoat();
            cameraPositions.Add(cameraPosFocusBoat);

            //Ocean Overview Level
            cameraPosOceanOverview = _camera.transform.position;
            cameraPosOceanOverview.z += gameplayValues.GetCamZoomOceanOverview();
            cameraPositions.Add(cameraPosOceanOverview);

            //Zoomed Hook Level
            cameraPosZoomedHook = _camera.transform.position;
            cameraPosZoomedHook.z += gameplayValues.GetCamZoomZoomedHook();
            cameraPositions.Add(cameraPosZoomedHook);
            //int enumSize = System.Enum.GetNames(typeof(CameraFocus)).Length;

            //CAMERA PARENTS
            cameraParents = new List<GameObject>();
            cameraParents.Add(GameObject.FindGameObjectWithTag("BoatCamHolder"));
            cameraParents.Add(GameObject.FindGameObjectWithTag("OceanCamHolder"));
            cameraParents.Add(GameObject.FindGameObjectWithTag("HookCamHolder"));

            for (int i = 0; i < cameraParents.Count; i++)
            {
                if (cameraParents[i] == null)
                {
                    Debug.Log("ERROR: Camera parent object [" + i + "], (which is likely related to " + (CameraFocus)i + ") does not exist or is not tagged correctly.");
                }
            }

            hasStartBeenCalled = true;
        }
        else
        {
            Debug.Log("WARNING: Attempting to call ArtificialStart() for a second time. Unnecessary call.");
        }
    }

    /// <summary>
    /// Manually parent the GameObject to any GameObject. If you are planning to parent the camera to the Boat, Ocean or Hook Cam Holders, rather use SetCameraFocusPoint() instead.
    /// </summary>
    /// <param name="pTransform"></param>
    public static void SetParent(Transform pTransform)
    {
        if (hasStartBeenCalled == true)
        {
            if (pTransform != null)
            {
                for (int i = 0; i < cameraParents.Count; i++)
                {
                    if (cameraParents[i].transform == pTransform)
                    {
                        Debug.Log("WARNING: Given transform to parent is equal to one of the Cam Holders (Boat, Ocean or Hook) If you are attempting to focus the camera on that position, rather use SetCameraFocusPoint() instead.");
                    }
                }
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
            _camera.transform.position = cameraPositions[(int)cameraFocus];

            if (pAttachToRelatedParent == true)
            {
                //Explicit call to avoid accidentally overriding transform.setparent
                CameraHandler.SetParent(cameraParents[(int)cameraFocus].transform);
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
    }
}
