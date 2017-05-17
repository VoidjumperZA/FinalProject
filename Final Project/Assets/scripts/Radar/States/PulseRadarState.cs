﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseRadarState : AbstractRadarState {

    // Fish detector
    private float _radarAngle;
    private float _scrollSpeed;
    private float _revealDuration;
    public PulseRadarState(radar pRadar, float pRadarAngle, float pScrollSpeed, float pRevealDuration) : base(pRadar)
    {
        _radarAngle = pRadarAngle;
        _scrollSpeed = pScrollSpeed;
        _revealDuration = pRevealDuration;
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
        float offset = Time.time * _scrollSpeed;
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
            if (collectable != _radar && collectable != basic.Hook && collectable != basic.Boat && collectable)
            {
                bool visible = Vector3.Dot(-_radar.gameObject.transform.up, (collectable.transform.position - _radar.gameObject.transform.position).normalized) > Mathf.Cos(_radarAngle);
                if (visible) collectable.Reveal(_revealDuration);
            }
        }
    }
}
