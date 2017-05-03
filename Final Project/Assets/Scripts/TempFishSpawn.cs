using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFishSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] fishPrefabs;

    [SerializeField]
    private float timeBetweenSpawns;
    [SerializeField]
    private GameObject[] leftSpawns;
    [SerializeField]
    private GameObject[] rightSpawns;
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
            Debug.Log("WARNING: Inconsistent number of fish to spawn levels.");
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
                GameObject newFish;
                //Debug.Log("Spawning fish. ID: " + randomFish + ". Polarity: " + polarity);

                //LEFT
                if (polarity == 0)
                {
                    newFish = Instantiate(fishPrefabs[randomFish]);
                    newFish.transform.position = leftSpawns[randomFish].transform.position;
                    newFish.GetComponent<fish>().SetDirection(1.0f);
                    newFish.GetComponent<fish>().SetFishType(randomFish);
                    //Debug.Log("Fish Pos (Left): " + newFish.transform.position);
                }
                //RIGHT
                else
                {
                    newFish = Instantiate(fishPrefabs[randomFish]);
                    newFish.transform.position = rightSpawns[randomFish].transform.position;
                    newFish.GetComponent<fish>().SetDirection(-1.0f);
                    newFish.GetComponent<fish>().SetFishType(randomFish);
                    //Debug.Log("Fish Pos (Right): " + newFish.transform.position);
                }   
                //Set our time back to max            
                timePassed = timeBetweenSpawns;
            }
        } 
    }
}
