using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneFishState : AbstractFishState {

    public NoneFishState(fish pFish) : base(pFish)
    {

    }
    public override void Start()
    {
        Debug.Log("Swim!");
        SetState(fish.FishState.Swim);
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {

    }
    public override fish.FishState StateType()
    {
        return fish.FishState.None;
    }
}
