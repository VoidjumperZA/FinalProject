using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counter {
    private bool _active = false;
    private bool _done = false;

    private float _step;
    private float _counter = 0;
    private float _limit;
    private bool _realTime;

    private bool _passedPercentage = false;

    public counter(float pLimit, float pStep = 0.0f)
    {
        _limit = pLimit;
        _step = pStep;
        _realTime = (pStep == 0.0f) ? true : false;
        //Debug.Log("Counter(" + _limit + ", " + _step + ") created!");
    }

    public void Reset()
    {
        _counter = 0;
        _active = true;
        _done = false;
    }
    public void Reset(float pLimit, float pStep = 0.0f, bool pRealTime = true)
    {
        _limit = pLimit;
        _step = pStep;
        _realTime = pRealTime;
        Reset();
    }
    public void Count()
    {
        if (_active)
        {
            _counter += (_realTime) ? Time.deltaTime : _step;
            if (_counter >= _limit)
            {
                _counter = 0;
                _active = false;
                _done = true;
            }
        }
    }
    public void Increase()
    {
        if (!_active && _counter == 0) _active = true;
        if (_active && _counter < _limit)
        {
            _counter += (_realTime) ? Time.deltaTime : _step;
            if (_counter >= _limit)
            {
                _counter = _limit;
                _active = false;
                _done = true;
            }
        }
    }
    public void SetActive(bool pBool)
    {
        _active = pBool;
    }
    public bool Done()
    {
        return _done;
    }
    public float PercentagePassed()
    {
        return _counter / _limit;
    }
    public float PercentageLeft()
    {
        return 1 - (_counter / _limit);
    }
    public bool Remaining(float pPercentage)
    {
        if (_passedPercentage) return false;
        if ((_counter / _limit) + pPercentage >= 0.9f || (_counter / _limit) + pPercentage <= 1.1f)
        {
            _passedPercentage = true;
            return true;
        }
        else return false;
    }
}
