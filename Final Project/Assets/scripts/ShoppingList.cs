using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour {
    [HideInInspector] public bool Introduced = false;
    [SerializeField] private Text[] _listTextfields;
    private Dictionary<int, int[]> _fishInfo = new Dictionary<int, int[]>();

    private int[,] _toCollectPreset = new int[,] { { 10, 20, 30 }, { 20, 40, 60 }, { 40, 80, 120 } };//new int[,] { { 15, 10, 5 }, { 30, 20, 10 }, { 60, 40, 20 } };

    /*private int[] _type = new int[3];
    private int[] _toCollect = new int[3];
    private int[] _collected = new int[3];
    private int[] _fishLevel = new int[] { 0, 0, 0 };*/

    void Start () {
        CacheNewCollection();
        SetUpTextFields();
        Show(false);
    }
	
	public void Update () {
        if (Input.GetKeyDown(KeyCode.Z)) CacheNewCollection();
    }

    public void AddFish(fish pFish)
    {
        int type = (int)pFish.GetFishType();
        _fishInfo[type][1] += 1;
        SetUpTextFields();
    }
    private void CacheNewCollection()
    {
        //Debug.Log("Started generating");
        _fishInfo = new Dictionary<int, int[]>();
        List<int> chosen = new List<int>();

        int tempType = 0;
        for (int i = 0; i < 3; i++)
        {
            bool again = true;
            while (again)
            {
                tempType = Random.Range(0, 3);
                if (!chosen.Contains(tempType))
                {
                    again = false;
                    chosen.Add(tempType);
                }
            }
            _fishInfo[chosen[i]] = new int[3] { Random.Range(0, 3), 0, _toCollectPreset[chosen[i], Random.Range(0, 3)] };
        }
        chosen.Clear();
        //for (int i =0; i < 3; i++) Debug.Log(i + "  " + _fishType[i]);
    }
    /*private int GetToCollect(int pType, int pLevel)
    {
        return _toCollectPreset[pLevel, pType];
    }*/
    private void SetUpTextFields()
    {
        _listTextfields[0].text = "Small fish color: " + _fishInfo[0][0] + " collected: " + _fishInfo[0][1] + "/" + _fishInfo[0][2];
        _listTextfields[1].text = "Medium fish color: " + _fishInfo[1][0] + " collected: " + _fishInfo[1][1] + "/" + _fishInfo[1][2];
        _listTextfields[2].text = "Large fish color: " + _fishInfo[2][0] + " collected: " + _fishInfo[2][1] + "/" + _fishInfo[2][2];
    }
    private void EditLine(int pIndex)
    {
    }
    public void Show(bool pBool)
    {
        for (int i = 0; i < 3; i++)
        {
            _listTextfields[i].gameObject.SetActive(pBool);
        }
        //Debug.Log("Shopping list is hidden");
    }
    /*private int GetInfo(FishInfo pList, FishInfo pInfo)
    {
        return _listTextfields[(int)pList];
    }*/

}
