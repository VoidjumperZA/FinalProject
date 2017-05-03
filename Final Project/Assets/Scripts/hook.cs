using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hook : general
{
    // Fishing
    private boat _boat;
    private bool _fishing = false;

    // Movement
    [SerializeField] private float _ropeLength;
    [SerializeField] private float _speed;
    [SerializeField] private float _fallSpeed;
    private Vector3 _xyOffset;
    private Vector3 _velocity;
    // X Velocity damping
    [SerializeField] private float _xOffsetDamping;
    // States
    public enum HookState { None, Fish, Reel }
    private HookState _hookState = HookState.None;
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        Debug.Log(_hookState + " Hook");
        if (_selected)
        {
            StateNoneUpdate();
            StateFishUpdate();
        }
        else
        {
            DampXVelocityAfterDeselected();
            StateReelUpdate();
        }
        ApplyVelocity();
    }
    private void StateNoneUpdate()
    {
        if (_hookState == HookState.None)
        {
        }
    }
    private void StateFishUpdate()
    {
        if (_hookState == HookState.Fish)
        {
            if (Input.GetMouseButton(0)) SetXYAxisOffset(mouse.Instance.GetWorldPoint());
        }
    }
    private void StateReelUpdate()
    {
        if (_hookState == HookState.Reel)
        {
            Vector3 differenceVector = (_boat.gameObject.transform.position - gameObject.transform.position);
            if (differenceVector.magnitude >= _speed) gameObject.transform.Translate(differenceVector.normalized * _speed);
            if (differenceVector.magnitude < _speed)
            {
                gameObject.transform.position = _boat.transform.position;
                _hookState = HookState.None;
                _boat.SetState(boat.BoatState.None);
            }
        }
    }
    // -------- Fishing --------
    public void DeployHook()
    {
        if (_hookState == HookState.None)
        {
            _fishing = true;
            _hookState = HookState.Fish;
        }
    }
    // -------- Movement --------
    private void ApplyVelocity()
    {
        if (!_fishing) return;

        _velocity = new Vector3(_xyOffset.x * _speed, Mathf.Min(_xyOffset.y * _speed / 2, -_fallSpeed), 0);
        gameObject.transform.Translate(_velocity);
    }
    private void SetXYAxisOffset(Vector3 pPosition)
    {
        _xyOffset = new Vector3(pPosition.x - gameObject.transform.position.x, pPosition.y - gameObject.transform.position.y, 0);
        _xyOffset.Normalize();
    }
    // X Velocity damping
    private void DampXVelocityAfterDeselected()
    {
        _xyOffset *= _xOffsetDamping;
    }
    // -------- General Script Override --------
    public override void Select()
    {
        if (!_fishing) return;
        base.Select();
        //Debug.Log("hook - Select() " + _selected);
        _hookState = HookState.Fish;
        _xyOffset = Vector3.zero;

    }
    public override void Deselect()
    {
        base.Deselect();
        //Debug.Log("hook - Deselect() " + _selected);
        if (_fishing) _hookState = HookState.Fish;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_hookState == HookState.Fish)
        {
            if (other.gameObject.CompareTag("Floor"))
            {
                _hookState = HookState.Reel;
                _fishing = false;
            }
        }
    }
    public void AssignBoat(boat pBoat)
    {
        _boat = pBoat;
    }
    public void SetState(HookState pState)
    {
        _hookState = pState;
    }
}
