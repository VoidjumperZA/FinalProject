using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTrashState : AbstractTrashState
{
    private counter _outlineCounter;
    private float _speed;
    public FloatTrashState(trash pTrash, float pSpeed, float pRevealDuration) : base(pTrash)
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
        if (_trash.Revealed) HandleOutline();
    }
    public override void Refresh()
    {

    }
    public override trash.TrashState StateType()
    {
        return trash.TrashState.Float;
    }
    private void HandleOutline()
    {
        _outlineCounter.Count();
        //if (_outlineCounter.Remaining(0.33f)) _blink = true;
        if (_outlineCounter.Done())
        {
            _trash.Hide();
            //_blink = false;
            _outlineCounter.Reset();
        }

        /*if (_blink)
        {

        }*/
    }
}
