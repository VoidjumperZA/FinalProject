using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic : MonoBehaviour
{
    private InputTimer _inputTimer;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _boatSpawn;
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private GameObject _radarPrefab;
    [SerializeField] private GameObject _hookPrefab;

    private int cameraHandlerUpdateKey;

    private general _selected = null;
    private static List<general> _generals = new List<general>(); public static List<general> Generals { get { return _generals; } }
    public static boat Boat { get { return (boat)_generals[0]; }  set {  _generals[0] = (boat)value; } }
    public static hook Hook { get { return (hook)_generals[1]; } set {  _generals[1] = (hook)value; } }
    public static Radar Radar { get { return (Radar)_generals[2]; } set { _generals[1] = (Radar)value; } }

    void Start()
    {
        _generals.Add(SpawnBoat()); // _generals[0]
        _generals.Add(SpawnHook()); // _generals[1]

        Boat.AssignHook(Hook);
        Boat.AssignRadar(SpawnRadar()); // _generals[3]
        Hook.AssignBoat(Boat);

        Debug.Log(_generals.Count + " generals");
        //InputTimer and basic should be on the same object, but I'm explictly calling in case they ever aren't
        //and therefore I can still get the script
        _inputTimer = GameObject.Find("Manager").GetComponent<InputTimer>(); if (!_inputTimer) Debug.Log("ERROR: Cannot get a reference to InputTimer from the Manager object.");
        CameraHandler.ArtificialStart();
        cameraHandlerUpdateKey = CameraHandler.RequestUpdateCallPermission();
    }
    void Update()
    {
        SelectNewGeneral();
        DeselectPreviousGeneral();
        RenderTrail(Boat.transform.position, Hook.gameObject.transform.position);        
    }

    private void LateUpdate()
    {
        CameraHandler.ArtificialUpdate(cameraHandlerUpdateKey);
    }
    private void SelectNewGeneral()
    {
        // On Left mouse button click
        if (!Input.GetMouseButtonDown(0)) return;
        _inputTimer.ResetClock();
        // Deselect() previously selected object
        if (_selected)
        {
            _selected.Deselect();
            _selected = null;
        }
        // Raycast to get new object with general component
        _selected = mouse.GetGeneral();
        // Select() new object
        if (_selected)
        {
            _selected.Select();
        }
    }
    private void DeselectPreviousGeneral()
    {
        if (Input.GetMouseButtonUp(0) && _selected)
        {
            _selected.Deselect();
            _inputTimer.ResetClock();
        }
    }
    private boat SpawnBoat()
    {
        return Instantiate(_boatPrefab, _boatSpawn.position, Quaternion.identity).GetComponent<boat>();
    }
    private Radar SpawnRadar()
    {
        return Instantiate(_radarPrefab, _boatSpawn.position + new Vector3(0,0,0.25f), Quaternion.identity).GetComponent<Radar>();
    }
    private hook SpawnHook()
    {
        return Instantiate(_hookPrefab, _generals[0].transform.position, Quaternion.identity).GetComponent<hook>();
    }
    private void RenderTrail(Vector3 pPositionOne, Vector3 pPositionTwo)
    {
        _lineRenderer.SetPositions(new Vector3[] { pPositionOne, pPositionTwo });
    }
    public void AddFish(fish pFish)
    {
        _generals.Add(pFish);
    }
}
