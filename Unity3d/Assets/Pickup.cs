using System;
using UnityEngine;




public class Pickup : MonoBehaviour {
	public GameObject Door;
	private bool HasKey;

	// Use this for initialization
	void Start () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag ("key")) {
			HasKey = true;
			Door.GetComponent<BoxCollider> ().isTrigger = true;
			Destroy (other.gameObject);
		}

		if (other.CompareTag ("door") && HasKey) {
			// magic goes here
			Destroy(other.gameObject);
		}

    }
    // Update is called once per frame
    void Update () {
		
	}
}
