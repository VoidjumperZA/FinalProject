using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneTrashState : AbstractTrashState
{

    public NoneTrashState(trash pTrash) : base(pTrash)
    {

    }
    public override void Start()
    {
        SetState(trash.TrashState.Float);
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {

    }
    public override trash.TrashState StateType()
    { 
        return trash.TrashState.None;
    }
}
