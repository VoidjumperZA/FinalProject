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

    private static float _jellyfishZoneLeft;
    private static float _jellyfishZoneRight;
    private static float _jellyfishZoneUp;
    private static float _jellyfishZoneDown;

    // Use this for initialization
    void Start ()
    {
        _numJellyfish = 0;
        _spawnCounter = 0;
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!basic.GlobalUI.InTutorial)
        {
            //Debug.Log("Hello jellyfish");
            if(_numJellyfish < _maxNumJellyfish)
            {
                _spawnCounter += Time.deltaTime;

                if(_spawnCounter >= _spawnCooldown)
                {
                    _numJellyfish += 1;
                    _spawnCounter = 0;
               
                    Jellyfish theJellyFish = Instantiate(_jellyfishPrefab, new Vector3(Random.Range(_jellyfishZoneLeft, _jellyfishZoneRight), Random.Range(_jellyfishZoneDown, _jellyfishZoneUp), 0), gameObject.transform.rotation).GetComponent<Jellyfish>();
                    basic.AddCollectable(theJellyFish);
                }
            }
        }

    }

    public static void setJellyfishZoneLeft(float leftBoundarie)
    {
        _jellyfishZoneLeft = leftBoundarie;
    }

    public static void setJellyfishZoneRight(float rightBoundarie)
    {
        _jellyfishZoneRight = rightBoundarie;
    }

    public static void setJellyfishZoneUp(float upBoundarie)
    {
        _jellyfishZoneUp = upBoundarie;
    }

    public static void setJellyfishZoneDown(float downBoundarie)
    {
        _jellyfishZoneDown = downBoundarie;
    }
}
