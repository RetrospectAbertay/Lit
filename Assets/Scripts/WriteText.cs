using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteText : MonoBehaviour {

    Text textObject;
    string curDisplay;
    float writeSpeed;

	// Use this for initialization
	void Start ()
    {
        // get the contents of the string
        curDisplay = this.GetComponent<Text>().text;
        Debug.Log(curDisplay);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
