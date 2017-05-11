using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFishSpawn : MonoBehaviour
{
    private basic _basic;

    [SerializeField] private GameObject[] _fishPrefabs;
    [Header("Spawns")]
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _spawnWidth;
    [SerializeField] private Transform[] _leftSpawns;
    [SerializeField] private Transform[] _rightSpawns;
    [SerializeField] private float _verticalSpawnFluctuation;
    private float _timePassed;
    private bool _valid;

    // Use this for initialization
    void Start()
    {
        _basic = GetComponent<basic>();
        //Max our time to start
        _timePassed = _timeBetweenSpawns;
        _spawnWidth /= 2;

        //Is our game valid, if there is disparity between how many fish types we have and the levels of spawning
        //then mark that as invalid.
        _valid = true;
        if (_fishPrefabs.Length != _leftSpawns.Length || _fishPrefabs.Length != _rightSpawns.Length)
        {
            Debug.Log("WARNING: Fish spawns are missing in FishSpawner.\n\tMARKING SCRIPT AS INVALID");
            _valid = false;
        }
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
                _timePassed = _timeBetweenSpawns;
            }
        } 
    }
    private fish CreateFish(int pPolarity)
    {
        //Choose a random fish and direction
        int randomFish = Random.Range(0, _fishPrefabs.Length);
        float verticalOffset = Random.Range(-_verticalSpawnFluctuation, _verticalSpawnFluctuation);

        GameObject newFish = Instantiate(_fishPrefabs[randomFish],
                                        (pPolarity == 0) ?  _leftSpawns[randomFish].position : _rightSpawns[randomFish].position + new Vector3(0, verticalOffset, 0),
                                        (pPolarity == 0) ? _leftSpawns[randomFish].rotation : _rightSpawns[randomFish].rotation);
        newFish.GetComponent<fish>().SetDirection(1.0f);
        newFish.GetComponent<fish>().SetFishType(randomFish);
        return newFish.GetComponent<fish>();
    }
    private void MoveSpawnArea(Vector3 pBoatPosition)
    {
        Vector3 differenceVector = pBoatPosition - new Vector3(0, 0.5f, 0);
        if (differenceVector.magnitude > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i < 3) _leftSpawns[i].position = new Vector3(pBoatPosition.x - _spawnWidth, _leftSpawns[i].position.y, _leftSpawns[i].position.z);
                else _rightSpawns[i-3].position = new Vector3(pBoatPosition.x + _spawnWidth, _rightSpawns[i-3].position.y, _rightSpawns[i-3].position.z);
            }
        }
    }
}
