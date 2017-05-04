using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayValues : MonoBehaviour
{
    //Screen Shake
    [SerializeField]
    [Range(0.0f, 3.0f)]
    private float screenShakeIntensity;
    [SerializeField]
    private int screenShakeDuration;


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

}
