using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
    private bool _active = false;
    [SerializeField] private float _cooldownDuration;
    private counter _cooldown;

	public float scrollSpeed = 0.5F;
    [SerializeField] private Renderer _renderer;
	void Start() {
        _renderer.enabled = false;
        _cooldown = new counter(_cooldownDuration);
	}
	void Update() {
        if (_active) VisualiseRadarCone();
	}
    private void VisualiseRadarCone()
    {
        _cooldown.Count();
        float offset = Time.time * scrollSpeed;
        _renderer.material.mainTextureOffset = new Vector2(0, offset);
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
}