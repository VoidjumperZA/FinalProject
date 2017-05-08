using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : general {
    // Fishing
    private hook _hook = null;
    // Movement
    [SerializeField] private float _speed;
    private Vector3 _destination;
    // Action recognizion
    private counter _counter;
    // States
    public enum BoatState { None, Move, Fish }
    private BoatState _boatState = BoatState.None;

    //Camera and zoom levels
    private Camera mainCam;
    private GameplayValues gameplayValues;
    Vector3 cameraPosZoomedHook;
    Vector3 cameraPosFocusBoat;
    Vector3 cameraPosOceanOverview;
    public override void Start()
    {
        base.Start();
        _counter = new counter(0.3f);
        mainCam = Camera.main;

        gameplayValues = GameObject.Find("Manager").GetComponent<GameplayValues>();

        //Focus Boat Level
        cameraPosFocusBoat = mainCam.transform.position;
        cameraPosFocusBoat.z += gameplayValues.GetCamZoomFocusBoat();

        //Zoomed Hook Level
        cameraPosZoomedHook = mainCam.transform.position;
        cameraPosZoomedHook.z += gameplayValues.GetCamZoomZoomedHook();

        //Ocean Overview Level
        cameraPosOceanOverview = mainCam.transform.position;
        cameraPosOceanOverview.z += gameplayValues.GetCamZoomOceanOverview();
        // After inicialization
    }
    public override void Update()
    {
       // Debug.Log(_boatState + " Boat");
        if (_selected)
        {
            StateNoneUpdate();
            StateMoveUpdate();
            StateFishUpdate();
        }
    }
    // -------- State Machine --------
    private void StateNoneUpdate()
    {
        if (_boatState == BoatState.None)
        {
            _counter.Count();
            if (_counter.Done())
            {
                _boatState = SidewaysOrDownwards() ? BoatState.Move : BoatState.Fish;
                if (_boatState == BoatState.Fish)
                {
                    //Debug.Log("Switching cam parent to hook.");
                    GameObject hookCam = GameObject.FindGameObjectWithTag("HookCamHolder");
                    //mainCam.transform.position = hookCam.transform.position;
                    mainCam.transform.SetParent(hookCam.transform);
                    mainCam.transform.position = cameraPosZoomedHook;
                }
            }
        }
    }
    private void StateMoveUpdate()
    {
        if (_boatState == BoatState.Move)
        {
            if (Input.GetMouseButton(0))
            {
                SetDestination(mouse.Instance.GetWorldPoint());
                GameObject.Find("Manager").GetComponent<InputTimer>().ResetClock();
            }
            MoveToDestination();
        }
    }
    private void StateFishUpdate()
    {
        if (_boatState == BoatState.Fish)
        {
            _hook.DeployHook();
        }
    }
    // -------- Action Recognizion --------
    private bool SidewaysOrDownwards()
    {
        Vector3 mouseWorldPoint = mouse.Instance.GetWorldPoint();
        return Mathf.Abs(mouseWorldPoint.x - gameObject.transform.position.x) > Mathf.Abs(mouseWorldPoint.y - gameObject.transform.position.y);
    }
    // -------- Movement --------
    private void MoveToDestination()
    {
        if (_destination == Vector3.zero) return;
        Vector3 differenceVector = _destination - gameObject.transform.position;
        if (differenceVector.magnitude >= _speed)
        {
            gameObject.transform.Translate(differenceVector.normalized * _speed);
            _hook.gameObject.transform.position = gameObject.transform.position;
        }

    }
    private void SetDestination(Vector3 pPosition)
    { 
        // Debug.Log(pPosition.ToString() + " mouseWorldPoint");
        _destination = new Vector3(pPosition.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    // -------- Fishing --------
    // -------- General Script Override --------
    public override void Select()
    {
        base.Select();
        //Debug.Log("boat - Select() " + _selected);
        _counter.Reset();

    }
    public override void Deselect()
    {
        base.Deselect();
        //Debug.Log("boat - Deselect() " + _selected);
        if (_boatState != BoatState.Fish) _boatState = BoatState.None;
        _counter.SetActive(false);
    }
    public void AssignHook(hook pHook)
    {
        _hook = pHook;
    }
    public void SetState(BoatState pState)
    {
        _boatState = pState;
    }
}
