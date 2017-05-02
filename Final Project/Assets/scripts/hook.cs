using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hook : general {
    // Fishing
    private bool _fishing = false;

    // Movement
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _xOffsetDamping;
    private float _xOffset;
    private Vector3 _velocity;
    // X Velocity damping
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        if (_selected)
        {
            // Move state
            if (Input.GetMouseButton(0)) SetXAxisOffset(mouse.Instance.GetWorldPoint());
        }
        else
        {
            DampXVelocityAfterDeselected();
        }
        ApplyVelocity();
    }
    // -------- Fishing --------
    public void DeployHook()
    {
        _fishing = true;
    }
    // -------- Movement --------
    private void ApplyVelocity()
    {
        if (!_fishing) return;
        _velocity = new Vector3(_xOffset * _speed, -_fallSpeed, 0);
        gameObject.transform.Translate(_velocity);
    }
    private void SetXAxisOffset(Vector3 pPosition)
    {
        _xOffset = pPosition.x - gameObject.transform.position.x;
    }
    // X Velocity damping
    private void DampXVelocityAfterDeselected()
    {
        _xOffset *= _xOffsetDamping;
    }
    // -------- General Script Override --------
    public override void Select()
    {
        base.Select();
        Debug.Log("hook - Select() " + _selected);
        _xOffset = 0;

    }
    public override void Deselect()
    {
        base.Deselect();
        Debug.Log("hook - Deselect() " + _selected);
    }
}
