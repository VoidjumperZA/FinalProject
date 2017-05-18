using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeafloorSpawning : MonoBehaviour
{
    [SerializeField]
    private int amountOfTrashOnSeafloor;
    [SerializeField]
    private int amountOfSpecialItemsOnSeafloor;
    [SerializeField]
    private GameObject specicalItemPiece;
    [SerializeField]
    private GameObject[] trashPieces;
    [SerializeField]
    private Transform[] specialItemSpawns;
    private List<Transform> trashSpawns;


    // Use this for initialization
    void Start()
    {
        Debug.Log("TrashPieces: " + trashPieces.Length);
        trashSpawns = new List<Transform>();
        GameObject[] arr = GameObject.FindGameObjectsWithTag("TrashSpawn");
        for (int i = 0; i < arr.Length; i++)
        {
            trashSpawns.Add(arr[i].transform);
        }
        Debug.Log("TrashSpawns: " + trashSpawns.Count);
        int spawnCount = amountOfTrashOnSeafloor;
        if (amountOfTrashOnSeafloor > trashSpawns.Count)
        {
            Debug.Log("WARNING: Trying to spawn more pieces of trash than available spawn points, some will not be instantiated.");
            spawnCount = trashSpawns.Count;
        }
        for (int i = 0; i < spawnCount; i++)
        {           
            int spawnNumber = Random.Range(0, trashSpawns.Count);
            GameObject newTrash = Instantiate(trashPieces[Random.Range(0, trashPieces.Length)]);
            newTrash.transform.position = trashSpawns[spawnNumber].transform.position;
            newTrash.SetActive(true);
            trashSpawns.Remove(trashSpawns[spawnNumber]);
        }
        basic.Scorehandler.SetTotalNumberOfTrashPieces(spawnCount);
    }

    // Update is called once per frame
    void Update()
    { 

    }
}
