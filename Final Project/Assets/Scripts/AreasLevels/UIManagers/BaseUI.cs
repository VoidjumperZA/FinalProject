using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour {
    public Canvas canvas;
    public GameObject ScoreUI; //the appearing score ui
    public GameObject ScoreUIPosition; //where are we spawning that ui

    public virtual void Start () {
        //Debug.Log("BaseUI - Start();");
    }
    public virtual void Update()
    {

    }
    public virtual void OnEnterScene()
    {

    }
    public virtual void OnLeaveScene()
    {

    }
    public void SetActive(bool pBool, params GameObject[] pGameObjects)
    {
        foreach (GameObject gO in pGameObjects) gO.SetActive(pBool);
    }
    public void SetActiveButtons(bool pBool, params Button[] pButtons)
    {
        foreach (Button button in pButtons) button.gameObject.SetActive(pBool);
    }
    public virtual void HookScoreToggle(bool pBool)
    {

    }
}
