using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScript : MonoBehaviour {

    public enum TRANSITLEVEL
    {
        FirstLevel,
        SecondLevel,
        ThirdLevel,
        FourthLevel,
        FifthLevel
    }

    public TRANSITLEVEL nextLevel;
    public Text timexLetters;


	// Use this for initialization
	void Start () {
        switch (nextLevel)
        {
            case TRANSITLEVEL.FirstLevel:
                timexLetters.text = "";
                break;
            case TRANSITLEVEL.SecondLevel:
                timexLetters.text = "T";
                break;
            case TRANSITLEVEL.ThirdLevel:
                timexLetters.text = "T I";
                break;
            case TRANSITLEVEL.FourthLevel:
                timexLetters.text = "T I M";
                break;
            case TRANSITLEVEL.FifthLevel:
                timexLetters.text = "T I M E";
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        // player press space - go to the next level
        if (Input.GetKeyDown("space"))
        {
            switch(nextLevel)
            {
                case TRANSITLEVEL.FirstLevel:
                    SceneManager.LoadScene("1. T Level");
                    timexLetters.text = "";
                    break;
                case TRANSITLEVEL.SecondLevel:
                    SceneManager.LoadScene("2. I Level");
                    timexLetters.text = "T";
                    break;
                case TRANSITLEVEL.ThirdLevel:
                    SceneManager.LoadScene("3. M Level");
                    timexLetters.text = "T I";
                    break;
                case TRANSITLEVEL.FourthLevel:
                    SceneManager.LoadScene("4. E Level");
                    timexLetters.text = "T I M";
                    break;
                case TRANSITLEVEL.FifthLevel:
                    SceneManager.LoadScene("5. X Level");
                    timexLetters.text = "T I M E";
                    break;
            }
        }
    }
}
