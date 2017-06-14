using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRadarState
{
    protected radar _radar = null;

    public AbstractRadarState(radar pRadar)
    {
        if (!_radar) _radar = pRadar;
    }

    public abstract void Start();

    public abstract void Update();
    public virtual void FixedUpdate()
    {

    }
    public virtual void Refresh()
    {

    }

    public abstract radar.RadarState StateType();
    public virtual void SetState(radar.RadarState pState)
    {
        _radar.SetState(pState);
    }
}