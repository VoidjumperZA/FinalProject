using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFishSpawn : MonoBehaviour
{
    private basic _basic;

    [Header("Fish")]
    [SerializeField]
    private GameObject[] _fishPrefabs;
    [Header("Spawns")]
    [SerializeField]
    private float minTimeBetweenSpawns;
    [SerializeField]
    private float maxTimeBetweenSpawns;   
    [SerializeField]
    private float _spawnWidth;
    [SerializeField]
    private Transform _leftSpawn;
    [SerializeField]
    private Transform _rightSpawn;
    [SerializeField]
    private float _verticalSpawnFluctuation;
    private float _timePassed;
    private bool _valid;
   

    // Use this for initialization
    void Start()
    {
        _basic = GetComponent<basic>();
        //Max our time to start
        _timePassed = maxTimeBetweenSpawns;
        _spawnWidth /= 2;

        //Is our game valid, if there is disparity between how many fish types we have and the levels of spawning
        //then mark that as invalid.
        _valid = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_valid == true)
        {
            MoveSpawnArea(basic.Boat.transform.position);
            //Always count down time
            _timePassed -= Time.deltaTime;
            //Once our spawn timer is up
            if (_timePassed <= 0)
            {
                _basic.AddFish(CreateFish(Random.Range(0, 2)));
                _timePassed = maxTimeBetweenSpawns;
            }
        }
    }
    private fish CreateFish(int pPolarity)
    {
        //Choose a random fish and direction
        int randomFish = Random.Range(0, _fishPrefabs.Length);
        float verticalOffset = Random.Range(-_verticalSpawnFluctuation, _verticalSpawnFluctuation);

        GameObject newFish = Instantiate(_fishPrefabs[randomFish],
                                        (pPolarity == 0) ? _leftSpawn.position + new Vector3(0, verticalOffset, 0) : _rightSpawn.position + new Vector3(0, verticalOffset, 0),
                                        (pPolarity == 0) ? _leftSpawn.rotation : _rightSpawn.rotation);
        newFish.GetComponent<fish>().SetDirection(1.0f);
        newFish.GetComponent<fish>().SetFishType(randomFish);
        return newFish.GetComponent<fish>();
    }


    private void MoveSpawnArea(Vector3 pBoatPosition)
    {
        Vector3 differenceVector = pBoatPosition - new Vector3(0, 0.5f, 0);
        if (differenceVector.magnitude > 0)
        { 
                _leftSpawn.position = new Vector3(pBoatPosition.x - _spawnWidth, _leftSpawn.position.y, _leftSpawn.position.z);
                _rightSpawn.position = new Vector3(pBoatPosition.x + _spawnWidth, _rightSpawn.position.y, _rightSpawn.position.z);           
        }
    }
}
