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
    public GameObject LoadCanvas;
    public GameObject MainCanvas;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
      
    }

    public void TransitionToNextLvl()
    {
        GameObject ControlLayoutAnimation = GameObject.Find("Controls");
        if (ControlLayoutAnimation)
            {
            ControlLayoutAnimation.SetActive(false);
            }
            MainCanvas.SetActive(false);
            LoadCanvas.SetActive(true);
            switch (nextLevel)
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
