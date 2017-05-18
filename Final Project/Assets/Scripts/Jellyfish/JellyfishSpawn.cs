using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishSpawn : MonoBehaviour {

    [SerializeField]
    private GameObject _jellyfishPrefab;
    [SerializeField]
    private int _maxNumJellyfish;
    private int _numJellyfish;

    [SerializeField]
    private float _spawnCooldown;
    private float _spawnCounter;

    private float _jellyfishZoneLeft;
    private float _jellyfishZoneRight;
    private float _jellyfishZoneUp;
    private float _jellyfishZoneDown;

    // Use this for initialization
    void Start ()
    {
        _numJellyfish = 0;
        _spawnCounter = 0;

        _jellyfishZoneUp = basic.GetJellyfishZoneUp();
        _jellyfishZoneDown = basic.GetJellyfishZoneDown();
        _jellyfishZoneLeft = basic.GetJellyfishZoneLeft();
        _jellyfishZoneRight = basic.GetJellyfishZoneRight();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(_numJellyfish < _maxNumJellyfish)
        {
            _spawnCounter += Time.deltaTime;

            if(_spawnCounter >= _spawnCooldown)
            {
                _numJellyfish += 1;
                _spawnCounter = 0;
                Instantiate(_jellyfishPrefab, new Vector3(Random.Range(_jellyfishZoneLeft, _jellyfishZoneRight), Random.Range(_jellyfishZoneDown, _jellyfishZoneUp), 0), gameObject.transform.rotation);
            }
        }
        

    }
}
