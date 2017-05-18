using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTrashState : AbstractTrashState
{
    private counter _outlineCounter;
    private float _spinsPerSecond;
    private float _spinRadius;
    private int _spinDirection;
    public FloatTrashState(trash pTrash, float pSpinsPerSeconds, float pRevealDuration, float pSpinRadius) : base(pTrash)
    {
        _spinsPerSecond = pSpinsPerSeconds;
        _spinRadius = pSpinRadius;
        _outlineCounter = new counter(pRevealDuration);
        _spinDirection = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
    }
    public override void Start()
    {
        _outlineCounter.Reset();
    }
    public override void Update()
    {
        if (_trash.Revealed) HandleOutline();
        FloatAround();
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
        if (_outlineCounter.Done())
        {
            _trash.Hide();
            _outlineCounter.Reset();
        }
    }
    private void FloatAround()
    {
        _trash.gameObject.transform.Translate(new Vector3(0, 
                                                          Mathf.Sin(2*Mathf.PI * Time.time * _spinsPerSecond) * _spinDirection, 
                                                          Mathf.Cos(2 * Mathf.PI * Time.time * _spinsPerSecond) * _spinDirection).normalized * _spinRadius);
    }
    public void ResetOutLineCounter(float pRevealDuration)
    {
        _outlineCounter.Reset(pRevealDuration);
    }
}
