using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHookTrashState : AbstractTrashState
{
    public FollowHookTrashState(trash pTrash) : base(pTrash)
    {
    }
    public override void Start()
    {
       // _trash.ToggleOutliner(false);
        _trash.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    public override void Update()
    {
        _trash.gameObject.transform.position = basic.Hook.HookTip.position;
    }
    public override void Refresh()
    {

    }
    public override trash.TrashState StateType()
    {
        return trash.TrashState.FollowHook;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
