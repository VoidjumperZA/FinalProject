using System;
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
        
        basic.Tempfishspawn.RemoveOneFishFromTracked();
        _fish.Animator.enabled = false;
        _fish.Animator.SetBool("Death", true);
        _fish.ToggleOutliner(false);
        _fish.gameObject.GetComponent<BoxCollider>().enabled = false;
        HandleJointsRigidBodies();


    }
    public override void Update()
    {
        //_fish.RagdollJoints[4].transform.position = basic.Hook.transform.position;
        if (_fish._head.Length > 0) _fish._head[0].gameObject.transform.position = basic.Hook.transform.position;
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
    private void HandleJointsRigidBodies()
    {
        if (_fish._head.Length > 0)
        {
            foreach (GameObject obj in _fish._head)
            {
                obj.GetComponent<Rigidbody>().isKinematic = false;
            }
            _fish._head[0].transform.rotation = Quaternion.LookRotation(Vector3.right, -Vector3.forward);
        }
    }
}
