﻿using System.Collections;
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

    private general _selected = null;
    private List<general> _generals = new List<general>();

    void Start()
    {
        _generals.Add(SpawnBoat());
        _generals.Add(SpawnHook());

        ((boat)_generals[0]).AssignHook((hook)_generals[1]);
        ((boat)_generals[0]).AssignRadar(SpawnRadar());
        ((hook)_generals[1]).AssignBoat((boat)_generals[0]);

        Debug.Log(_generals.Count + " generals");
        //InputTimer and basic should be on the same object, but I'm explictly calling in case they ever aren't
        //and therefore I can still get the script
        _inputTimer = GameObject.Find("Manager").GetComponent<InputTimer>();
        if (_inputTimer == null)
        {
            Debug.Log("ERROR: Cannot get a reference to InputTimer from the Manager object.");
        }
        CameraHandler.ArtificialStart();
    }
    void Update()
    {
        CameraHandler.ArtificialUpdate();
        SelectNewGeneral();
        DeselectPreviousGeneral();
        RenderTrail(_generals[0].gameObject.transform.position, _generals[1].gameObject.transform.position);
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
        return Instantiate(_radarPrefab, _boatSpawn.position, Quaternion.identity).GetComponent<Radar>();
    }
    private hook SpawnHook()
    {
        return Instantiate(_hookPrefab, _generals[0].transform.position, Quaternion.identity).GetComponent<hook>();
    }
    private void RenderTrail(Vector3 pPositionOne, Vector3 pPositionTwo)
    {
        _lineRenderer.SetPositions(new Vector3[] { pPositionOne, pPositionTwo });
    }

}
