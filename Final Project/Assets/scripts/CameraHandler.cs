using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraHandler
{
    //public static CameraHandler Instance;
    private static Camera _camera { get { return Camera.main; } }
    private static Vector2 shakePolarities;
    private static GameplayValues gameplayValues;
    private static bool screenShakeLock; 
    public static void SetParent(Transform pTransform)
    {
        _camera.transform.SetParent(pTransform);
        gameplayValues = GameObject.Find("Manager").GetComponent<GameplayValues>();
        screenShakeLock = false;
    }
    /// <summary>
    /// Apply screenshake to the camera. Use pShowDebugLog = true to get a console report of the movement applied.
    /// </summary>
    /// <param name="pShakeIntensity"></param>
    /// <param name="pResetting"></param>
    public static void ApplyScreenShake(bool pShowDebugLog = false)
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

    /// <summary>
    /// Inversely apply screenshake. Use pShowDebugLog = true to get a console report of the movement applied.
    /// </summary>
    /// <param name="pShowDebugLog"></param>
    public static void ResetScreenShake(bool pShowDebugLog = false)
    {
        internalShake(true, pShowDebugLog);
        screenShakeLock = false;
    }

    //Avoid duplicate code
    private static void internalShake(bool pResetting, bool pShowDebugLog)
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
            Debug.Log("Moving screen: [" + screenShakeMovement.x + ", " + screenShakeMovement.y + "," + screenShakeMovement.z + "] units.");
        }
    }

    private static void chooseRandomShakeDirections()
    {
        //Choose either a positive or negative direction for both the X and Y components
        int yPolarity = Random.Range(0, 2);
        Debug.Log("yPol: " + yPolarity);
        if (yPolarity == 0)
        {
            yPolarity = -1;
        }
        Debug.Log("yPol has been corrected to: " + yPolarity);
        int xPolarity = Random.Range(0, 2);
        Debug.Log("xPol: " + xPolarity);
        if (xPolarity == 0)
        {
            xPolarity = -1;
        }
        Debug.Log("xPol has been corrected to: " + xPolarity);
        //Save those to a vector
        shakePolarities.x = xPolarity;
        shakePolarities.y = yPolarity;
    }

}
