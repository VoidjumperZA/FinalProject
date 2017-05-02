using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hook : MonoBehaviour {

    [SerializeField]
    private float _slideSpeed;
    private Vector3 _destination;
    // Use this for initialization
    void Start()
    {
        _destination = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (_destination != Vector3.zero && (_destination - gameObject.transform.position).magnitude > _slideSpeed)
        {
            gameObject.transform.Translate((_destination - gameObject.transform.position).normalized * _slideSpeed);
        }
    }

    public void SetDestination(Vector3 pDestination)
    {
        _destination = pDestination;
    }

    public void Appear()
    {
        gameObject.transform.localPosition = new Vector3(0, -1, 0);
    }
    public void Disappear()
    {
        gameObject.transform.localPosition = Vector3.zero;
    }
}
