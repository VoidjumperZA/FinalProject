using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFishState
{
    protected fish _fish = null;

    public AbstractFishState(fish pFish)
    {
        if (!_fish) _fish = pFish;
    }

    public abstract void Start();

    public abstract void Update();
    public virtual void Refresh()
    {

    }

    public abstract fish.FishState StateType();
    public virtual void SetState(fish.FishState pState)
    {
        _fish.SetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
}