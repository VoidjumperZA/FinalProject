using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractJellyfishState
{
    protected Jellyfish _jellyfish = null;

    public AbstractJellyfishState(Jellyfish pJellyfish)
    {
        if (!_jellyfish) _jellyfish = pJellyfish;
    }

    public abstract void Start();

    public abstract void Update();
    public virtual void FixedUpdate() { }
    public virtual void Refresh(){ }

    public abstract Jellyfish.JellyfishState StateType();
    public virtual void SetState(Jellyfish.JellyfishState pState)
    {
        _jellyfish.SetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
    
}
