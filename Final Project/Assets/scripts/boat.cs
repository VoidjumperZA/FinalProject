using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boat : MonoBehaviour {
    [SerializeField]
    private float _slideSpeed;
    private Vector3 _destination;
	// Use this for initialization
	void Start () {
        _destination = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if ((_destination - gameObject.transform.position).magnitude > _slideSpeed)
        {
            gameObject.transform.Translate((_destination - gameObject.transform.position).normalized * _slideSpeed);
        }
	}

    public void SetDestination(Vector3 pDestination)
    {
        _destination = pDestination;
    }
}
