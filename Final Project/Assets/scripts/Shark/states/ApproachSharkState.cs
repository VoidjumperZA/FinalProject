using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachSharkState : AbstractSharkState
{
    private bool _approach = true;
    private Transform _sharkSpawn;
    private Vector3 _destinationOffset;
    private float _approachCounter = 0;
    private float _approachDuration;
    public ApproachSharkState(shark pShark, Transform pSharkSpawn, Vector3 pDestinationOffset, float pApproachDuration) : base(pShark)
    {
        _sharkSpawn = pSharkSpawn;
        _destinationOffset = pDestinationOffset;
        _approachDuration = pApproachDuration;
    }
    public override void Start()
    {
        _approach = true;
        _shark.transform.position = _sharkSpawn.position;
        _shark.transform.rotation = _sharkSpawn.rotation;
        _approachCounter = 0;
    }
    public override void Update()
    {
        if (_approach)
        {
            _approachCounter += Time.deltaTime;
            if (_approachCounter < _approachDuration)
            {
                float lerp = _approachCounter / _approachDuration;
                _shark.transform.position = Vector3.Lerp(_sharkSpawn.position, basic.Hook.transform.position + _destinationOffset, lerp);
            }
            else
            {
                _shark.transform.position = basic.Hook.transform.position + _destinationOffset;
                _approach = false;
                SetState(shark.SharkState.None);
            }
        }
    }
    public override void Refresh()
    {

    }
    public override shark.SharkState StateType()
    {
        return shark.SharkState.Approach;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
