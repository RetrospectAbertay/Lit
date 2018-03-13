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
        FifthLevel,
        Menu
    }

    public TRANSITLEVEL nextLevel;

	// Use this for initialization
	void Start () {

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
                    break;
                case TRANSITLEVEL.SecondLevel:
                    SceneManager.LoadScene("2. I Level");
                    break;
                case TRANSITLEVEL.ThirdLevel:
                    SceneManager.LoadScene("3. M Level");
                    break;
                case TRANSITLEVEL.FourthLevel:
                    SceneManager.LoadScene("4. E Level");
                    break;
                case TRANSITLEVEL.FifthLevel:
                    SceneManager.LoadScene("5. X Level");
                    break;
                case TRANSITLEVEL.Menu:
                    SceneManager.LoadScene("Menu");
                    break;
            }
        }
    }
}
