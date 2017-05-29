using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookState : AbstractHookState
{
    private float _dragSpeed;
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
        _dragSpeed = pSpeed;
        _xOffsetDamping = pOffsetDamping;
        _fallSpeed = pFallSpeed;
    }

    //
    public override void Start()
    {

        hookRotationAmount = 1.0f;
        currentHookRotation = 0.0f;
        maxHookRotation = 25.0f;
        basic.Camerahandler.SetViewPoint(CameraHandler.CameraFocus.Hook);
    }

    //
    public override void Update()
    {
        if ((_hook.transform.position - basic.Boat.transform.position).magnitude < 10) ApplyVelocity(_dragSpeed); 
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (basic.GlobalUI.InTutorial)
                {
                    basic.GlobalUI.ShowHandSwipe(false);
                    basic.GlobalUI.SwipehandCompleted = true;
                }
                SetXYAxisOffset(mouse.GetWorldPoint());
            }
            ApplyVelocity(_fallSpeed);
            DampVelocityOffset();
        }

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
    private void ApplyVelocity(float pFallSpeed)
    {
        _velocity = new Vector3(_xyOffset.x * _dragSpeed, Mathf.Min(_xyOffset.y * _dragSpeed / 2, -pFallSpeed), 0);
        _hook.gameObject.transform.Translate(_velocity);
    }

    //
    private void DampVelocityOffset()
    {
        if (_xyOffset.magnitude > 0)
            _xyOffset *= _xOffsetDamping;
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
            //The game time is out before this condition can be true, I am going to leave it here just in case
            if (basic.GlobalUI.InTutorial)
            {
                basic.GlobalUI.ShowHandSwipe(false);
                basic.GlobalUI.SwipehandCompleted = true;
            }
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
            basic.Camerahandler.CreateShakePoint();
        }
        if (other.gameObject.CompareTag("Jellyfish"))
        {
            Jellyfish theJellyfish = other.gameObject.GetComponent<Jellyfish>();
            //if (!theJellyfish.Visible) return;

            basic.Scorehandler.RemoveScore(basic.Scorehandler.GetJellyfishPenalty(), true);

            basic.Camerahandler.CreateShakePoint();

            SetState(hook.HookState.Reel);
            GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);
            //Create a new list maybe
            //Change animation for the fish and state
            //Remove fish from list 
            //Destroy fish

        }
        if (other.gameObject.CompareTag("Trash"))
        {
            trash theTrash = other.gameObject.GetComponent<trash>();
            if (!theTrash.Visible) return;

            theTrash.SetState(trash.TrashState.FollowHook);
            _hook.TrashOnHook.Add(theTrash);
            bool firstTime = basic.Scorehandler.CollectATrashPiece();
            basic.GlobalUI.UpdateOceanProgressBar(firstTime);
            basic.Camerahandler.CreateShakePoint();

            //The game time is out before this condition can be true, I am going to leave it here just in case
            if (basic.GlobalUI.InTutorial)
            {
                basic.GlobalUI.ShowHandSwipe(false);
                basic.GlobalUI.SwipehandCompleted = true;
            }
            SetState(hook.HookState.Reel);
           
            GameObject.Find("Manager").GetComponent<Combo>().ClearPreviousCombo(false);

        }

    }
}
