using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteText : MonoBehaviour {

    Text textObject;
    string stringToDisplay;
    public List<string> paragraphs;
    public float writeSpeed;
    float timer;
    int curChar = 0;
    int curParagraph = 0;
    bool finishedWriting = false;

	// Use this for initialization
	void Start ()
    {
        // get the text object iself
        textObject = this.GetComponent<Text>();
        textObject.text = "";
        // get the contents of the string
        stringToDisplay = paragraphs[0];
        Debug.Log(stringToDisplay);
        // setup timer to init to 0
        timer = 0.0f;

    }
	
	// Update is called once per frame
	void Update ()
    {
        // check if we still need to write
		if(!finishedWriting)
        {
            Debug.Log("updating timer");
            // increment timer
            timer += Time.deltaTime;
            if(timer > writeSpeed)
            { 
                // display a new character
                updateText();
                // increment cur char
                curChar++;
                // reset timer
                timer = 0.0f;
            }
        }
	}

    void updateText()
    {
        // check if we need to switch paragraphs
        if (curChar >= stringToDisplay.Length)
        {
            if (curParagraph < paragraphs.Count)
            {
                // increment paragraph to display
                curParagraph++;
                // set string to display
                stringToDisplay = paragraphs[curParagraph];
                // reset current char
                curChar = 0;
                // clear text object
                textObject.text = "";
                // update text
                textObject.text += stringToDisplay[curChar];
            }
            else
            {
                finishedWriting = true;
            }
        }
        else
        {
            // update text
            textObject.text += stringToDisplay[curChar];
        }
    }
}
