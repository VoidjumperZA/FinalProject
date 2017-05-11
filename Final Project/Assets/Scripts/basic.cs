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
    public static boat Boat;
    public static hook Hook;
    public static Radar Radar;

    private GameObject boat;
    private GameObject floor;
    private float seaDepth;

    void Start()
    {
        Boat = SpawnBoat();
        Hook = SpawnHook();
        Radar = SpawnRadar();

        Boat.AssignHook(Hook);
        Boat.AssignRadar(Radar);
        Hook.AssignBoat(Boat);
        //Debug.Log(_generals.Count + " generals");

        //InputTimer and basic should be on the same object, but I'm explictly calling in case they ever aren't
        //and therefore I can still get the script
        _inputTimer = GetComponent<InputTimer>(); if (!_inputTimer) Debug.Log("ERROR: Cannot get a reference to InputTimer from the Manager object.");
        GlobalUi = GetComponent<GlobalUI>(); if (!GlobalUi) Debug.Log("ERROR: Cannot get a reference to GlobalUI from the Manager object.");
        Scorehandler = GetComponent<ScoreHandler>(); if (!Scorehandler) Debug.Log("ERROR: Cannot get reference to ScoreHandler from Manager object");
        combo = GetComponent<Combo>(); if (!combo) Debug.Log("ERROR: Cannot get reference to Combo from Manager object");

        floor = GameObject.FindGameObjectWithTag("Floor");
        boat = GameObject.FindGameObjectWithTag("Boat");
        Vector3 difference = boat.transform.position - floor.transform.position;
        seaDepth = difference.y;
        //
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
        boat theBoat = Instantiate(_boatPrefab, _boatSpawn.position, Quaternion.identity).GetComponent<boat>();
        _generals.Add(theBoat);
        return theBoat;
    }
    private hook SpawnHook()
    {
        hook theHook = Instantiate(_hookPrefab, _generals[0].transform.position, Quaternion.identity).GetComponent<hook>();
        _generals.Add(theHook);
        return theHook;
    }
    private Radar SpawnRadar()
    {
        Radar theRadar = Instantiate(_radarPrefab, _boatSpawn.position + new Vector3(0,-3f,0.25f), Quaternion.identity).GetComponent<Radar>();
        _generals.Add(theRadar);
        return theRadar;
    }
    private void RenderTrail(Vector3 pPositionOne, Vector3 pPositionTwo)
    {
        _lineRenderer.SetPositions(new Vector3[] { pPositionOne, pPositionTwo });
    }
    public void AddFish(fish pFish)
    {
        _generals.Add(pFish);
    }
    public void AddTrash(trash pTrash)
    {
        _generals.Add(pTrash);
    }

    public float GetSeaDepth()
    {
        return seaDepth;
    }
}
