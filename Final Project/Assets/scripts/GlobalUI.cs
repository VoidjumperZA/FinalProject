﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour {

    [SerializeField] private Button _deployHookButton;
    [SerializeField] private Button _reelUpHook;
    [SerializeField] private Button _radarButton;

    void Start ()
    {
        //Warnings
        if (!_deployHookButton) Debug.Log("Warning: You need to assign DeployHookButton to GlobalUI.");
        if (!_reelUpHook) Debug.Log("Warning: You need to assign ReelUpButton to GlobalUI.");
        if (!_radarButton) Debug.Log("Warning: You need to assign RadarButton to GlobalUI.");

        _reelUpHook.gameObject.SetActive(false);
    }
	
	
    public void DeployHook()
    {
        basic.Boat.SetState(boat.BoatState.Fish);
        CameraHandler.SetCameraFocusPoint(CameraHandler.CameraFocus.ZoomedHook, true);
        GameObject.Find("Manager").GetComponent<Combo>().CreateNewCombo();
        _deployHookButton.gameObject.SetActive(false);
        _radarButton.gameObject.SetActive(false);
        _reelUpHook.gameObject.SetActive(true);
   

    }

    /*public void SendRadarPulse()
    {
        Radar r = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();
        r.SendPulse();
    }*/

    public void ReelUpHook()
    {
        basic.Hook.SetState(hook.HookState.Reel);
        GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
    }

    public void SwitchHookButtons()
    {
        _reelUpHook.gameObject.SetActive(false);
        _deployHookButton.gameObject.SetActive(true);
        _radarButton.gameObject.SetActive(true);    
    }

    private void DisableButton(Button buttonToDisable)
    {
        buttonToDisable.interactable = false;
    }

    private void EnableButton(Button buttonToEnable)
    {
        buttonToEnable.interactable = true;
    }


}
