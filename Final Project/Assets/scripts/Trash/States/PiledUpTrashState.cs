using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiledUpTrashState : AbstractTrashState
{
    public PiledUpTrashState(trash pTrash) : base(pTrash)
    {
    }
    public override void Start()
    {
        basic.Generals.Remove(_trash);
        basic.Trailer.StuffOnTrailer.Add(_trash);
    }
    public override void Update()
    {

    }
    public override void Refresh()
    {

    }
    public override trash.TrashState StateType()
    {
        return trash.TrashState.PiledUp;
    }
    public override void OnTriggerEnter(Collider other)
    {

    }
}
