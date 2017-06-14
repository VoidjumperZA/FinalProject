using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishSpawn : MonoBehaviour {

    [SerializeField]
    private GameObject _jellyFishPrefab;
    [SerializeField]
    private int _maxNumJellyfish;
    private int _numJellyfish;

    [SerializeField]
    private float _spawnCooldown;
    private float _spawnCounter;

    [SerializeField] private Transform _leftSpawner;
    [SerializeField] private Transform _rightSpawner;
    [HideInInspector] public float LBound { get { return _leftSpawner.position.x; } }
    [HideInInspector] public float RBound { get { return _rightSpawner.position.x; } }
    [HideInInspector] public float UBound { get { return _rightSpawner.position.y + _rightSpawner.lossyScale.y / 2; } }
    [HideInInspector] public float DBound { get { return _rightSpawner.position.y - _rightSpawner.lossyScale.y / 2; } }

    public static List<Jellyfish> SpawnedJellyFish = new List<Jellyfish>();

    // Use this for initialization
    void Start ()
    {
        _numJellyfish = 0;
        _spawnCounter = 0;
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //if (!basic.GlobalUI.InTutorial)
        //{
            //Debug.Log("Hello jellyfish");
            /*if(_numJellyfish < _maxNumJellyfish)
            {
                _spawnCounter += Time.deltaTime;

                if(_spawnCounter >= _spawnCooldown)
                {
                    _numJellyfish += 1;
                    _spawnCounter = 0;
                    Vector3 spawnPos = new Vector3(Random.Range(_leftSpawner.position.x, _rightSpawner.position.x), 
                                                   Random.Range(_rightSpawner.position.y + _rightSpawner.lossyScale.y/2, _rightSpawner.position.y - _rightSpawner.lossyScale.y/2), 0);
                    Jellyfish theJellyFish = Instantiate(_jellyFishPrefab, spawnPos, gameObject.transform.rotation).GetComponent<Jellyfish>();
                    SpawnedJellyFish.Add(theJellyFish);
                }
            }*/
        //}
    }
}
