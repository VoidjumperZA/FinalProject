﻿using System;
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

    //Screen shake
    private bool camShaking;
    private int screenShakeDuration;
    private int screenShakeCounter;

    // Rotation
    private float hookRotationAmount;
    private float maxHookRotation;
    private float currentHookRotation;

    public FishHookState(hook pHook, float pSpeed, float pOffsetDamping, float pFallSpeed) : base(pHook)
    {
        _speed = pSpeed;
        _xOffsetDamping = pOffsetDamping;
        _fallSpeed = pFallSpeed;
    }

    //
    public override void Start()
    {

        hookRotationAmount = 1.0f;
        currentHookRotation = 0.0f;
        maxHookRotation = 25.0f;
        CameraHandler.SetViewPoint(CameraHandler.CameraFocus.Hook);
    }

    //
    public override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetXYAxisOffset(mouse.GetWorldPoint());
            //Debug.Log(mouse.GetWorldPoint().ToString());
        }
        ApplyVelocity();
        DampXVelocity();
        //SetCameraAndHookAngle();
    }

    // -------- Movement --------
    private void SetCameraAndHookAngle()
    {
        if (_xyOffset.x < 0)
        {
            if (currentHookRotation < maxHookRotation)
            {
                currentHookRotation += hookRotationAmount;
                _hook.gameObject.transform.Rotate(0.0f, 0.0f, currentHookRotation);
                Camera.main.transform.Rotate(0.0f, 0.0f, -currentHookRotation);
            }
        }
        else if (_xyOffset.x > 0)
        {
            if (currentHookRotation > -maxHookRotation)
            {
                currentHookRotation -= hookRotationAmount;
                _hook.gameObject.transform.Rotate(0.0f, 0.0f, -currentHookRotation);
                Camera.main.transform.Rotate(0.0f, 0.0f, currentHookRotation);
            }
        }
    }

    //
    public override void Refresh()
    {
        _xyOffset = Vector2.zero;
    }

    //
    public override hook.HookState StateType()
    {
        return hook.HookState.Fish;
    }

    //
    private void SetXYAxisOffset(Vector3 pPosition)
    {
        _xyOffset = new Vector3(pPosition.x - _hook.gameObject.transform.position.x, pPosition.y - _hook.gameObject.transform.position.y, 0);
        _xyOffset.Normalize();
    }

    //
    private void ApplyVelocity()
    {
        _velocity = new Vector3(_xyOffset.x * _speed, Mathf.Min(_xyOffset.y * _speed / 2, -_fallSpeed), 0);
        _hook.gameObject.transform.Translate(_velocity);
    }

    //
    private void DampXVelocity()
    {
        if (_xyOffset.magnitude > 0)
            _xyOffset *= _xOffsetDamping;

        if (_xyOffset.x == 0 && basic.Boat.gameObject.transform.position.x - _hook.gameObject.transform.position.x > 0)
            _hook.gameObject.transform.Translate(new Vector3(basic.Boat.gameObject.transform.position.x - _hook.gameObject.transform.position.x, 0, 0));
    }

    //
    private void shakeCameraOnCollect()
    {
        screenShakeCounter++;
        if (screenShakeCounter >= screenShakeDuration)
        {
            camShaking = false;
            screenShakeCounter = 0;
            //CameraHandler.ResetScreenShake(true);
        }
    }

    //
    public override void OnTriggerEnter(Collider other)
    {
        if (!_hook || !other) return;
        //Reel the hook in if you touch the floor
        if (other.gameObject.CompareTag("Floor"))
        {
            SetState(hook.HookState.Reel);
            GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
        } 
        //On contact with a fish
        if (other.gameObject.CompareTag("Fish"))
        {
            fish theFish = other.gameObject.GetComponent<fish>();
            if (!theFish.Visible) return;

            theFish.SetState(fish.FishState.FollowHook);
            _hook.FishOnHook.Add(theFish);
            basic.Shoppinglist.AddFish(theFish);
            basic.Scorehandler.AddScore(basic.Scorehandler.GetFishScore(theFish.fishType), true, true);
            if (!basic.GlobalUI.InTutorial)
            {
                basic.combo.CheckComboProgress(theFish.fishType);
            }
            if (!basic.Shoppinglist.Introduced)
            {
                basic.Shoppinglist.Show(true);
                basic.Shoppinglist.Introduced = true;
            }
            CameraHandler.CreateShakePoint();
        }
        if (other.gameObject.CompareTag("Jellyfish"))
        {
            Jellyfish theJellyfish = other.gameObject.GetComponent<Jellyfish>();
            //if (!theJellyfish.Visible) return;

            basic.Scorehandler.RemoveScore(basic.Scorehandler.GetJellyfishPenalty(), true);

            CameraHandler.CreateShakePoint();

            SetState(hook.HookState.Reel);
            GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
            //Create a new list maybe
            //Change animation for the fish and state
            //Remove fish from list 
            //Destroy fish
            //Screen shake

            //Remove some fish
        }
        if (other.gameObject.CompareTag("Trash"))
        {
            trash theTrash = other.gameObject.GetComponent<trash>();
            if (!theTrash.Visible) return;

            theTrash.SetState(trash.TrashState.FollowHook);
            _hook.TrashOnHook.Add(theTrash);
            bool firstTime = basic.Scorehandler.CollectATrashPiece();
            basic.GlobalUI.UpdateOceanProgressBar(firstTime);
            CameraHandler.CreateShakePoint();

        }

    }
}
