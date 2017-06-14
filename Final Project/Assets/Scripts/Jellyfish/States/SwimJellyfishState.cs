using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimJellyfishState : AbstractJellyfishState
{

    private counter _outlineCounter;

    //Movement
    private float _speed;
    private float _lerpSpeed;

    //Target point related.
    private float _distance;
    private Vector3 _targetPoint;

    //Variables for checking that the target point is inside the area 
    private float _leftBound;
    private float _rightBound;
    private float _upperBound;
    private float _lowerBound;
    private float _jellyfishZoneSizeY;
    
    public SwimJellyfishState(Jellyfish pJellyfish, float pSpeed, float pLerpSpeed, float pRevealDuration) : base(pJellyfish)
    {
        _speed = pSpeed;
        _lerpSpeed = pLerpSpeed;
        _outlineCounter = new counter(pRevealDuration);
    }

    // Use this for initialization
    public override void Start()
    {
        //Debug.Log("Jellyfih created");
        _outlineCounter.Reset();

        //Getting the position and size of the zone where the jellyfish can move
        /*_leftBound = GameManager.JellyFishSpawner.LBound;
        _rightBound = GameManager.JellyFishSpawner.RBound;
        _upperBound = GameManager.JellyFishSpawner.UBound;
        _lowerBound = GameManager.JellyFishSpawner.DBound;*/
        /*_jellyfishZoneUp = basic.GetJellyfishZoneUp();
        _jellyfishZoneDown = basic.GetJellyfishZoneDown();
        _jellyfishZoneLeft = basic.GetJellyfishZoneLeft();
        _jellyfishZoneRight = basic.GetJellyfishZoneRight();
        _jellyfishZoneSizeY = basic.GetJellyfishZoneSizeY();*/

        _distance = 1.5f;
        
        //Creates a target from the position of the jellyfish 
        _targetPoint = _jellyfish.gameObject.transform.position;
        createNewPoint('u');
    }

    // Update is called once per frame
    public override void Update()
    {
        move();

        if (_jellyfish.Revealed) HandleOutline();
    }
    
    private void move()
    {
        //Rotate to look at the target
        _jellyfish.gameObject.transform.rotation = Quaternion.Slerp(_jellyfish.gameObject.transform.rotation, Quaternion.LookRotation(_targetPoint - _jellyfish.gameObject.transform.position), _lerpSpeed * Time.deltaTime);

        //Move towards target
        _jellyfish.gameObject.transform.position += _jellyfish.gameObject.transform.forward * Time.deltaTime * _speed;
        
        //Checking if the jellyfish reached the target point
        if (Vector3.Distance(_jellyfish.gameObject.transform.position, _targetPoint) <= _distance)
        {   
            createNewPoint('u');  
        }
    }
  
    private void createNewPoint(char side)
    {
        //Debug.Log("Enter create point");
        bool firstTime = true;
        float angle;
        switch (side)
        {
            case 'l':
                angle = Random.Range(0, Mathf.PI);
                if (angle > Mathf.PI / 2) angle += Mathf.PI;
                //Debug.Log("Angle l: " + angle);
                break;

            case 'r':
                angle = Random.Range(Mathf.PI / 2, Mathf.PI * 3 / 2);
               // Debug.Log("Angle r: " + angle);
                break;

            case 'u':
                angle = Random.Range(0, Mathf.PI);
                angle *= -1;
                //Debug.Log("Angle u: " + angle);
                break;

            case 'd':
                angle = Random.Range(0, Mathf.PI);
                //Debug.Log("Angle d: " + angle);
                break;

            default:
                Debug.Log("Please call createNewPoint function with one of the following values: l (left), r (right), d (down), u (up)");
                angle = Random.Range(0, Mathf.PI);
                break;
        }

        //Calculate new point
        //Debug.Log("New point");
        float distanceToNewPoint = 5; //Random.Range(_jellyfishZoneSizeY/2,_jellyfishZoneSizeY);
        
        float x = _targetPoint.x + Mathf.Cos(angle) * distanceToNewPoint;
        float y = _targetPoint.y + Mathf.Sin(angle) * distanceToNewPoint;

        _targetPoint = new Vector3(x, y, 0);

        //Point for visualizing
        //_jellyfish._point.transform.position = _targetPoint;

        //Check if point is inside zone
        if (_targetPoint.x >  _rightBound)
        {
            createNewPoint('r');
           // Debug.Log("Call create new point from x >: Targetpointx = " + _targetPoint.x);
        }
        if(_targetPoint.x < _leftBound)
        {
            createNewPoint('l');
            //Debug.Log("Call create new point from x <: Targetpointx = " + _targetPoint.x);
        }
        if(_targetPoint.y > _upperBound)
        {
            createNewPoint('u');
           // Debug.Log("Call create new point from Y >: TargetpointY = " + _targetPoint.y);
        }
        if (_targetPoint.y < _lowerBound)
        {
            createNewPoint('d');
            //Debug.Log("Call create new point from Y <: TargetpointY = " + _targetPoint.y);
        }

        //Debug.Log("Exit ");

    }


    public override void Refresh()
    {

    }
    public override Jellyfish.JellyfishState StateType()
    {
        return Jellyfish.JellyfishState.Swim;
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            basic.RemoveCollectable(_jellyfish);
            GameObject.Destroy(_jellyfish.gameObject);
        }
    }
    private void HandleOutline()
    {
        _outlineCounter.Count();
        //if (_outlineCounter.Remaining(0.33f)) _blink = true;
        if (_outlineCounter.Done())
        {
            _jellyfish.Hide();
            //_blink = false;
            _outlineCounter.Reset();
        }
    }
}
