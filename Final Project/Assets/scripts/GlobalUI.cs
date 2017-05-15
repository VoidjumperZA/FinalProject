using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour {
    [SerializeField] private Button _playGameButton;


    [SerializeField] private Button _deployHookButton;
    [SerializeField] private Button _reelUpHook;
    [SerializeField] private Button _radarButton;

    void Start ()
    {
        //Warnings
        if (!_deployHookButton) Debug.Log("Warning: You need to assign DeployHookButton to GlobalUI.");
        if (!_reelUpHook) Debug.Log("Warning: You need to assign ReelUpButton to GlobalUI.");
        if (!_radarButton) Debug.Log("Warning: You need to assign RadarButton to GlobalUI.");

        DeployHookButton(false);
        RadarButton(false);
        ReelUpHookButton(false);
    }
    public void OnPlayGameClick()
    {
        CameraHandler.SetCameraFocusPoint(CameraHandler.CameraFocus.OceanOverview, true);
        basic.Boat.SetState(boat.BoatState.SetUp);
        basic.Shoppinglist.Show(true);
        _playGameButton.gameObject.SetActive(false);
    }
	public void DeployHookButton(bool pBool) {  _deployHookButton.gameObject.SetActive(pBool); }
    public void ReelUpHookButton(bool pBool) { _reelUpHook.gameObject.SetActive(pBool); }
    public void RadarButton(bool pBool) { _radarButton.gameObject.SetActive(pBool); }
	
    public void DeployHook()
    {
        basic.Boat.SetState(boat.BoatState.Fish);
        basic.Scorehandler.ToggleHookScoreUI(true);
        GameObject.Find("Manager").GetComponent<Combo>().CreateNewCombo();
        DeployHookButton(false);
        //RadarButton(false);
        ReelUpHookButton(true);
   

    }

    public void ReelUpHook()
    {
        basic.Hook.SetState(hook.HookState.Reel);
        GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
    }

    public void SwitchHookButtons()
    {
        DeployHookButton(true);
        //RadarButton(true);
        ReelUpHookButton(false); 
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
