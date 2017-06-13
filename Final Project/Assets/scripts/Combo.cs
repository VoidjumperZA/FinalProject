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
    private Sprite[] comboBrokenIconSprites;
    [SerializeField]
    private Sprite[] comboFishIconSprites;
    [SerializeField]
    private GameObject iconSpawnPosition;
    [Header("Values")]
    [SerializeField]
    private float iconScalingSize;
    [SerializeField]
    [Range(0, 500)]
    private float widthBetweenIcons;
    [SerializeField]
    private float iconSlideDistance;
    [SerializeField]
    private float iconSlideSpeed;
    [SerializeField]
    private int minComboSize;
    [SerializeField]
    private int maxComboSize;
    [SerializeField]
    private float comboScoreUIMovementSpeed;
    [SerializeField]
    private float comboScoreUIAlphaFade;
    [Header("Canvas")]
    [SerializeField]
    private Canvas canvas;
    private bool animatingComboBreak;
    private int iconsFinishedSliding;

    private GameplayValues gameplayValues;
    private int comboLength;
    private int comboIndex;
    private List<fish.FishType> combo;
    private List<Image> iconBackgroundsList;
    private List<Image> iconFishList;
    private List<float> iconSlideDistancesList;
    private List<float> iconSlideCounterList;
    private bool comboCanBeGenerated;

    private enum IconBackgroundStates { Standard, Next, Completed, Broken };
    private IconBackgroundStates iconBackgroundStates;
    // Use this for initialization
    void Start()
    {
        combo = new List<fish.FishType>();
        iconBackgroundsList = new List<Image>();
        iconFishList = new List<Image>();
        iconSlideDistancesList = new List<float>();
        iconSlideCounterList = new List<float>();
        gameplayValues = GameObject.Find("Manager").GetComponent<GameplayValues>();
        animatingComboBreak = false;
        iconsFinishedSliding = 0;
        comboCanBeGenerated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CreateNewCombo();
        }
        if (animatingComboBreak == true)
        {
            if (iconsFinishedSliding < iconBackgroundsList.Count)
            {
                Vector3 iconSpeedVector = new Vector3(0.0f, iconSlideSpeed);
                for (int i = 0; i < iconBackgroundsList.Count; i++)
                {
                    if (iconSlideCounterList[i] < Mathf.Abs(iconSlideDistancesList[i]))
                    {
                        iconBackgroundsList[i].transform.Translate(iconSpeedVector * Mathf.Sign(iconSlideDistancesList[i]));
                        iconFishList[i].transform.Translate(iconSpeedVector * Mathf.Sign(iconSlideDistancesList[i]));
                        iconSlideCounterList[i] += iconSlideSpeed;
                    }
                    else
                    {
                        iconsFinishedSliding++;
                        //Debug.Log("" + iconsFinishedSliding + " finished sliding.");
                    }
                }
            }
            else
            {
                animatingComboBreak = false;
                iconsFinishedSliding = 0;
                CreateNewCombo();
            }
        }

    }

    public void CheckComboProgress(fish.FishType pFishType)
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
    }
}
