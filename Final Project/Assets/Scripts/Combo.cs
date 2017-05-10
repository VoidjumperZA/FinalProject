using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{

    [Header("Combo Pieces")]
    [SerializeField]
    private Image comboBackgroundImageToInstantiate;
    [SerializeField]
    private Image comboFishImageToInstantiate;
    [SerializeField]
    private Sprite[] comboBackgroundIconSprites;
    [SerializeField]
    private Sprite[] comboFishIconSprites;
    [SerializeField]
    private GameObject iconSpawnPosition;
    [SerializeField]
    [Range(0, 500)]
    private float widthBetweenIcons;
    [SerializeField]
    private Canvas canvas;

    private GameplayValues gameplayValues;
    private int comboLength;
    private int comboIndex;
    private List<fish.FishType> combo;
    private List<Image> iconBackgroundsList;
    private List<Image> iconFishList;

    private enum IconBackgroundStates { Standard, Next, Completed, Broken };
    private IconBackgroundStates iconBackgroundStates;
    // Use this for initialization
    void Start()
    {
        combo = new List<fish.FishType>();
        iconBackgroundsList = new List<Image>();
        iconFishList = new List<Image>();
        gameplayValues = GameObject.Find("Manager").GetComponent<GameplayValues>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            createNewCombo();
        }
    }

    public void CheckComboProgress(fish.FishType pFishType)
    {
        Debug.Log("Checking Combo Progress.");
        if (pFishType == combo[comboIndex])
        {
            Debug.Log("Combo type was correct!");
            iconBackgroundsList[comboIndex].sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Completed];
            comboIndex++;
            iconBackgroundsList[comboIndex].sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Next];
        }
    }

    private void createNewCombo()
    {
        comboIndex = 0;
        for (int i = 0; i < iconBackgroundsList.Count; i++)
        {
            Destroy(iconBackgroundsList[i].gameObject);
            Destroy(iconFishList[i].gameObject);
        }
        iconBackgroundsList.Clear();
        iconFishList.Clear();

        comboLength = Random.Range(gameplayValues.GetMinComboSize(), gameplayValues.GetMaxComboSize());
        Debug.Log("New Combo [" + comboLength + 1 + "]: ");
        int numberOfFish = System.Enum.GetNames(typeof(fish.FishType)).Length;
        for (int i = comboLength; i > -1; i--)
        {
            int fishTypeIndex = Random.Range(0, numberOfFish);
            combo.Add((fish.FishType)fishTypeIndex);
            Debug.Log("- (" + fishTypeIndex + ")" + combo[comboLength - i].ToString());
            Image newComboIconBackground = GameObject.Instantiate(comboBackgroundImageToInstantiate, canvas.transform);
            Image newComboIconFish = GameObject.Instantiate(comboFishImageToInstantiate, canvas.transform);
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

    }

    
}
