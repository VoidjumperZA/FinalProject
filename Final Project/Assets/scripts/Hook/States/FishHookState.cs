using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookState : AbstractHookState
{
    private float _speed;
    private float _fallSpeed;
    private Vector3 _xyOffset;
    private float _xOffsetDamping;
    private Vector3 _velocity;

    public FishHookState(hook pHook, float pSpeed, float pOffsetDamping, float pFallSpeed) : base(pHook)
    {
        _speed = pSpeed;
        _xOffsetDamping = pOffsetDamping;
        _fallSpeed = pFallSpeed;
    }
    public override void Start()
    {

    }
    public override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetXYAxisOffset(mouse.GetWorldPoint());
        }
        ApplyVelocity();
        DampXVelocityAfterDeselected();
    }
    public override void Refresh()
    {
        _xyOffset = Vector2.zero;
    }
    public override hook.HookState StateType()
    {
        return hook.HookState.Fish;
    }
    private void SetXYAxisOffset(Vector3 pPosition)
    {
        _xyOffset = new Vector3(pPosition.x - _hook.gameObject.transform.position.x, pPosition.y - _hook.gameObject.transform.position.y, 0);
        _xyOffset.Normalize();
    }
    private void ApplyVelocity()
    {
        _velocity = new Vector3(_xyOffset.x * _speed, Mathf.Min(_xyOffset.y * _speed / 2, -_fallSpeed), 0);
        _hook.gameObject.transform.Translate(_velocity);
    }
    private void DampXVelocityAfterDeselected()
    {
        if (_xyOffset.magnitude > 0) _xyOffset *= _xOffsetDamping;
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (!_hook || !other) return;
        //Reel the hook in if you touch the floor
        if (other.gameObject.CompareTag("Floor"))
        {
            SetState(hook.HookState.Reel);
            GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
        } //On contact with a fish
        if (other.gameObject.CompareTag("Fish"))
        {
            fish theFish = other.gameObject.GetComponent<fish>();
            if (!theFish.Visible) return;
            theFish.SetState(fish.FishState.FollowHook);
            _hook.FishOnHook.Add(theFish);
            GameObject.Find("Manager").GetComponent<ScoreHandler>().AddScore(theFish.GetScore(), true);
            GameObject.Find("Manager").GetComponent<Combo>().CheckComboProgress(theFish.fishType);
            if (theFish.fishType == fish.FishType.Large)
            {
                //SetState(hook.HookState.Reel);
                
            }
        }
    }
}
