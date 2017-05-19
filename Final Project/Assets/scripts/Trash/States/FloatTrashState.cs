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

    private int _stayVisibleRange;
    public FloatTrashState(trash pTrash, float pSpinsPerSeconds, float pSpinRadius) : base(pTrash)
    {
        _spinsPerSecond = pSpinsPerSeconds;
        _spinRadius = pSpinRadius;
        _outlineCounter = new counter(0);
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
        if (Mathf.Abs(basic.Radar.transform.position.x - _trash.transform.position.x) > _stayVisibleRange)
        {
            _outlineCounter.Count();
            if (_outlineCounter.Done())
            {
                _trash.Hide();
                _outlineCounter.Reset();
            }
        }
    }
    private void FloatAround()
    {
        _trash.gameObject.transform.Translate(new Vector3(0, 
                                                          Mathf.Sin(2*Mathf.PI * Time.time * _spinsPerSecond) * _spinDirection, 
                                                          Mathf.Cos(2 * Mathf.PI * Time.time * _spinsPerSecond) * _spinDirection).normalized * _spinRadius);
    }
    public void ResetOutLineCounter(float pRevealDuration, int pCollectableStaysVisibleRange)
    {
        _stayVisibleRange = pCollectableStaysVisibleRange;
        _outlineCounter.Reset(pRevealDuration);
    }
}
