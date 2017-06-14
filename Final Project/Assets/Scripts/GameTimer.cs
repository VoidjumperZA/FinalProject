using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    private float totalTimePerLevel;
    private float timeLeft;
    private bool counting;
    // Use this for initialization
    void Start()
    {
        totalTimePerLevel = 3 * 60;
        timeLeft = totalTimePerLevel;
        counting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (counting == true)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                basic.EndGame();
                //boat movement stop
                //controls disabled
                //fish stop spawning
                //hook reel if it's down
                //radar stop pulsing
            }
        }
    }

    public float GetTimeLeft()
    {
        return timeLeft;

    }

    public string GetFormattedTimeLeftAsString()
    {
        int seconds = Mathf.FloorToInt(GetFormattedTimeLeftAsVec2().y);
        string componentBreaker = ":";
        if (seconds < 10)
        {
            componentBreaker = ":0";
        }
        string str = GetFormattedTimeLeftAsVec2().x + componentBreaker + GetFormattedTimeLeftAsVec2().y;
        //Debug.Log(str);
        return str;
    }

    /// <summary>
    /// Returns a formatted time with x as minutes and y as seconds.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetFormattedTimeLeftAsVec2()
    {
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.Floor(timeLeft % 60);
        Vector2 time = new Vector2(minutes, seconds);
        return time;
    }

    public void BeginCountdown()
    {
        counting = true;
    }

}