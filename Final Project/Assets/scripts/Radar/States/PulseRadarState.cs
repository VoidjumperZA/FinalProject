using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseRadarState : AbstractRadarState {

    // Fish detector
    private float _radarAngle;
    private float scrollSpeed = 0.5F;
    public PulseRadarState(radar pRadar, float pRadarAngle) : base(pRadar)
    {
        _radarAngle = pRadarAngle;
    }
	public override void Start () {
        _radar.Renderer.enabled = true;
		
	}
	public override void Update () {
        VisualiseRadarCone();
	}
    public override void Refresh()
    {

    }
    public override radar.RadarState StateType()
    {
        return radar.RadarState.Pulse;
    }
    private void VisualiseRadarCone()
    {
        float offset = Time.time * scrollSpeed;
        _radar.Renderer.material.mainTextureOffset = new Vector2(0, offset);
        DetectCollectables();
        /*if (_cooldown.Done())
        {
            _active = false;
            _renderer.enabled = false;
        }*/
    }
    private void DetectCollectables()
    {
        if (basic.Generals == null) Debug.Log("IS NULL");
        foreach (general collectable in basic.Generals)
        {
            bool visible = Vector3.Dot(-_radar.gameObject.transform.up, (collectable.transform.position - _radar.gameObject.transform.position).normalized) > Mathf.Cos(_radarAngle);
            if (visible) collectable.Reveal();
        }
    }
}
