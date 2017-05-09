using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
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
            combo.Add((fish.FishType)Random.Range(0, numberOfFish));
            Debug.Log("- " + combo[i].ToString());
        }

    }

    
}
