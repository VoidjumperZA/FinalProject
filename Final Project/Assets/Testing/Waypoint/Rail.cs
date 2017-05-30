using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private bool _loop = false;
    [SerializeField] private bool _mirror = false;
    [SerializeField] private bool _flipped = false;
    private bool _active = false;

    public TrackID Track = TrackID.None;
    [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();
    private int _currentWaypoint = 0;
    private Vector3 _toPosition;
    private Quaternion _toRotation;

    [SerializeField] private float _cycleTime;
    private float _timePerWaypoint;
    private float _currentTime = 0;
    private float _time = 0;

    private Vector3 _fromPosition;
    private Quaternion _fromRotation;

    private Color color;
    public void Activate()
    {
        _active = true;
    }
    public void Start () {
        SetDebugColor();
        _timePerWaypoint = _cycleTime / _waypoints.Count;

        if (_flipped) Flip();
        Refresh();
        //basic.Cameracontroller.AddTrail(Track, this);
    }
    public void Run(ref Camera pCamera)
    {
        if (_active) Move(ref pCamera);
    }
    public void Refresh()
    {
        _currentWaypoint = 0;
        Next();
        _active = false;
    }
    void Update () {

        for (int i = 1; i < _waypoints.Count; i++) Debug.DrawLine(_waypoints[i - 1].Position(), _waypoints[i].Position(), color);
	}
    private void Move(ref Camera pCamera)
    {
        _currentTime += Time.deltaTime;
        //_time += Time.deltaTime;
        if (_currentTime <= _timePerWaypoint)
        {
            float lerp = _currentTime / _timePerWaypoint;
            pCamera.transform.position = Vector3.Lerp(_fromPosition, _toPosition, lerp);
            pCamera.transform.rotation = Quaternion.Slerp(_fromRotation, _toRotation, lerp);
        }
        if (_currentTime > _timePerWaypoint)
        {
            if (_currentWaypoint < _waypoints.Count - 1) Next(); 
            else _active = false; 
        }
    }
    public void Next()
    {
        _fromPosition = _waypoints[_currentWaypoint].Position();
        _fromRotation = _waypoints[_currentWaypoint].Rotation();
        _currentWaypoint += 1;
        _toPosition = _waypoints[_currentWaypoint].Position();
        _toRotation = _waypoints[_currentWaypoint].Rotation();

        _currentTime = 0;
        _time = 0;
    }
    public void Flip()
    {
        _waypoints.Reverse();
    }

    private void SetDebugColor()
    {
        switch (Track)
        {
            case TrackID.BoatToOcean:
                color = Color.red;
                break;
            case TrackID.OceanToBoat:
                color = Color.black;
                break;
            case TrackID.OceanToHook:
                color = Color.green;
                break;
            case TrackID.HookToOcean:
                color = Color.blue;
                break;
        }
    }
}
