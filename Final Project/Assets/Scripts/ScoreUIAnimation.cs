using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIAnimation : MonoBehaviour {
    [SerializeField]
    private Text selfText;
    [SerializeField]
    private Image selfImage;
    [SerializeField]
    private float shrinkSpeed;
    [SerializeField]
    private float alphaFade;
    [SerializeField]
    private int percentOfSizeRequiredForDespawn;
    private float percentScale;

    private float originalScale;
    // Use this for initialization
    void Start ()
    {
        //get a workable float value
        percentScale = percentOfSizeRequiredForDespawn / 100.0f;

    }
	
	// Update is called once per frame
	void Update ()
    {
        //Shrink by an amount per frame
        float uniformScale = gameObject.transform.localScale.x;
        gameObject.transform.localScale = new Vector3(uniformScale - shrinkSpeed, uniformScale - shrinkSpeed, uniformScale - shrinkSpeed);

        //TEXT FADING
        //Create a new colour that is equal to the text
        Color faded = selfText.GetComponent<Text>().color;
        //Minus the value we want to fade by
        faded.a = faded.a - alphaFade;
        //Set our text back to that colour
        selfText.GetComponent<Text>().color = faded;

        //IMAGE FADING
        //Use that same colour, but set it to the image's colour this time
        faded = selfImage.GetComponent<Image>().color;
        //Minus the faded value
        faded.a = faded.a - alphaFade;
        //And set it back
        selfImage.GetComponent<Image>().color = faded;

        //If we are x percent of our oringal scale, or nearly invisible, just despawn this object
        if (gameObject.transform.localScale.x <= originalScale * percentScale || selfImage.GetComponent<Image>().color.a < 0.1f)
        {
            //Debug.Log("UI is below size or opacity threshold.");
            Destroy(gameObject);
        }
    }

    public void SetSpawnParametres(float pAngle, float pScale)
    {
        //Rotate and scale the UI
        //Debug.Log("Angle is: " + pAngle);
        //gameObject.transform.localScale = new Vector3(pScale, pScale, pScale);
        gameObject.transform.Rotate(0.0f, 0.0f, pAngle);

        //Save this value to track our shrinkage later
        originalScale = pScale;
    }

    public void SetScoreText(float pScore)
    {
        selfText.text = "+" + pScore + "!";
    }

    public void SetScoreText(string pPercentScore)
    {
        selfText.text = pPercentScore;
    }

    public void SetScoreTextColour(Color pColour)
    {
        selfText.color = pColour;
    }
}
