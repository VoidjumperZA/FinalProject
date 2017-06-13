using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Header("BoatMovementAreaBoundaries")]
    [SerializeField] private Transform _leftDetector;
    [SerializeField] private Transform _rightDetector;
    [Header("SceneTransitionBoatPoints")]
    [SerializeField] protected Transform _enterBoatPoint;
    [SerializeField] protected Transform _leaveBoatPoint;
    [Header("SceneTransitionCameraHolders")]
    [SerializeField] protected Transform _startCamHolder;
    [SerializeField] protected Transform _middleCamHolder;
    [SerializeField] protected Transform _endCamHolder;

    [SerializeField] private BaseUI _levelUI;
    [SerializeField] private ShoppingList _shoppingList;
    [SerializeField] private FishSpawn _fishSpawner;
    public virtual void Start () {
        SetUpCamera();
        SetUpBoat();

        GameManager.Fishspawner = _fishSpawner;
        _shoppingList.GenerateShoppingList();


        GameManager.Levelmanager = this;
        //Debug.Log("LevelManager - Start();");
    }
	public virtual void Update () {

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
        if (_levelUI) _levelUI.OnEnterScene();
        else Debug.Log("LevelUI not assigned to LevelManager script");
    }
    public void UIOnLeaveScene()
    {
        if (_levelUI) _levelUI.OnLeaveScene();
    }
}
