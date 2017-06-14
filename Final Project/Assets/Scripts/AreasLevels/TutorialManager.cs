using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : LevelManager
{
    private Transform _leftDetector;
    private Transform _rightDetector;
    [Header("TutorialUI")]
    [SerializeField]
    private TutorialUI _tutorialUI;

    public virtual void Start()
    {
        _leftDetector = LevelManager.GetLeftDetector();
        _rightDetector = LevelManager.GetRightDetector();
        //_tutorialUI = LevelManager.GetLevelUI();

        SetUpCamera();
        SetUpBoat();

        //Is this the right way to do it? I added a getter in LevelManager and made the function and variable static. 
        GameManager.Fishspawner = LevelManager.GetFishSpawn();

        
        GameManager.Tutorialmanager = this;
        //Debug.Log("LevelManager - Start();");
    }
    public virtual void Update()
    {

    }
    protected void SetUpCamera()
    {
        GameManager.Camerahandler.StartMiddleEndCameraHolder(_startCamHolder, _middleCamHolder, _endCamHolder);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Start, true);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Middle, false);
    }
    private void SetUpBoat()
    {
        SetEnterLeaveBoatStateDestinations();
        // Set Move state boundaries
        if (_leftDetector && _rightDetector) GameManager.Boat.SetMoveBoatStateBoundaries(new Vector3[] { _leftDetector.position, _rightDetector.position });
    }
    protected void SetEnterLeaveBoatStateDestinations()
    {
        // Set Enter and Leave state destination position
        GameManager.Boat.SetEnterStateDestination(_enterBoatPoint.position);
        GameManager.Boat.SetLeaveStateDestination(_leaveBoatPoint.position);
    }
    public void UIOnEnterScene()
    {
        if (_tutorialUI) _tutorialUI.OnEnterScene();
        else Debug.Log("LevelUI not assigned to LevelManager script");
    }
    public void UIOnLeaveScene()
    {
        if (_tutorialUI) _tutorialUI.OnLeaveScene();
    }
}
