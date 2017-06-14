using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI {
    [SerializeField] private Button _playTutorial;
    [SerializeField] private Button _skipTutorial;
    public override void Start ()
    {
        SetActiveButtons(true, _playTutorial, _skipTutorial);
        //Debug.Log("MenuUI - Start();");
    }
    public override void Update()
    {

    }
    public void OnPlayTutorialClick()
    {
        //Debug.Log("PlayClicked!");
        GameManager.LoadSceneAsync(1, 4);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.End);
        GameManager.Camerahandler.Play();
        GameManager.Boat.SetState(boat.BoatState.LeaveScene);
        SetActiveButtons(false, _playTutorial, _skipTutorial);
    }
    public void OnSkipTutorialClick()
    {
        //Debug.Log("SkipClicked!");
        GameManager.LoadSceneAsync(2, 4);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.End);
        GameManager.Camerahandler.Play();
        //GameManager.Camerahandler.Play();
        GameManager.Boat.SetState(boat.BoatState.LeaveScene);
        SetActiveButtons(false, _playTutorial, _skipTutorial);
    }
}
