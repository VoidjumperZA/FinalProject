using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneRadarState : AbstractRadarState {

	public NoneRadarState(radar pRadar) : base(pRadar)
    {

    }
    public override void Start()
    {
        _radar.Renderer.enabled = false;
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {
        
    }
    public override radar.RadarState StateType()
    {
        return radar.RadarState.None;
    }
}
