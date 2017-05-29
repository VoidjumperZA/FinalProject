using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlotOfThingsScript : MonoBehaviour {
    [SerializeField] private GameObject _prefab;


    private List<fish> _resolvedFish = new List<fish>();
    public Rigidbody _joint;


    void Start()
    {

    }
	void Update ()
    {
	}
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fish"))
        {
            //collider.gameObject.GetComponent<MeshCollider>().isTrigger = true;
            fish _fish = collider.gameObject.GetComponent<fish>();
            Rigidbody _rb = _fish.GetComponent<Rigidbody>();
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.useGravity = false;
            if (_resolvedFish.Count == 0) _joint.GetComponent<HingeJoint>().connectedBody = _rb;
            else
            {
                //_resolvedFish[_resolvedFish.Count - 1].Joints[0]
            }
            _resolvedFish.Add(_fish);

        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fish"))
        {
            fish _fish = collider.gameObject.GetComponent<fish>();
            Rigidbody _rb = _fish.GetComponent<Rigidbody>();
            Debug.Log("dsadsadsa");
        }
    }
}
