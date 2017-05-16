using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimJellyfishState : AbstractJellyfishState
{

    private counter _outlineCounter;

    //Movement
    private float _speed;
    private float _lerpSpeed;

    //Distance from the jellyfish to the point it is going to.
    private float _distance;

    private Vector3 targetPoint;
    //private float _seaDepth;
    //private float _seaWidth;
    //private float distanceToNewPoint;
    private bool positive;

    //Variables for checking that the target point is inside the area 
    float jellyfishZoneX;
    float jellyfishZoneXPos;
    float jellyfishZoneY;
    float jellyfishZoneYPos;
    


    private float _zoneX;
    private float _zoneY;

    public SwimJellyfishState(Jellyfish pJellyfish, float pSpeed, float pLerpSpeed, float pRevealDuration) : base(pJellyfish)
    {
        _speed = pSpeed;
        _lerpSpeed = pLerpSpeed;
        _outlineCounter = new counter(pRevealDuration);
    }
    // Use this for initialization
    public override void Start()
    {
        _outlineCounter.Reset();

        jellyfishZoneX = _jellyfish._jellyfishZone.transform.localScale.x / 2;
        jellyfishZoneXPos = _jellyfish._jellyfishZone.transform.position.x;
        jellyfishZoneY = _jellyfish._jellyfishZone.transform.localScale.y / 2;
        jellyfishZoneYPos = _jellyfish._jellyfishZone.transform.position.y;
        


        //_seaDepth = basic.GetSeaDepth();
        //_seaWidth = basic.GetSeaWidth();
        //Debug.Log("SeaWidth: " + _seaWidth);
        //distanceToNewPoint = _seaDepth * -1 / 6;
        _distance = 1.5f;

        //Creates a target from the position of the jellyfish 
        targetPoint = _jellyfish.gameObject.transform.position;
        positive = true;
        createNewPoint();
    }

    // Update is called once per frame
    public override void Update()
    {
        move();
        if (_jellyfish.Revealed) HandleOutline();
    }
    /*public override void FixedUpdate()
    {
        //turn();
    }*/


    private void move()
    {
        //Rotate to look at the target
        _jellyfish.gameObject.transform.rotation = Quaternion.Slerp(_jellyfish.gameObject.transform.rotation, Quaternion.LookRotation(targetPoint - _jellyfish.gameObject.transform.position), _lerpSpeed * Time.deltaTime);

        //Move towards target
        _jellyfish.gameObject.transform.position += _jellyfish.gameObject.transform.forward * Time.deltaTime * _speed;

        /*Vector3 _movement = transform.forward * _movementSpeed * Time.deltaTime;
        _jellyrigidbody.MovePosition(transform.position + _movement);*/

        //_jellyfish.gameObject.transform.Translate(Vector3.forward * _speed);
        //Debug.Log("Distance: " + Vector3.Distance(_jellyfish.gameObject.transform.position, targetPoint));
        if (Vector3.Distance(_jellyfish.gameObject.transform.position, targetPoint) <= _distance)
        {
            
            createNewPoint();
            
        }
    }
    private void turn()
    {
        _jellyfish._lerpHelp.transform.LookAt(targetPoint);
        _jellyfish.gameObject.transform.rotation = Quaternion.Lerp(_jellyfish.gameObject.transform.rotation, _jellyfish._lerpHelp.transform.rotation, _lerpSpeed);
    }
   
    
    private void createNewPoint()
    {
        bool firstTime = true;
   
        //Calculate new point
        float angle = Random.Range(0, Mathf.PI);
        float distanceToNewPoint = Random.Range(jellyfishZoneY/2,jellyfishZoneY);

        if (positive)
        {
            angle *= -1;
            positive = false;
        }
        else
        {
            positive = true;
        }

        float x = targetPoint.x + Mathf.Cos(angle) * distanceToNewPoint;
        float y = targetPoint.y + Mathf.Sin(angle) * distanceToNewPoint;

        targetPoint = new Vector3(x, y, 0);

        //Point for visualizing
        _jellyfish._point.transform.position = targetPoint;

        //Check if point is inside zone
        if (targetPoint.x >  jellyfishZoneXPos + jellyfishZoneX)
        {
            createNewPoint();
            Debug.Log("Call create new point from x >: Targetpointx = " + targetPoint.x);
        }
        if(targetPoint.x < jellyfishZoneXPos - jellyfishZoneX)
        {
            createNewPoint();
            Debug.Log("Call create new point from x <: Targetpointx = " + targetPoint.x);
        }
        if(targetPoint.y > jellyfishZoneYPos + jellyfishZoneY)
        {
            createNewPoint();
            Debug.Log("Call create new point from Y >: TargetpointY = " + targetPoint.y);
        }
        if (targetPoint.y < jellyfishZoneYPos - jellyfishZoneY)
        {
            createNewPoint();
            Debug.Log("Call create new point from Y <: TargetpointY = " + targetPoint.y);
        }

        /*
        while (firstTime || !(targetPoint.y < _seaDepth / 3 && targetPoint.y > (_seaDepth * 2) / 3))
        {

            float angle = Random.Range(0, Mathf.PI);
            //float distanceToNewPoint = 3;
            //float distanceToNewPoint = Random.Range(_seaDepth * -1 / 12, _seaDepth * -1 / 6);

            if (positive)
            {
                angle *= -1;
                positive = false;
            }
            else
            {
                positive = true;
            }

            float x = targetPoint.x + Mathf.Cos(angle) * distanceToNewPoint;
            float y = targetPoint.y + Mathf.Sin(angle) * distanceToNewPoint;

            firstTime = false;
            targetPoint = new Vector3(x, y, 0);
            _jellyfish._point.transform.position = targetPoint;
        }*/

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
            basic.Generals.Remove(_jellyfish);
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
