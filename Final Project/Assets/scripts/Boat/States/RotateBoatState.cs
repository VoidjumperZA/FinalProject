using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBoatState : AbstractBoatState
{
    private float _rotationCounter = 0;
    private float _rotationDuration;
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private Quaternion _rightRotation;
    private Quaternion _leftRotation;
    private GameObject _boatModel;

    public RotateBoatState(boat pBoat, float pRotationDuration, GameObject pBoatModel) : base(pBoat)
    {
        _rotationDuration = pRotationDuration;
        _boatModel = pBoatModel;

        // Set Right and Left Rotation
        _rightRotation = _boatModel.transform.rotation;
        _boatModel.transform.Rotate(0.0f, 180, 0.0f);
        _leftRotation = _boatModel.transform.rotation;
        _boatModel.transform.rotation = _rightRotation;
    }
    public override void Start()
    {
        _rotationCounter = 0;
        _startRotation = _boatModel.transform.rotation;
        _endRotation = (_startRotation == _rightRotation) ? _leftRotation : _rightRotation;
    }
    public override void Update()
    {
        _rotationCounter += Time.deltaTime;
        if (_rotationCounter <= _rotationDuration)
        {
            float lerp = _rotationCounter / _rotationDuration;
            _boatModel.transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, lerp);
        }
        else
        {
            _boatModel.transform.rotation = _endRotation;
            SetState(boat.BoatState.Move);
        }
    }
    public override void Refresh()
    {

    }
    public override boat.BoatState StateType()
    {
        return boat.BoatState.Rotate;
    }
}