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
        _fish.Animator.SetBool("Death", true);
        _fish.ToggleOutliner(false);
        _fish.gameObject.GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("Manager").GetComponent<TempFishSpawn>().RemoveOneFishFromTracked();
        //for (int i = 0; i < _fish.RagdollJoints.Length; i++)
        //{
        // if (i != 4)
        // {
        //  _fish.RagdollJoints[i].GetComponent<Rigidbody>().useGravity = true;
        //   _fish.RagdollJoints[i].GetComponent<BoxCollider>().enabled = true;
        // }
        //  }
        // _fish.gameObject.GetComponent<Animator>().speed = 0;




    }
    public override void Update()
    {
        //_fish.RagdollJoints[4].transform.position = basic.Hook.transform.position;
        _fish.gameObject.transform.position = basic.Hook.transform.position;
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
}
