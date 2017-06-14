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
        basic.RemoveCollectable(_trash);
        GameObject.Destroy(_trash.gameObject);
        // Now objects are being destroyed but need to be replaced with low poly model / other model on the boat.
        //basic.Trailer.StuffOnTrailer.Add(_trash);
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
