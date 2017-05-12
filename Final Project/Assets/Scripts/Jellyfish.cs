using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : general
{

    //Movement
    [SerializeField] private float _speed;
    //[SerializeField] private float _maxSteerAngle; //45f

    private Rigidbody _jellyrigidbody;

    //Distance from the jellyfish to the point it is going to.
    private float _distance;

    private Vector3 targetPoint;
    private float _seaDepth;
    private float distanceToNewPoint;
    private bool positive;

    public GameObject target;
    //Score
    [SerializeField] private int _penalization;

    // Radar related
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private cakeslice.Outline _outliner;


    // Use this for initialization
    public override void Start ()
    {
        _jellyrigidbody = GetComponent<Rigidbody>();
        
        _seaDepth = basic.GetSeaDepth();
        //Debug.Log("Seadepth: " + _seaDepth);
        distanceToNewPoint = _seaDepth * -1 / 6;
        _distance = 0.1f;

        //Llamar a generarPunto()
        targetPoint = transform.position;
        positive = true;
        createNewPoint();
}
	
	// Update is called once per frame
	public override void Update ()
    {

    }

    public void FixedUpdate()
    {
        move();
        turn();
    }
    

    private void move()
    {

        Vector3 _movement = transform.forward * _speed * Time.deltaTime;
        _jellyrigidbody.MovePosition(transform.position + _movement);
        if (Vector3.Distance(transform.position, targetPoint) <= _distance)
        {
            createNewPoint();
        }
    }

    private void turn()
    {
        Transform trans = gameObject.transform;
        trans.LookAt(target.transform);
        Quaternion.Lerp(gameObject.transform.rotation, trans.rotation, 0.01f);

        /*Vector3 previousRotation = transform.eulerAngles;

        Vector3 auxPosition = new Vector3(targetPoint.x, targetPoint.y, transform.position.z);
        transform.LookAt(auxPosition);


        Vector3 targetRotation = transform.eulerAngles;

        Vector3 currentRotation = new Vector3(Mathf.LerpAngle(previousRotation.x, targetRotation.x, Time.deltaTime * 3), previousRotation.y, previousRotation.z );

        transform.eulerAngles = currentRotation;*/

    }

    private void createNewPoint()
    {
        bool firstTime = true;
        //float x = 0f;
        //float y = 0f;

        while (firstTime || !(targetPoint.y < _seaDepth/3 && targetPoint.y > (_seaDepth * 2) / 3 ) )
        { 

            float angle = Random.Range( 0, Mathf.PI);

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
        

        target.transform.position = targetPoint;

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Fish Despawner" || other.gameObject.tag == "Floor")
        {
            basic.Generals.Remove(this);
            GameObject.Destroy(gameObject);
        }
    }
    /*
    public override void ToggleOutliner(bool pBool)
    {
        _outliner.enabled = pBool;
    }
    public override void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
        _renderer.enabled = pBool;
    }*/
}
