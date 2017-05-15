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
        basic.Generals.Remove(_fish);
        basic.Trailer.StuffOnTrailer.Add(_fish);
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
