using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour {
    private bool _initialized = false;
    [HideInInspector] public bool Introduced = false;

    [HideInInspector] public int AmountOfTypes = 3;
    [SerializeField] private int _usePreset;
    [SerializeField] private Vector3[] _presetsPerFishType;
    [SerializeField] private List<Text> _listTextfields;
    [SerializeField] private List<Image> _listImageFields;
    [SerializeField] private List<Sprite> _listAllFishImages;
    public Dictionary<int, List<int>> FishInfo { get; set; }

    private List<List<int>> _toCollectPreset = new List<List<int>>(); //new int[,] { { 10, 20, 30 }, { 20, 40, 60 }, { 40, 80, 120 } };//new int[,] { { 15, 10, 5 }, { 30, 20, 10 }, { 60, 40, 20 } };
    void Start () {
        //GenerateShoppingList();
        //Show(false);
    }
	
	public void Update () {
        if (Input.GetKeyDown(KeyCode.Z)) GenerateShoppingList();
        //FadeIn();
    }
    private void Initialize()
    {
        AmountOfTypes = _presetsPerFishType.Length;
        _usePreset = Mathf.Min(_usePreset, AmountOfTypes - 1);
        _toCollectPreset.Clear();
        for (int i = 0; i < _presetsPerFishType.Length; i++)
        {
            _toCollectPreset.Add(new List<int> { (int)_presetsPerFishType[i].x, (int)_presetsPerFishType[i].y, (int)_presetsPerFishType[i].z });
        }
        if (!_initialized)
        {
            FishInfo = new Dictionary<int, List<int>>();
            _initialized = true;
        }
    }
    public void GenerateShoppingList()
    {
        Initialize();
        //Debug.Log("Started generating");
        FishInfo.Clear();
        List<int> alreadyChosen = new List<int>();

        int tempType = 0;
        for (int i = 0; i < AmountOfTypes; i++)
        {
            bool again = true;
            while (again)
            {
                tempType = Random.Range(0, AmountOfTypes);
                if (!alreadyChosen.Contains(tempType))
                {
                    again = false;
                    alreadyChosen.Add(tempType);
                }
            }
            FishInfo[i] = new List<int>();
            FishInfo[i].Add(alreadyChosen[i]);
            FishInfo[i].Add(0);
            FishInfo[i].Add(_toCollectPreset[FishInfo[i][0]][(_usePreset == 0) ? Random.Range(0, AmountOfTypes) : _usePreset]);
        }
        GameManager.Fishspawner.TakeShoppingListValues();

        alreadyChosen.Clear();
        SetUpTextFields();
        SetUpImageFields();
    }
    private void SetUpTextFields()
    {
        _listTextfields[0].text = FishInfo[0][1] + "/" + FishInfo[0][2];
        _listTextfields[1].text = FishInfo[1][1] + "/" + FishInfo[1][2];
        _listTextfields[2].text = FishInfo[2][1] + "/" + FishInfo[2][2];
    }

    private void SetUpImageFields()
    {
        _listImageFields[0].sprite = _listAllFishImages[FishInfo[0][0]];
        _listImageFields[1].sprite = _listAllFishImages[FishInfo[1][0]];
        _listImageFields[2].sprite = _listAllFishImages[FishInfo[2][0]];
    }
    public void CollectFish(int pFishType)
    {
        for (int i = 0; i < FishInfo.Count; i++)
        {
            if (FishInfo[i][0] == pFishType) if (FishInfo[i][1] < FishInfo[i][2]) FishInfo[i][1] += 1;
        }
        SetUpTextFields();
    }
    private void FadeIn()
    {
        //_background.CrossFadeAlpha(1.0f, 2, false);
    }
    public void Show(bool pBool)
    {
        //_background.CrossFadeAlpha(0.0f, 0.0f, false);
        //_background.gameObject.SetActive(pBool);
        FadeIn();
        for (int i = 0; i < 3; i++)
        {
            _listTextfields[i].gameObject.SetActive(pBool);
        }
    }

}
