using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : general {
    // Pulse visualisation
    private bool _active = false;
    [SerializeField] private float _cooldownDuration;
    private counter _cooldown;
	public float scrollSpeed = 0.5F;
    [SerializeField] private Renderer _renderer;
    // Fish detector
    [SerializeField] private float _radarAngle;
	public override void Start() {
        _renderer.enabled = false;
        _cooldown = new counter(_cooldownDuration);
	}
	public override void Update() {
        if (_active) VisualiseRadarCone();
	}
    private void VisualiseRadarCone()
    {
        _cooldown.Count();
        float offset = Time.time * scrollSpeed;
        _renderer.material.mainTextureOffset = new Vector2(0, offset);
        DetectCollectables();
        if (_cooldown.Done())
        {
            _active = false;
            _renderer.enabled = false;
        }
    }
    public void SendPulse()
    {
        if (_active) return;
        
        _renderer.enabled = true;
        _active = true;
        _cooldown.Reset();

    }
    private void DetectCollectables()
    {
        foreach (general collectable in basic.Generals)
        {

            bool visible = Vector3.Dot(-gameObject.transform.up, (collectable.transform.position - gameObject.transform.position).normalized) > Mathf.Cos(_radarAngle);
            if (visible) collectable.Reveal();
            /*pFish.ToggleOutliner(visible);
            pFish.ToggleRenderer(visible);*/

        }
    }
}