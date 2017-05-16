using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishTest : MonoBehaviour {
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform spawn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z)) SpawnFish();
	}
    private void SpawnFish()
    {
        Instantiate(prefab, spawn.position, spawn.rotation);
    }
}
