using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour {
    [HideInInspector] public bool Introduced = false;
    private enum FishInfo { Field, Type, Collected, ToCollect }
    [SerializeField] private Text[] _listTextfields; private int amount { get { return 3/*_listTextfields.Length*/; } }
    private int[] _type = new int[3];
    private int[] _toCollect = new int[3];
    private int[] _collected = new int[3];
    private int[,] _toCollectPreset = new int[,] { { 1, 2, 3 }, { 2, 4, 6 }, { 4, 8, 12 } };//new int[,] { { 15, 10, 5 }, { 30, 20, 10 }, { 60, 40, 20 } };
    private int[] _fishLevel = new int[] { 0, 0, 0 };

	void Start () {
        CacheNewCollection();
        SetUpLines();
        Show(false);
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.K)) CacheNewCollection();
	}

    public void AddFish(fish pFish)
    {
        //int pType = (int)pFish.GetFishType();
        _collected[(int)pFish.GetFishType()] += 1;
        /*_collected[(int)pFish.GetFishType()] = collected;

        Debug.Log(pType + " FishType " + collected + " collected " + _toCollect[pType] + " tocollect ");
        for (int i = 0; i < amount; i++)
        {
            if (_type[i] == pType)
            {
                if (_collected[pType] == _toCollect[pType])
            {
                     _fishLevel[pType] += 1;
                    _toCollect[pType] = _toCollectPreset[_fishLevel[pType], pType];

                    Debug.Log(pType + " FishType " + collected + " collected " + _toCollect[pType] + " tocollect ");
                    break;
                }
            }
        }*/
        SetUpLines();
    }
    private void CacheNewCollection()
    {
        int a = Random.Range(0, amount);
        int b = a;
        while (b == a) { b = Random.Range(0, amount); }
        int c = a;
        while (c == a || c == b) { c = Random.Range(0, amount); }
        //Debug.Log(a + " " + b + " " + c);
        _type = new[] { a, b, c };
        for (int i = 0; i < amount; i++) _toCollect[i] = GetToCollect(_type[i], _fishLevel[i]);

    }
    private int GetToCollect(int pType, int pLevel)
    {
        return _toCollectPreset[pLevel, pType];
    }
    private void SetUpLines()
    {
        for (int i = 0; i < amount; i++) _listTextfields[i].text = "Fish " + (_type[i] + 1) + " collected: " + _collected[i] + "/" + _toCollect[i];
    }
    private void EditLine(int pIndex)
    {
    }
    public void Show(bool pBool)
    {
        foreach (Text text in _listTextfields) text.gameObject.SetActive(pBool);
    }
    /*private int GetInfo(FishInfo pList, FishInfo pInfo)
    {
        return _listTextfields[(int)pList];
    }*/

}
