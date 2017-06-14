using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private GameObject _seaSurface;
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
    [Header("References")]

    public BaseUI _baseUI;
    [SerializeField]
    protected FishSpawn _fishSpawner;

    [SerializeField]
    protected ShoppingList _shoppingList;
    [SerializeField]
    protected JellyFishSpawn _jellyFishSpawn;
    public virtual void Start()
    {
        SetUpCamera();
        SetUpBoat();

        GameManager.Fishspawner = _fishSpawner;
        _shoppingList.GenerateShoppingList();
        GameManager.ShopList = _shoppingList;
        GameManager.JellyFishSpawner = _jellyFishSpawn;


        GameManager.Levelmanager = this;
        //Debug.Log("LevelManager - Start();");
    }
	public virtual void Update () {

    }
    protected void SetUpCamera()
    {
        GameManager.Camerahandler.SeaSurface = _seaSurface.transform;
        GameManager.Camerahandler.ToggleBelowWater(false);
        GameManager.Camerahandler.StartMiddleEndCameraHolder(_startCamHolder, _middleCamHolder, _endCamHolder);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Start, true);
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Middle, false);
    }
    protected void SetUpBoat()
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
    public virtual void UIOnEnterScene()
    {
        if (_baseUI) _baseUI.OnEnterScene();
        else Debug.Log("LevelUI not assigned to LevelManager script");
    }
    public virtual void UIOnLeaveScene()
    {
        if (_baseUI) _baseUI.OnLeaveScene();
    }
    public Canvas Canvas()
    {
        if (_baseUI.canvas) return _baseUI.canvas;
        return null;
    }
}
