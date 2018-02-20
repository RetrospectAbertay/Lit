using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteText : MonoBehaviour {

    Text textObject;
    string stringToDisplay;
    public float writeSpeed;
    float timer;
    int curChar = 0;

	// Use this for initialization
	void Start ()
    {
        // get the text object iself
        textObject = this.GetComponent<Text>();
        // get the contents of the string
        stringToDisplay = textObject.text;
        Debug.Log(stringToDisplay);
        // setup timer to init to 0
        timer = 0.0f;
        // display empty text
        updateText();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // check if we still need to write
		if(curChar < stringToDisplay.Length)
        {
            Debug.Log("updating timer");
            // increment timer
            timer += Time.deltaTime;
            if(timer > writeSpeed)
            {
                // increment cur char
                curChar++;
                // display a new character
                updateText();
                // reset timer
                timer = 0.0f;
            }
        }
	}

    void updateText()
    {
        // initialise new string
        string curText = "";
        // iterate through the initial message
        for(int i = 0; i < curChar; i++)
        {
            // add each char to the current text
            curText += stringToDisplay[i];
        }
        Debug.Log(curText);
        // update text object
        textObject.text = curText;
    }
}
