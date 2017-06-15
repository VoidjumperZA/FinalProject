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
    public virtual AbstractBoatState GetState(boat.BoatState pState)
    {
        return _boat.GetState(pState);
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
    public virtual void SetDestination(Vector3 pDestination)
    {

    }
    public virtual void SetVelocity(float pVelocity)
    {

    }
    public virtual void LeftOrRight(bool pLeftTrueRightFalse)
    {

    }
    public virtual void SetBoundaries(Vector3[] pBoundaries)
    {

    }
    public virtual void FinalizeInitialization()
    {

    }
}
