using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFishSpawn : MonoBehaviour
{
    [SerializeField] private basic _basic;

    [SerializeField]
    private GameObject[] fishPrefabs;

    [SerializeField]
    private float timeBetweenSpawns;
    [SerializeField]
    private GameObject[] leftSpawns;
    [SerializeField]
    private GameObject[] rightSpawns;
    [SerializeField]
    private float verticalSpawnFluctuation;
    private float timePassed;
    private bool valid;

    // Use this for initialization
    void Start()
    {
        //Max our time to start
        timePassed = timeBetweenSpawns;

        //Is our game valid, if there is disparity between how many fish types we have and the levels of spawning
        //then mark that as invalid.
        valid = true;
        if (fishPrefabs.Length != leftSpawns.Length || fishPrefabs.Length != rightSpawns.Length)
        {
            Debug.Log("WARNING: Inconsistent number of fish to spawn levels.\n\tMARKING SCRIPT AS INVALID");
            valid = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (valid == true)
        {
            //Always count down time
            timePassed -= Time.deltaTime;
            //Once our spawn timer is up
            if (timePassed <= 0)
            {
                //Choose a random fish and direction
                int randomFish = Random.Range(0, fishPrefabs.Length);
                int polarity = Random.Range(0, 2);
                float verticalOffset = Random.Range(-verticalSpawnFluctuation, verticalSpawnFluctuation);
                GameObject newFish;
                //Debug.Log("Spawning fish. ID: " + randomFish + ". Polarity: " + polarity);

                //LEFT
                if (polarity == 0)
                {
                    newFish = Instantiate(fishPrefabs[randomFish], leftSpawns[randomFish].transform.position, leftSpawns[randomFish].transform.rotation);

                    //newFish = Instantiate(fishPrefabs[randomFish]);
                    Vector3 spawnPosition = leftSpawns[randomFish].transform.position;
                    spawnPosition.y += verticalOffset;
                    newFish.transform.position = spawnPosition;//leftSpawns[randomFish].transform.position;
                    newFish.GetComponent<fish>().SetDirection(1.0f);
                    newFish.GetComponent<fish>().SetFishType(randomFish);
                    //Debug.Log("Fish Pos (Left): " + newFish.transform.position);
                }
                //RIGHT
                else
                {
                    newFish = Instantiate(fishPrefabs[randomFish], rightSpawns[randomFish].transform.position, rightSpawns[randomFish].transform.rotation);
                    //newFish = Instantiate(fishPrefabs[randomFish]);
                    Vector3 spawnPosition = rightSpawns[randomFish].transform.position;
                    spawnPosition.y += verticalOffset;
                    newFish.transform.position = spawnPosition;//rightSpawns[randomFish].transform.position;
                    newFish.GetComponent<fish>().SetDirection(1.0f);
                    newFish.GetComponent<fish>().SetFishType(randomFish);
                    //Debug.Log("Fish Pos (Right): " + newFish.transform.position);
                }
                _basic.AddFish(newFish.GetComponent<fish>()); 
                //Set our time back to max            
                timePassed = timeBetweenSpawns;
            }
        } 
    }
}
