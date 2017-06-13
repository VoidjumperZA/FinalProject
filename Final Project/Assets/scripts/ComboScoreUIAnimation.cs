using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboScoreUIAnimation : MonoBehaviour {
    [SerializeField]
    private Text selfText;
    private Image selfImage;
    private Combo combo;
    private bool fading;
	// Use this for initialization
	void Start () {
        selfImage = gameObject.GetComponent<Image>();
        combo = GameObject.Find("Manager").GetComponent<Combo>();
        fading = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Moving towards the score
        if (fading == false)
        {
            Vector3 hookScorePosition = Camera.main.WorldToScreenPoint(basic.Hook.transform.position);
            float speed = combo.GetComboScoreUIMovementSpeed();

            Vector3 differenceVector = (hookScorePosition - gameObject.transform.position);
            if (differenceVector.magnitude >= speed) gameObject.transform.Translate(differenceVector.normalized * speed);
            if (differenceVector.magnitude < speed)
            {
                fading = true;
                ScoreHandler scoreHandler = GameObject.Find("Manager").GetComponent<ScoreHandler>();
                scoreHandler.AddScore(scoreHandler.GetComboScoreValue(), false, false);
            }
        }
        //Fading out
        else
        {
            float alphaFade = combo.GetComboScoreUIAlphaFade();
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
        }
		
    }

    public void SetScoreText(int pScore)
    {
        selfText.text = "+" + pScore + "!";
    }
}
