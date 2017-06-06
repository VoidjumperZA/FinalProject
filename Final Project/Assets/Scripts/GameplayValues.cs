using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayValues : MonoBehaviour
{
    //Screen Shake
    [Header("Screen shake")]
    [SerializeField] [Range(0.0f, 10.0f)] private float _shakePointDistance;
    [SerializeField] [Range(1, 10)] private int _maxShakePoints;
    [SerializeField] private float _shakeSpeed;
    [SerializeField] private bool _applyJellyFeel;
    [SerializeField] private float _menuToOceanDuration;
    [SerializeField] private float _oceanToHookDuration;
    [SerializeField] private float _hookToOceanDuration;


    [Header("Boat Values")]
    [SerializeField]
    private int boatRotationSpeed;
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

    public float GetShakeSpeed()
    {
        return _shakeSpeed;
    }

    public int GetBoatRotationSpeed()
    {
        return boatRotationSpeed;
    }

    public bool GetApplyJellyFeel()
    {
        return _applyJellyFeel;
    }
    public int GetMaxShakePoints()
    {
        return _maxShakePoints;
    }
    public float MenuToOcean()
    {
        return _menuToOceanDuration;
    }
    public float OceanToHook()
    {
        return _oceanToHookDuration;
    }
    public float HookToOcean()
    {
        return _hookToOceanDuration;
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
