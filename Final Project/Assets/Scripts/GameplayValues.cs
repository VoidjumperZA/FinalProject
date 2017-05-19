using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayValues : MonoBehaviour
{
    //Screen Shake
    [Header("Screen shake")]
    [SerializeField] [Range(0.0f, 10.0f)] private float _shakePointDistance;
    [SerializeField] [Range(1, 10)] private int _maxShakePoints;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private bool _applyJellyFeel;
    /*[SerializeField]
    private int screenShakeDuration;

    [Header("Camera Zoom")]
    [SerializeField]
    private float camDefaultFocusBoatModifier;

    [SerializeField]
    private float camOceanOverviewZoomLevel;

    [SerializeField]
    private float camZoomedHookZoomLevel;*/

    /*[Header("Ability Statistics")]
    [SerializeField]
    private float sonarFadeTime;*/

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetShakePointDistance()
    {
        return _shakePointDistance;
    }

    public float GetCameraSpeed()
    {
        return _cameraSpeed;
    }

    public bool GetApplyJellyFeel()
    {
        return _applyJellyFeel;
    }
    public int GetMaxShakePoints()
    {
        return _maxShakePoints;
    }
    /*public int GetScreenShakeDuration()
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
    }*/

}
