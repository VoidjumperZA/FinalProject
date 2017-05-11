using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayValues : MonoBehaviour
{
    //Screen Shake
    [Header("Screen shake")]
    [SerializeField]
    [Range(0.0f, 3.0f)]
    private float screenShakeIntensity;
    [SerializeField]
    private int screenShakeDuration;

    [Header("Camera Zoom")]
    [SerializeField]
    private float camDefaultFocusBoatModifier;

    [SerializeField]
    private float camOceanOverviewZoomLevel;

    [SerializeField]
    private float camZoomedHookZoomLevel;

    [SerializeField]
    private float zoomSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetScreenShakeIntensity()
    {
        return screenShakeIntensity;
    }

    public int GetScreenShakeDuration()
    {
        return screenShakeDuration;
    }

    public float GetCamZoomFocusBoat()
    {
        return camDefaultFocusBoatModifier;
    }

    public float GetCamZoomOceanOverview()
    {
        return camOceanOverviewZoomLevel;
    }

    public float GetCamZoomZoomedHook()
    {
        return camZoomedHookZoomLevel;
    }

    public float GetCamZoomSpeed()
    {
        return zoomSpeed;
    }

}
