using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterSceneBoatState : AbstractBoatState
{
    private float _acceleration;
    private float _velocity = 0;
    private float _maxVelocity;
    private float _deceleration;
    private Vector3 _destination;
    private float _halfDestination;

    private float _currentLerpTime = 0;
    private float _totalLerpTime = 4;
    private Vector3 _fromPosition;

    public EnterSceneBoatState(boat pBoat, float pAcceleration, float pMaxVelocity, float pDeceleration) : base(pBoat)
    {
        _acceleration = pAcceleration;
        _maxVelocity = pMaxVelocity;
        _deceleration = pDeceleration;
    }

    public override void Start()
    {
        _boat.transform.position = new Vector3(0, -3.5f, 0);
        _fromPosition = _boat.transform.position;
        _currentLerpTime = 0;
    }

    public override void Update()
    {
        _currentLerpTime += Time.deltaTime;
        if (_currentLerpTime <= _totalLerpTime)
        {
            float lerp = _currentLerpTime / _totalLerpTime;
            _boat.transform.position = Vector3.Lerp(_fromPosition, _destination, lerp);
        }
        else
        {
            _boat.transform.position = _destination;
            SetState(boat.BoatState.Stationary);

        }
    }
    public override void Refresh()
    {
        GameManager.UIOnEnterScene();
        /*basic.GlobalUI.DeployHookButton(true);
        basic.GlobalUI.ShowTotalScore(true);
        basic.GlobalUI.BeginGameTimer();
        basic.Tempfishspawn.StartFertilityDegradeCoroutine();

        //Once in the centre of the ocean, not in the tutorial sequence
        if (basic.GlobalUI.GetInTutorial() == false)
        {
            basic.Tempfishspawn._boatSetUp = true;
            basic.Seafloorspawning.SpawnTrash();
            basic.Seafloorspawning.SpawnSpecialItems();
        }
       
        if (basic.GlobalUI.InTutorial) basic.GlobalUI.ShowHandHookButton(true);*/
    }

    public override boat.BoatState StateType()
    {
        return boat.BoatState.EnterScene;
    }
    public override void SetDestination(Vector3 pDestination)
    {
        _destination = pDestination;
    }
}
