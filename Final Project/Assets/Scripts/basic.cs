using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic : MonoBehaviour
{
    private InputTimer _inputTimer;
    [HideInInspector] public static GlobalUI GlobalUi;
    [HideInInspector] public static ScoreHandler Scorehandler;
    [HideInInspector] public static Combo combo;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _boatSpawn;
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private GameObject _radarPrefab;
    [SerializeField] private GameObject _hookPrefab;

    private int cameraHandlerUpdateKey;
    private static List<general> _generals = new List<general>(); public static List<general> Generals { get { return _generals; } }
    public static boat Boat { get { return (boat)_generals[0]; }  set {  _generals[0] = (boat)value; } }
    public static hook Hook { get { return (hook)_generals[1]; } set {  _generals[1] = (hook)value; } }
    public static Radar Radar { get { return (Radar)_generals[2]; } set { _generals[2] = (Radar)value; } }

    void Start()
    {
        _generals.Add(SpawnBoat()); // _generals[0]
        _generals.Add(SpawnHook()); // _generals[1]
        _generals.Add(SpawnRadar());

        Boat.AssignHook(Hook);
        Boat.AssignRadar(Radar); // _generals[3]
        Hook.AssignBoat(Boat);
        //Debug.Log(_generals.Count + " generals");

        //InputTimer and basic should be on the same object, but I'm explictly calling in case they ever aren't
        //and therefore I can still get the script
        _inputTimer = GetComponent<InputTimer>(); if (!_inputTimer) Debug.Log("ERROR: Cannot get a reference to InputTimer from the Manager object.");
        GlobalUi = GetComponent<GlobalUI>(); if (!GlobalUi) Debug.Log("ERROR: Cannot get a reference to GlobalUI from the Manager object.");
        Scorehandler = GetComponent<ScoreHandler>(); if (!Scorehandler) Debug.Log("ERROR: Cannot get reference to ScoreHandler from Manager object");
        combo = GetComponent<Combo>(); if (!combo) Debug.Log("ERROR: Cannot get reference to Combo from Manager object");

        CameraHandler.ArtificialStart();
        cameraHandlerUpdateKey = CameraHandler.RequestUpdateCallPermission();
    }
    void Update()
    {
        RenderTrail(Boat.transform.position, Hook.gameObject.transform.position);
        if (Input.GetMouseButton(0)) _inputTimer.ResetClock();    
    }

    private void LateUpdate()
    {
        CameraHandler.ArtificialUpdate(cameraHandlerUpdateKey);
    }
    private boat SpawnBoat()
    {
        return Instantiate(_boatPrefab, _boatSpawn.position, Quaternion.identity).GetComponent<boat>();
    }
    private Radar SpawnRadar()
    {
        return Instantiate(_radarPrefab, _boatSpawn.position + new Vector3(0,-5f,0.25f), Quaternion.identity).GetComponent<Radar>();
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
