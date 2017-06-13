using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookState : AbstractHookState
{
    private float _fallSpeed;
    private Vector2 _prevInputVelocity;
    private Vector3 _totalVelocity;
    private Vector3 _inputDirection;
    private float _sideSpeed;
    private float _downSpeed;

    public FishHookState(hook pHook, float pSideSpeed, float pDownSpeed, float pFallSpeed) : base(pHook)
    {
        _sideSpeed = pSideSpeed;
        _downSpeed = pDownSpeed;
        _fallSpeed = pFallSpeed;
    }
    public override void Start()
    {
        GameManager.Camerahandler.SetViewPoint(CameraHandler.FocusPoint.Hook);
    }
    public override void Update()
    {
        if ((_hook.transform.position - GameManager.Boat.transform.position).magnitude < 10)
        {
            _hook.transform.Translate(-Vector3.up * _downSpeed);
        } 
        else
        {
            ApplyVelocity(-_fallSpeed, mouse.Touching());
        }
    }
    //
    public override void Refresh()
    {
        _inputDirection = Vector2.zero;
        _totalVelocity = Vector3.zero;
    }

    //
    public override hook.HookState StateType()
    {
        return hook.HookState.Fish;
    }
    //
    private void SetInputDirection(Vector3 pPosition)
    {
        _inputDirection = new Vector3(pPosition.x - _hook.HookTip.position.x, pPosition.y - _hook.HookTip.position.y, 0);
        _inputDirection.Normalize();
    }

    //
    private void ApplyVelocity(float pFallSpeed, bool pSteering)
    {
        if (pSteering) SetInputDirection(mouse.GetWorldPoint());
        Vector3 acceleration = new Vector3(_inputDirection.x * _sideSpeed, _inputDirection.y * _downSpeed, 0);

        _totalVelocity.x = (pSteering) ? acceleration.x : _totalVelocity.x * 0.9f;
        _totalVelocity.y = (pFallSpeed > acceleration.y) ? acceleration.y : pFallSpeed;
        _totalVelocity.z = acceleration.z;
        _hook.gameObject.transform.Translate(_totalVelocity);
        _inputDirection = Vector3.zero;
    }

    //
    public override void OnTriggerEnter(Collider other)
    {
        if (!_hook || !other) return;
        //Reel the hook in if you touch the floor
        if (other.gameObject.CompareTag("Floor"))
        {
            //The game time is out before this condition can be true, I am going to leave it here just in case
            /*if (basic.GlobalUI.InTutorial)
            {
                basic.GlobalUI.ShowHandSwipe(false);
                basic.GlobalUI.SwipehandCompleted = true;
            }*/
            SetState(hook.HookState.Reel);
            //basic.combo.ClearPreviousCombo(false);
			//GameObject.Instantiate (basic.HookHit, _hook.HookTip.position, Quaternion.identity);
        } 
        //On contact with a fish
        if (other.gameObject.CompareTag("Fish"))
        {
            fish theFish = other.gameObject.GetComponent<fish>();
            if (!theFish || !theFish.Visible) return;
            theFish.SetState(fish.FishState.FollowHook);
            GameManager.ShopList.CollectFish((int)theFish.GetFishType());
            GameManager.Scorehandler.AddScore(theFish.GetFishType(), true, true);
            /*if (!basic.GlobalUI.InTutorial)
            {
                basic.combo.CheckComboProgress(theFish.fishType);
            }
            if (!basic.Shoppinglist.Introduced)
            {
                basic.Shoppinglist.Show(true);
                basic.Shoppinglist.Introduced = true;
            }*/
            //basic.Camerahandler.CreateShakePoint();
        }
        if (other.gameObject.CompareTag("Jellyfish"))
        {
            Jellyfish theJellyfish = other.gameObject.GetComponent<Jellyfish>();
            if (!theJellyfish) return;
			_hook.EnableJellyAttackEffect ();
            GameManager.Scorehandler.RemoveScore(true);

           // basic.Camerahandler.CreateShakePoint();

            SetState(hook.HookState.Reel);
            //basic.combo.ClearPreviousCombo(false);
            //Create a new list maybe
            //Change animation for the fish and state
            //Remove fish from list 
            //Destroy fish

        }
        if (other.gameObject.CompareTag("Trash"))
        {
            trash theTrash = other.gameObject.GetComponent<trash>();
            if (!theTrash || !theTrash.Visible) return;

            theTrash.SetState(trash.TrashState.FollowHook);
            //_hook.TrashOnHook.Add(theTrash);

            //bool firstTime = basic.Scorehandler.CollectATrashPiece();
            //basic.GlobalUI.UpdateOceanProgressBar(firstTime);
            //basic.Camerahandler.CreateShakePoint();

            //The game time is out before this condition can be true, I am going to leave it here just in case
            /*if (basic.GlobalUI.InTutorial)
            {
                basic.GlobalUI.ShowHandSwipe(false);
                basic.GlobalUI.SwipehandCompleted = true;
            }*/
            SetState(hook.HookState.Reel);
           
            //basic.combo.ClearPreviousCombo(false);

        }

    }
}
