using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour {

    private Button _deployHookButton;
    private Button _reelUpHook;
    private Button _radarButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeployHook()
    {
        boat b = GameObject.FindGameObjectWithTag("Boat").GetComponent<boat>();
        b.EnableFishing();

    }

    private void DisableButton(Button buttonToDisable) { button1.interactable = false; }
}
