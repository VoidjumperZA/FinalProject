using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour {
    private basic _basic;
    [SerializeField] private GameObject[] _trashPrefabs;
    [SerializeField] private Transform _leftSpawn;
    [SerializeField] private Transform _rightSpawn;
    [SerializeField] private int _trashAmount;
	// Use this for initialization
	void Start () {
        _basic = GetComponent<basic>();
        SpawnTrash();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnTrash()
    {
        Vector3 area = _rightSpawn.position - _leftSpawn.position;
        Vector3 spawnPosition;
        for (int i = 0; i < _trashAmount; i++)
        {
            spawnPosition = new Vector3(_leftSpawn.position.x + (area.x - (area.x / _trashAmount) * i) - area.x / _trashAmount, _leftSpawn.position.y + (_trashAmount / 12.0f) * i, _leftSpawn.position.z);
            trash theTrash = Instantiate(_trashPrefabs[0], spawnPosition, _trashPrefabs[0].transform.rotation).GetComponent<trash>();
            basic.AddCollectable(theTrash);
        }
    }
}
