using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
    private bool _active = false;
    [SerializeField] private float _cooldownDuration;
    private counter _cooldown;

	public float scrollSpeed = 0.5F;
    [SerializeField] private Renderer _renderer;
	void Start() {
        _cooldown = new counter(_cooldownDuration);
        _renderer.enabled = false;
	}
	void Update() {
        if (Input.GetKeyDown(KeyCode.L)) SendPulse();
        if (_active)
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
	}
    public void SendPulse()
    {
        _renderer.enabled = true;
        _active = true;
        _cooldown.Reset();

    }
}