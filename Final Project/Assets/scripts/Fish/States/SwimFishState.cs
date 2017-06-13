using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimFishState : AbstractFishState
{
    private counter _outlineCounter;
    private float _speed;

    private int _stayVisibleRange;
    public float RevealDuration;
    private bool _firstTimeRevealed = false;
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
        //if (_fish.Revealed) HandleOutline();
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
        if (other.gameObject.tag == "FishDespawner" || other.gameObject.tag == "Floor")
        {
            GameManager.Fishspawner.QueueFishAgain(_fish, true, true, true);
        }
    }
    /*private void HandleOutline()
    {
        if (Mathf.Abs(GameManager.Radar.transform.position.x - _fish.transform.position.x) > _stayVisibleRange)
        {
            _outlineCounter.Count();
            _fish._color.a = _outlineCounter.PercentageLeft();
            _fish._material.color = _fish._color;
            if (_outlineCounter.Done())
            {
                _fish.Hide();
                _outlineCounter.Reset();
            }
        }
    }
    public void ResetOutLineCounter(float pRevealDuration, int pStayVisibleRange)
    {
        _stayVisibleRange = pStayVisibleRange;
        _outlineCounter.Reset(pRevealDuration);
    }*/
}
