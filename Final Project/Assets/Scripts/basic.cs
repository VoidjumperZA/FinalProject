using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basic : MonoBehaviour
{
    private InputTimer _inputTimer;
    [HideInInspector] public static GlobalUI GlobalUI;
    [HideInInspector] public static ScoreHandler Scorehandler;
    [HideInInspector] public static ShoppingList Shoppinglist;
    [HideInInspector] public static Combo combo;
    [HideInInspector] public static GameplayValues Gameplayvalues;
    [HideInInspector] public static TempFishSpawn Tempfishspawn;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _boatSpawn;
    [SerializeField] private Transform _boatSetUp;
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private GameObject _trailerPrefab;
    [SerializeField] private GameObject _radarPrefab;
    [SerializeField] private GameObject _hookPrefab;

    private int cameraHandlerUpdateKey;
    public static List<general> Generals = new List<general>();
    public static boat Boat;
    public static hook Hook;
    public static radar Radar;
    public static trailer Trailer;
    
    private GameObject floor;
    private static float seaDepth;

    //Zone for the jellyfish

    [SerializeField] private GameObject _jellyfishZone;
    private static float _jellyfishZonePosX;
    private static float _jellyfishZonePosY;
    private static float _jellyfishZoneSizeX;
    private static float _jellyfishZoneSizeY;


    void Start()
    {
        SpawnBoat(_boatSpawn.position, _boatSetUp.position);
        SpawnHook();
        SpawnRadar();
        SpawnTrailer();

        Boat.AssignHook(Hook);
        Boat.AssignRadar(Radar);
        Hook.AssignBoat(Boat);


        //Debug.Log(_generals.Count + " generals");

        //InputTimer and basic should be on the same object, but I'm explictly calling in case they ever aren't
        //and therefore I can still get the script
        _inputTimer = GetComponent<InputTimer>(); if (!_inputTimer) Debug.Log("ERROR: Cannot get a reference to InputTimer from the Manager object.");
        GlobalUI = GetComponent<GlobalUI>(); if (!GlobalUI) Debug.Log("ERROR: Cannot get a reference to GlobalUI from the Manager object.");
        Scorehandler = GetComponent<ScoreHandler>(); if (!Scorehandler) Debug.Log("ERROR: Cannot get reference to ScoreHandler from Manager object");
        Shoppinglist = GetComponent<ShoppingList>(); if (!Shoppinglist) Debug.Log("ERROR: Cannot get reference to ShoppingList from Manager object");
        combo = GetComponent<Combo>(); if (!combo) Debug.Log("ERROR: Cannot get reference to Combo from Manager object");
        Gameplayvalues = GetComponent<GameplayValues>(); if (!Gameplayvalues) Debug.Log("ERROR: Cannot get reference to GameplayValues from Manager object");
        Tempfishspawn = GetComponent<TempFishSpawn>(); if (!Tempfishspawn) Debug.Log("ERROR: Cannot get reference to TempFishSpawn from Manager object");

        CameraHandler.InitializeCameraHandler();
        CameraHandler.SetViewPoint(CameraHandler.CameraFocus.Boat);

        //Find out seaDepth
        floor = GameObject.FindGameObjectWithTag("Floor");
        Vector3 difference = floor.transform.position - Boat.transform.position;

        seaDepth = Mathf.Abs(difference.y) - floor.transform.lossyScale.y/2;
 

        //Getting the position and size of the zone where the jellyfish can move
        _jellyfishZoneSizeX = _jellyfishZone.transform.localScale.x / 2;
        _jellyfishZonePosX = _jellyfishZone.transform.position.x;
        _jellyfishZoneSizeY = _jellyfishZone.transform.localScale.y / 2;
        _jellyfishZonePosY = _jellyfishZone.transform.position.y;


        //Find out seaWidth
        //_docks = GameObject.FindGameObjectWithTag("Docks"); if (!_docks) Debug.Log("WARNING (Jellyfish uses this): You need to create the Docks and tag it with Docks");
        //_endOfLevel = GameObject.FindGameObjectWithTag("EndOfLevel"); if (!_endOfLevel) Debug.Log("WARNING (Jellyfish uses this): You need to create an empy object, place it at the end of the level (x) and tag it with EndOfLevel");
        //_seaWidth = Vector3.Distance(_docks.transform.position, _endOfLevel.transform.position);
        //
    }
    void Update()
    {
        RenderTrail();
        if (Input.GetMouseButton(0)) _inputTimer.ResetClock();
    }

    private void LateUpdate()
    {
        CameraHandler.Update();
    }
    private void SpawnBoat(Vector3 pSpawnPosition, Vector3 pSetUpPosition)
    {
        Boat = Instantiate(_boatPrefab, pSpawnPosition, Quaternion.identity).GetComponent<boat>();
        Boat.SetSetUpPosition(pSetUpPosition);
    }
    private void SpawnHook()
    {
        Hook = Instantiate(_hookPrefab, Boat.transform.position, Quaternion.identity).GetComponent<hook>();
    }
    private void SpawnRadar()
    {
        Radar = Instantiate(_radarPrefab, _boatSpawn.position + new Vector3(0,-5f,0.25f), Quaternion.identity).GetComponent<radar>();
    }
    private void SpawnTrailer()
    {
        Trailer = Instantiate(_trailerPrefab, Boat.transform.position + new Vector3(-10,0,0), _trailerPrefab.transform.rotation).GetComponent<trailer>();
        Trailer.gameObject.transform.SetParent(Boat.gameObject.transform);
    }
    private void RenderTrail()
    {
        if (!Boat && !Hook) return;
        _lineRenderer.SetPositions(new Vector3[] { Boat.gameObject.transform.position, Hook.gameObject.transform.position });
    }
    public static void AddCollectable(general pCollectable)
    {
        Generals.Add(pCollectable);
    }
    public static void RemoveCollectable(general pCollectable)
    {
        Generals.Remove(pCollectable);
    }
    public static float GetSeaDepth()
    {
        return seaDepth;
    }

    public static float GetJellyfishZonePosX()
    {
        return _jellyfishZonePosX;
    }

    public static float GetJellyfishZonePosY()
    {
        return _jellyfishZonePosY;
    }

    public static float GetJellyfishZoneSizeX()
    {
        return _jellyfishZoneSizeX;
    }

    public static float GetJellyfishZoneSizeY()
    {
        return _jellyfishZoneSizeY;
    }

    public static float GetJellyfishZoneLeft()
    {
        return _jellyfishZonePosX - _jellyfishZoneSizeX;
    }
    public static float GetJellyfishZoneRight()
    {
        return _jellyfishZonePosX + _jellyfishZoneSizeX;
    }
    public static float GetJellyfishZoneUp()
    {
        return _jellyfishZonePosY + _jellyfishZoneSizeY;
    }
    public static float GetJellyfishZoneDown()
    {
        return _jellyfishZonePosY + _jellyfishZoneSizeY;
    }
}
