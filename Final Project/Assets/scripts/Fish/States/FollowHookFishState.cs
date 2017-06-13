﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHookFishState : AbstractFishState
{
    public FollowHookFishState(fish pFish) : base(pFish)
    {
    }
    public override void Start()
    {
        GameObject.Destroy(_fish.gameObject.GetComponent<Collider>());
        GameManager.Hook.FishOnHook.Add(_fish);

        _fish.Animator.enabled = false;
        //_fish.Animator.SetBool("Death", true);*/
        _fish.ToggleOutliner(false);
        ActivateHingeJoints();


    }
    public override void Update()
    {
        // The Head of fish's skeleton follows the HookTip
        if (_fish.Joints.Length > 0) _fish.Joints[0].gameObject.transform.position = GameManager.Hook.HookTip.position;
    }
    public override void Refresh()
    {

    }
    public override fish.FishState StateType()
    {
        return fish.FishState.FollowHook;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
    private void ActivateHingeJoints()
    {
        if (_fish.Joints.Length > 0)
        {
            foreach (GameObject joint in _fish.Joints)
            {
                joint.GetComponent<Rigidbody>().isKinematic = false;
            }
            _fish.Joints[0].transform.rotation = Quaternion.LookRotation(Vector3.right, -Vector3.forward);
        }
    }
}
