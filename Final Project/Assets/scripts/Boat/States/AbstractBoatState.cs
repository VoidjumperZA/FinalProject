using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBoatState {
    protected boat _boat = null;

    public AbstractBoatState(boat pBoat)
    {
        if (!_boat) _boat = pBoat;
    }

	public abstract void Start();

    public abstract void Update();
    public virtual void Refresh()
    {

    }

    public abstract boat.BoatState StateType();
    public virtual void SetState(boat.BoatState pState)
    {
        _boat.SetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }


}
