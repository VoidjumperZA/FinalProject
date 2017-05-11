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
        for (int i = 0; i < _trashAmount; i++)
        {
            trash theTrash = Instantiate(_trashPrefabs[0], _leftSpawn.position + (area - (area / _trashAmount) * i), Quaternion.identity).GetComponent<trash>();
            _basic.AddTrash(theTrash);
        }
    }
}
