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
        _fish.ToggleOutliner(false);
        _fish.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    public override void Update()
    {
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
