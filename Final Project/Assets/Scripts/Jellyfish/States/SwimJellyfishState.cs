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
    //private float _distance;

    private Vector3 targetPoint;
    private float _seaDepth;
    //private float distanceToNewPoint;
    private bool positive;

    public GameObject _lerpHelp;

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

        //Path
        _seaDepth = basic.GetSeaDepth();
        //distanceToNewPoint = _seaDepth * -1 / 6;
       // _distance = 0.1f;

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
    public void FixedUpdate()
    {
        turn();
    }


    private void move()
    {

        /*Vector3 _movement = transform.forward * _movementSpeed * Time.deltaTime;
        _jellyrigidbody.MovePosition(transform.position + _movement);*/
        _jellyfish.gameObject.transform.Translate(Vector3.forward * _speed);
        
        if (Vector3.Distance(_jellyfish.gameObject.transform.position, targetPoint) <= _distance)
        {
            createNewPoint();
            
        }
    }
    private void turn()
    {
        _lerpHelp.transform.LookAt(targetPoint);
        _jellyfish.gameObject.transform.rotation = Quaternion.Lerp(_jellyfish.gameObject.transform.rotation, _lerpHelp.transform.rotation, _lerpSpeed);
    }
   
    
    private void createNewPoint()
    {
        bool firstTime = true;

        
        while (firstTime || !(targetPoint.y < _seaDepth / 3 && targetPoint.y > (_seaDepth * 2) / 3 && targetPoint.x))
        {

            float angle = Random.Range(0, Mathf.PI);
            float distanceToNewPoint = Random.Range(_seaDepth * -1 / 12, _seaDepth * -1 / 6);

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
        }

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
