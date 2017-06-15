using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    private bool _initialized = false;
    [SerializeField] private List<Sprite> _beforeCollectPrefab;
    [SerializeField] private List<Sprite> _afterCollectPrefab;
    private Dictionary<int, Sprite> _beforeCollect = new Dictionary<int, Sprite>();
    private Dictionary<int, Sprite> _afterCollect = new Dictionary<int, Sprite>();
    private List<List<int>> _fishInfo = new List<List<int>>();
    private List<int> _currentTypes = new List<int>();
    private int _currentToCollect = 0;
    private int _comboSize = 0;
    public void Initialize()
    {
        if (_initialized) return;
        _fishInfo = GameManager.ShopList.FishInfo;
        // Cache sprites
        for (int i = 0; i < _beforeCollectPrefab.Count; i++) _beforeCollect[i] = _beforeCollectPrefab[i];
        for (int i = 0; i < _afterCollectPrefab.Count; i++) _afterCollect[i] = _afterCollectPrefab[i];
        GenerateCombo();
        _initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized) return;
        if (Input.GetKeyDown(KeyCode.P)) GenerateCombo();
        if (Input.GetKeyDown(KeyCode.L)) Collect(_currentTypes[_currentToCollect]);

    }
    public void GenerateCombo()
    {
        _comboSize = Random.Range(3, 6);
        // Generate Combo
        GameManager.Levelmanager._levelUI.ComboUI.SetActive(true);
        // Generate Combo (Assing Sprites)
        _currentTypes.Clear();
        _currentToCollect = 0;
        // Combo Fillstages
        for (int i = 0; i < 5; i++) GameManager.Levelmanager._levelUI.ComboFillStages[i].gameObject.SetActive(false);
        // Combo Icons
        for (int i = 0; i < 5; i++) GameManager.Levelmanager._levelUI.ComboIconHolders[i].gameObject.SetActive(false);
        for (int i = 0; i < _comboSize; i++)
        {
            int rnd = Random.Range(0,  _fishInfo.Count);
            int type = _fishInfo[rnd][0];
            _currentTypes.Add(type);
            Debug.Log(rnd + "ComboRNDCount");
            ReAssign(_beforeCollect, i, type, _comboSize);
        }
    }
    public void Collect(int pFishType)
    {
        // Combo Icons
        if (_currentToCollect < _currentTypes.Count)
        {
            int type = _currentTypes[_currentToCollect];
            if (type == pFishType)
            {
                ReAssign(_afterCollect, _currentToCollect, type, _currentTypes.Count);
                _currentToCollect += 1;
            }
        }
        // Combo FillStages
        foreach (Image img in GameManager.Levelmanager._levelUI.ComboFillStages) img.gameObject.SetActive(false);
        if (_comboSize == 5) GameManager.Levelmanager._levelUI.ComboFillStages[_currentToCollect - 1].gameObject.SetActive(true);
        if (_comboSize == 4) GameManager.Levelmanager._levelUI.ComboFillStages[_currentToCollect].gameObject.SetActive(true);
        if (_comboSize == 3) GameManager.Levelmanager._levelUI.ComboFillStages[_currentToCollect + 1].gameObject.SetActive(true);

        /*for (int i = 0; i < _currentTypes.Count; i++)
        {
            GameManager.Levelmanager._levelUI.ComboFillStages[i].gameObject.SetActive(i == _currentToCollect-1);
        }*/
        if (_currentToCollect >= _currentTypes.Count)
        {
            GenerateCombo();
        }
    }
    private void ReAssign(Dictionary<int, Sprite> pDictionary, int pDictionaryIndex , int pType, int pComboSize)
    {
        float newX = ((-pComboSize * 100) + 100) + (pDictionaryIndex * 200);
        GameManager.Levelmanager._levelUI.ComboIconHolders[pDictionaryIndex].rectTransform.localPosition = new Vector3(newX, 0, 0);
        GameManager.Levelmanager._levelUI.ComboIconHolders[pDictionaryIndex].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pDictionary[pType].rect.width);
        GameManager.Levelmanager._levelUI.ComboIconHolders[pDictionaryIndex].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pDictionary[pType].rect.height);
        GameManager.Levelmanager._levelUI.ComboIconHolders[pDictionaryIndex].sprite = pDictionary[pType];
        GameManager.Levelmanager._levelUI.ComboIconHolders[pDictionaryIndex].gameObject.SetActive(true);
    }
    /*public void CheckComboProgress(fish.FishType pFishType)
    {
        //Debug.Log("Checking Combo Progress. Combo Index: " + comboIndex);
        if (animatingComboBreak == false && pFishType == combo[comboIndex])
        {
            //Debug.Log("Combo type was correct! Hit fish was " + pFishType + " and required fish was " + combo[comboIndex]);
            iconBackgroundsList[comboIndex].sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Completed];
            comboIndex++;
            if (comboIndex == (comboLength + 1))
            {
                //Debug.Log("Combo Completed! ComboIndex: " + comboIndex + " and combo length: " + comboLength);
                GameObject.Find("Manager").GetComponent<ScoreHandler>().AddComboScore();
                CreateNewCombo();
                return;
            }
            iconBackgroundsList[comboIndex].sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Next];
        }
        else
        {
            //Debug.Log("Combo type was incorrect! Hit fish was " + pFishType + " and required fish was " + combo[comboIndex]);
            animatingComboBreak = true;
            for (int i = 0; i < iconBackgroundsList.Count; i++)
            {
                iconSlideDistancesList.Add(Random.Range(-iconSlideDistance, iconSlideDistance));
                //Debug.Log("Distance: " + iconSlideDistancesList[i]);
                iconSlideCounterList.Add(0.0f);
                int iconIndex = -1;
                if (i < comboIndex)
                {
                    iconIndex = (int)IconBackgroundStates.Completed;
                }
                else if (i == comboIndex)
                {
                    iconIndex = (int)IconBackgroundStates.Next;
                }
                else if (i > comboIndex)
                {
                    iconIndex = (int)IconBackgroundStates.Standard;
                }
                iconBackgroundsList[i].sprite = comboBrokenIconSprites[iconIndex];
            }
        }
    }

    public void ClearPreviousCombo(bool pCanGenerateNewCombo)
    {
        comboCanBeGenerated = pCanGenerateNewCombo;
        comboIndex = 0;
        for (int i = 0; i < iconBackgroundsList.Count; i++)
        {
            Destroy(iconBackgroundsList[i].gameObject);
            Destroy(iconFishList[i].gameObject);
        }
        combo.Clear();
        iconBackgroundsList.Clear();
        iconFishList.Clear();
        iconSlideDistancesList.Clear();
        iconSlideCounterList.Clear();
    }

    public void CreateNewCombo()
    {
        //Debug.Log("Combo Size: " + combo.Count);
        ClearPreviousCombo(true);

        if (comboCanBeGenerated == true)
        {
            comboLength = Random.Range(minComboSize, maxComboSize);
            //Debug.Log("New Combo [" + (comboLength + 1) + "]: ");
            int numberOfFish = System.Enum.GetNames(typeof(fish.FishType)).Length;
           // Debug.Log("There are " + numberOfFish + " types.");
            for (int i = comboLength; i > -1; i--)
            {
                int fishTypeIndex = Random.Range(0, numberOfFish);
                combo.Add((fish.FishType)fishTypeIndex);
                //Debug.Log("-> (" + fishTypeIndex + ")" + combo[combo.Count - 1].ToString());
                Image newComboIconBackground = GameObject.Instantiate(comboBackgroundImageToInstantiate, canvas.transform);
                Image newComboIconFish = GameObject.Instantiate(comboFishImageToInstantiate, canvas.transform);
                newComboIconBackground.transform.localScale = new Vector3(iconScalingSize, iconScalingSize, iconScalingSize);
                newComboIconFish.transform.localScale = new Vector3(iconScalingSize, iconScalingSize, iconScalingSize);
                newComboIconFish.sprite = comboFishIconSprites[fishTypeIndex];

                if (i == comboLength)
                {
                    newComboIconBackground.sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Next];
                }
                else
                {
                    newComboIconBackground.sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Standard];
                }

                Vector3 iconPosition = iconSpawnPosition.transform.position;
                iconPosition.x -= (i * widthBetweenIcons);
                newComboIconBackground.transform.position = iconPosition;
                newComboIconFish.transform.position = iconPosition;
                iconBackgroundsList.Add(newComboIconBackground);
                iconFishList.Add(newComboIconFish);
            }
            for (int i = 0; i < comboLength + 1; i++)
            {
                //Debug.Log("- " + combo[i].ToString());
            }
        }       
    }

    public float GetComboScoreUIMovementSpeed()
    {
        return comboScoreUIMovementSpeed;
    }

    public float GetComboScoreUIAlphaFade()
    {
        return comboScoreUIAlphaFade;
    }*/
}
