using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoatState : AbstractBoatState {
    private float _acceleration;
    private float _velocity;
    private Vector3 _destination = Vector3.zero;

    public MoveBoatState(boat pBoat, float pSpeed) : base(pBoat)
    {
        _acceleration = pSpeed;
    }

    public override void Start()
    {

    }
    public override void Update()
    {
        basic.Radar.SendPulse();
        SetDestination(mouse.GetWorldPoint());
        if (!MoveToDestination())
        {
            basic.Hook.SetState(hook.HookState.None);
            _boat.SetState(boat.BoatState.None);

        }
    }
    private void SetDestination(Vector3 pPosition)
    {
        if (!Input.GetMouseButton(0)) return;
        _destination = new Vector3(pPosition.x, _boat.gameObject.transform.position.y, _boat.gameObject.transform.position.z);
    }
    private bool MoveToDestination()
    {
        Vector3 differenceVector = _destination - _boat.gameObject.transform.position;
        if (differenceVector.magnitude < _velocity)
        {
            _velocity = 0;
            return false;
        }
        else
        {
            _velocity += _acceleration * Time.deltaTime;
            _boat.gameObject.transform.Translate(differenceVector.normalized * _velocity);
            basic.Hook.SetState(hook.HookState.FollowBoat);
            return true;
        }

    }
    public override void Refresh()
    {
        _destination = _boat.gameObject.transform.position;
        _velocity = 0;
    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.Move;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FishingArea")
        {
            GameObject.Find("Manager").GetComponent<TempFishSpawn>().CalculateNewSpawnDensity();
        }
            
    }
}
