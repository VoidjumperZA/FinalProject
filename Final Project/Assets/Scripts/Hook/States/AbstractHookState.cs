using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractHookState
{
    protected hook _hook = null;

    public AbstractHookState(hook pHook)
    {
        if (!_hook) _hook = pHook;
    }

    public abstract void Start();

    public abstract void Update();
    public virtual void Refresh()
    {

    }

    public abstract hook.HookState StateType();
    public virtual void SetState(hook.HookState pState)
    {
        _hook.SetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }

}