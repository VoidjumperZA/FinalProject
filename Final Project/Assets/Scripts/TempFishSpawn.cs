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
        timePassed = timeBetweenSpawns;
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
            timePassed -= Time.deltaTime;
            if (timePassed <= 0)
            {
                int randomFish = Random.Range(0, fishPrefabs.Length);
                int polarity = Random.Range(0, 2);
                GameObject newFish;
                //Debug.Log("Spawning fish. ID: " + randomFish + ". Polarity: " + polarity);
                if (polarity == 0)
                {
                    newFish = Instantiate(fishPrefabs[randomFish], leftSpawns[randomFish].transform);
                    newFish.GetComponent<fish>().SetDirection(1.0f);
                    //Debug.Log("Fish Pos (Left): " + newFish.transform.position);
                }
                else
                {
                    newFish = Instantiate(fishPrefabs[randomFish], rightSpawns[randomFish].transform);
                    newFish.GetComponent<fish>().SetDirection(-1.0f);
                    //Debug.Log("Fish Pos (Right): " + newFish.transform.position);
                }               
                timePassed = timeBetweenSpawns;
            }
        } 
    }
}
