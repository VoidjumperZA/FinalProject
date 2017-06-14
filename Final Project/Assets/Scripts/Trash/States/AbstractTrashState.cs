using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTrashState
{
    protected trash _trash = null;

    public AbstractTrashState(trash pTrash)
    {
        if (!_trash) _trash = pTrash;
    }

    public abstract void Start();

    public abstract void Update();
    public virtual void Refresh()
    {

    }

    public abstract trash.TrashState StateType();
    public virtual void SetState(trash.TrashState pState)
    {
        _trash.SetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
}