using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimFishState : AbstractFishState
{
    private counter _outlineCounter;
    private float _speed;
    public float RevealDuration;
    public SwimFishState(fish pFish, float pSpeed) : base(pFish)
    {
        _speed = pSpeed;
        _outlineCounter = new counter(0);
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
            basic.RemoveCollectable(_fish);
            GameObject.Destroy(_fish.gameObject);
        }
    }
    private void HandleOutline()
    {
        _outlineCounter.Count();
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
    public void ResetOutLineCounter(float pRevealDuration)
    {
        _outlineCounter.Reset(pRevealDuration);
    }
}
