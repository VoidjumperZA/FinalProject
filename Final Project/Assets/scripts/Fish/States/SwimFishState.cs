using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimFishState : AbstractFishState
{
    private counter _outlineCounter;
    private float _speed;
    public SwimFishState(fish pFish, float pSpeed, float pRevealDuration) : base(pFish)
    {
        _speed = pSpeed;
        _outlineCounter = new counter(pRevealDuration);
    }
    public override void Start()
    {
        _outlineCounter.Reset();
    }
    public override void Update()
    {
        _fish.gameObject.transform.Translate(Vector3.forward * _speed);
        if (_fish.Revealed) HandleOutline();
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
            GameObject.Find("Manager").GetComponent<TempFishSpawn>().RemoveOneFishFromTracked();
            basic.Generals.Remove(_fish);
            GameObject.Destroy(_fish.gameObject);
        }
    }
    private void HandleOutline()
    {
        _outlineCounter.Count();
        //if (_outlineCounter.Remaining(0.33f)) _blink = true;
        if (_outlineCounter.Done())
        {
            _fish.Hide();
            //_blink = false;
            _outlineCounter.Reset();
        }

        /*if (_blink)
        {

        }*/
    }
}
