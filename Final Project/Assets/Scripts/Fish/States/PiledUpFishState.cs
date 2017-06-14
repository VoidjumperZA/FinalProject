using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiledUpFishState : AbstractFishState
{
    public PiledUpFishState(fish pFish) : base(pFish)
    {
    }
    public override void Start()
    {
        //basic.Trailer.Add(_fish.GetFishType());
        //Debug.Log("Removed: " + _fish.GetFishType());
        // Now objects are being destroyed but need to be replaced with low poly model / other model on the boat.
        //basic.Trailer.AddFish();
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {

    }
    public override fish.FishState StateType()
    {
        return fish.FishState.PiledUp;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
