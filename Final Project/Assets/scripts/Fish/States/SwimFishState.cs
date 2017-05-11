using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimFishState : AbstractFishState
{
    private float _speed;
    public SwimFishState(fish pFish, float pSpeed) : base(pFish)
    {
        _speed = pSpeed;
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        _fish.gameObject.transform.Translate(Vector3.forward * _speed);
    }
    public override void Refresh()
    {

    }
    public override fish.FishState StateType()
    {
        return fish.FishState.Swim;
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Fish Despawner" || other.gameObject.tag == "Floor")
        {
            basic.Generals.Remove(_fish);
            GameObject.Destroy(_fish.gameObject);
        }
    }
}
