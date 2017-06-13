using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {
    [SerializeField] private GameObject[] _fishPrefabs;
    [SerializeField] private int _fishCapacity;
    private int _fishAlive;

    [SerializeField] private float _spawnCooldown;
    private float _spawnCounter = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_fishAlive < _fishCapacity)
        {
            _spawnCounter += Time.deltaTime;
            if (_spawnCounter >= _spawnCooldown)
            {
                _fishAlive += 1;
                _spawnCounter = 0;
                Instantiate(_fishPrefabs[Random.Range(0, _fishPrefabs.Length)], gameObject.transform.position + new Vector3(0, Random.Range(-2, 5), 0), gameObject.transform.rotation);
            }
        }
	}
}
