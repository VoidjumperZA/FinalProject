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

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _boatSpawn;
    [SerializeField] private Transform _boatSetUp;
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private GameObject _trailerPrefab;
    [SerializeField] private GameObject _radarPrefab;
    [SerializeField] private GameObject _hookPrefab;

    private int cameraHandlerUpdateKey;
    private static List<general> _generals = new List<general>(); public static List<general> Generals { get { return _generals; } }
    public static boat Boat;
    public static hook Hook;

    public static radar Radar;

    //public static radar Radar;
    public static trailer Trailer;
    
    private GameObject floor;
    private static float seaDepth;
    //private GameObject _docks;
    //private GameObject _endOfLevel;
    //private static float _seaWidth;
   

    void Start()
    {
        Boat = SpawnBoat(_boatSpawn.position, _boatSetUp.position);
        Hook = SpawnHook();
        Radar = SpawnRadar();
        Trailer = SpawnTrailer();

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
        Gameplayvalues = GetComponent<GameplayValues>(); Debug.Log("ERROR: Cannot get reference to GameplayValues from Manager object");

        CameraHandler.InitializeCameraHandler();
        CameraHandler.SetViewPoint(CameraHandler.CameraFocus.Boat);

        //Find out seaDepth
        floor = GameObject.FindGameObjectWithTag("Floor");
        Vector3 difference = floor.transform.position - Boat.transform.position;

        seaDepth = Mathf.Abs(difference.y);
        //Find out seaWidth
        //_docks = GameObject.FindGameObjectWithTag("Docks"); if (!_docks) Debug.Log("WARNING (Jellyfish uses this): You need to create the Docks and tag it with Docks");
        //_endOfLevel = GameObject.FindGameObjectWithTag("EndOfLevel"); if (!_endOfLevel) Debug.Log("WARNING (Jellyfish uses this): You need to create an empy object, place it at the end of the level (x) and tag it with EndOfLevel");
        //_seaWidth = Vector3.Distance(_docks.transform.position, _endOfLevel.transform.position);
        //
        //CameraHandler.ArtificialStart();
        //cameraHandlerUpdateKey = CameraHandler.RequestUpdateCallPermission();
    }
    void Update()
    {
        RenderTrail(Boat.transform.position, Hook.gameObject.transform.position);
        if (Input.GetMouseButton(0)) _inputTimer.ResetClock();
        CameraHandler.Update();
    }

    private void LateUpdate()
    {
        //CameraHandler.ArtificialUpdate(cameraHandlerUpdateKey);
    }
    private boat SpawnBoat(Vector3 pSpawnPosition, Vector3 pSetUpPosition)
    {
        boat theBoat = Instantiate(_boatPrefab, pSpawnPosition, Quaternion.identity).GetComponent<boat>();
        theBoat.SetSetUpPosition(pSetUpPosition);
        _generals.Add(theBoat);
        return theBoat;
    }
    private hook SpawnHook()
    {
        hook theHook = Instantiate(_hookPrefab, _generals[0].transform.position, Quaternion.identity).GetComponent<hook>();
        _generals.Add(theHook);
        return theHook;
    }
    private radar SpawnRadar()
    {
        radar theRadar = Instantiate(_radarPrefab, _boatSpawn.position + new Vector3(0,-5f,0.25f), Quaternion.identity).GetComponent<radar>();
        _generals.Add(theRadar);
        return theRadar;
    }
    private trailer SpawnTrailer()
    {
        trailer theTrailer = Instantiate(_trailerPrefab, Boat.transform.position + new Vector3(-10,0,0), _trailerPrefab.transform.rotation).GetComponent<trailer>();
        theTrailer.gameObject.transform.SetParent(Boat.gameObject.transform);
        _generals.Add(theTrailer);
        return theTrailer;
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

    public static float GetSeaDepth()
    {
        return seaDepth;
    }

    /*public static float GetSeaWidth()
    {
        return _seaWidth;
    }*/
}
