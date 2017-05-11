using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : general
{

    //Movement
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSteerAngle; //45f

    [SerializeField] private Rigidbody _jellyrigidbody;

    //Distance from the jellyfish to the point it is going to.
    [SerializeField] private float _distance;

    private Transform targetPoint;
    private float seaDepth;
    
    //Score
    [SerializeField] private int _penalization;

    // Radar related
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private cakeslice.Outline _outliner;


    // Use this for initialization
    public override void Start ()
    {
        _jellyrigidbody = GetComponent<Rigidbody>();

        /*seaDepth = basic.getSeaDepth();*/
        //Llamar a generarPunto()
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

        _jellyrigidbody.MovePosition(_jellyrigidbody.position + _movement);
    }

    private void turn()
    {

        if (Vector3.Distance(transform.position, targetPoint.position) <= _distance)
        {
            //Llamar a generarPunto()

        }

        Vector3 previousRotation = transform.eulerAngles;

    }

    private Transform createNewPoint()
    {

        float newlenght = Random.Range(seaDepth/2 , seaDepth);
        float angle;



    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Fish Despawner" || other.gameObject.tag == "Floor")
        {
            basic.Generals.Remove(this);
            GameObject.Destroy(gameObject);
        }
    }

    public override void ToggleOutliner(bool pBool)
    {
        _outliner.enabled = pBool;
    }
    public override void ToggleRenderer(bool pBool)
    {
        Visible = pBool;
        _renderer.enabled = pBool;
    }
}
