using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{

    [Header("Combo Pieces")]
    [SerializeField]
    private Image comboImageToInstantiate;
    [SerializeField]
    private Sprite[] comboIconStates;
    [SerializeField]
    private GameObject iconSpawnPosition;
    [SerializeField]
    [Range(0, 500)]
    private float widthBetweenIcons;
    [SerializeField]
    private Canvas canvas;

    private GameplayValues gameplayValues;
    private int comboLength;
    private List<fish.FishType> combo;
    // Use this for initialization
    void Start()
    {
        combo = new List<fish.FishType>();
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

    private void createNewCombo()
    {
        comboLength = Random.Range(gameplayValues.GetMinComboSize(), gameplayValues.GetMaxComboSize() + 1);
        Debug.Log("New Combo [" + comboLength + "]: ");
        int numberOfFish = System.Enum.GetNames(typeof(fish.FishType)).Length;
        for (int i = 0; i < comboLength; i++)
        {
            int comboIndex = Random.Range(0, numberOfFish);
            combo.Add((fish.FishType)comboIndex);
            Debug.Log("- " + combo[i].ToString());
            Image newComboIcon = GameObject.Instantiate(comboImageToInstantiate, canvas.transform);

            if (i == comboLength - 1)
            {
                newComboIcon.sprite = comboIconStates[1];
            }
            else
            {
                newComboIcon.sprite = comboIconStates[0];
            }

            Vector3 iconPosition = iconSpawnPosition.transform.position;
            iconPosition.x -= (i * widthBetweenIcons);
            newComboIcon.transform.position = iconPosition;
        }

    }

    
}
