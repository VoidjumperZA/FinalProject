using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseRadarState : AbstractRadarState {

    // Fish detector
    private float _radarAngle;
    private float _scrollSpeed;
    private float _fadeOutDuration;
    private int _collectableStaysVisibleRange;
    public PulseRadarState(radar pRadar, float pRadarAngle, float pScrollSpeed, float pFadeOutDuration, int pCollectableStaysVisibleRange) : base(pRadar)
    {
        _radarAngle = pRadarAngle;
        _scrollSpeed = pScrollSpeed;
        _fadeOutDuration = pFadeOutDuration;
        _collectableStaysVisibleRange = pCollectableStaysVisibleRange;
    }
	public override void Start () {
        _radar.Renderer.enabled = true;

    }
	public override void Update () {
	}
    public override void FixedUpdate()
    {
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
    }
    private void DetectCollectables()
    {
        if (basic.Generals == null) Debug.Log("IS NULL");
        if (Time.time % 1.0f != 0) return;
        foreach (general collectable in basic.Generals)
        {
            if (collectable == null) continue;

            bool visible = Vector3.Dot(-_radar.gameObject.transform.up, (collectable.transform.position - _radar.transform.position).normalized) >= _radarAngle;
            if (visible) collectable.Reveal(_fadeOutDuration, _collectableStaysVisibleRange);
        }
    }

    /*IEnumerator DoScan()
    {
        //for(all fish)
        //scan 1 or a few fish
        //yield return null
    }*/

}
