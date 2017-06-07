using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpBoatState : AbstractBoatState
{
    private float _acceleration;
    private float _velocity = 0;
    private float _maxVelocity;
    private float _deceleration;
    private Vector3 _destination;
    private float _halfDestination;
    private GameObject[] levelBoundaries;
    private GameObject[] doubleBackPoints;

    public SetUpBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration, Vector3 pDestination) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
        _destination = pDestination;
        levelBoundaries = GameObject.FindGameObjectsWithTag("LevelBoundary");
        doubleBackPoints = GameObject.FindGameObjectsWithTag("DoubleBackDestination");
        //disble the boundaries and double back points
        setBoundaryDoubleBackState(false);
        basic.Tempfishspawn._boatSetUp = true;
    }

    public override void Start()
    {
        _halfDestination = (_destination - _boat.gameObject.transform.position).magnitude / 2;
        //_boat.gameObject.transform.position = _destination;
    }

    public override void Update()
    {
        Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
        if (differenceVector.magnitude > _halfDestination) _velocity += _acceleration;
        else _velocity -= _acceleration;
        if (_velocity > _maxVelocity) _velocity = _maxVelocity;
        if (_velocity < 0 || differenceVector.magnitude <= _velocity)
        {
            _velocity = 0;
            _boat.gameObject.transform.position = _destination;
            SetState(boat.BoatState.Stationary);
            //enable the boundaries and double back points
            setBoundaryDoubleBackState(true);
        }
        _boat.transform.Translate(differenceVector.normalized * _velocity);
    }

    private void setBoundaryDoubleBackState(bool pState)
    {
        for (int i = 0; i < levelBoundaries.Length; i++)
        {
            levelBoundaries[i].SetActive(pState);
            doubleBackPoints[i].SetActive(pState);
        }
    }
    public override void Refresh()
    {
        
        basic.GlobalUI.DeployHookButton(true);
        basic.GlobalUI.ShowTotalScore(true);
        basic.GlobalUI.BeginGameTimer();
        if (basic.GlobalUI.InTutorial) basic.GlobalUI.ShowHandHookButton(true);
    }

    public override boat.BoatState StateType()
    {
        return boat.BoatState.SetUp;
    }
}
