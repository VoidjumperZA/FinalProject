using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSharkState
{
    protected shark _shark = null;

    public AbstractSharkState(shark pShark)
    {
        if (!_shark) _shark = pShark;
    }
    public abstract void Start();
    public abstract void Update();
    public virtual void Refresh()
    {

    }
    public abstract shark.SharkState StateType();
    public virtual void SetState(shark.SharkState pState)
    {
        _shark.SetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
}
