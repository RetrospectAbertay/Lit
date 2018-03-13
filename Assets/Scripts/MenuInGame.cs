using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInGame : MonoBehaviour {

    enum MenuState
    {
        MAIN,
        OPTIONS,
        LOADING
    }

    private MenuState curState;
    private List<GameObject> curMenuText = new List<GameObject>();
    public List<GameObject> MainMenuText = new List<GameObject>();
    public List<GameObject> Options = new List<GameObject>();
    public List<GameObject> LoadingText = new List<GameObject>();
    private int curSelection;
    private bool menuIsOpen;
    private bool signalToggle;

    // Use this for initialization
    void Start () {
        menuIsOpen = false;
        curMenuText = MainMenuText;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (menuIsOpen)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                curSelection--;
                UpdateMenuSelection();
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                curSelection++;
                UpdateMenuSelection();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConfirmMenuSelection();
            }
        }
    }

    public bool IsInMenu()
    {
        return menuIsOpen;
    }

    public void ToggleMenu()
    {
        menuIsOpen = !menuIsOpen;
        curSelection = 0;
        if(!menuIsOpen)
        {
            for (int i = 0; i < curMenuText.Count; i++)
            {
                curMenuText[i].SetActive(false);
            }
        }
        else
        {
            SwitchMenu(MenuState.MAIN);
        }
    }

    void SwitchMenu(MenuState newState)
    {
        // update state
        curState = newState;
        // turn of previous menu
        for (int i = 0; i < curMenuText.Count; i++)
        {
            curMenuText[i].SetActive(false);
        }
        switch (curState)
        {
            case MenuState.MAIN:
                {
                    curMenuText = MainMenuText;
                    break;
                }
            case MenuState.OPTIONS:
                {

                    break;
                }
            case MenuState.LOADING:
                {
                    curMenuText = LoadingText;
                    break;
                }
            default:
                {
                    break;
                }
        }
        // Activate next menu screen
        for (int i = 0; i < curMenuText.Count; i++)
        {
            curMenuText[i].SetActive(true);
        }
        // setup selection
        curSelection = 0;
        // apply new selection
        UpdateMenuSelection();
    }

    void UpdateMenuSelection()
    {
        if (curSelection < 0)
        {
            Debug.Log("selection is less than zero!");
            curSelection = (curMenuText.Count - 1);
        }
        if (curSelection >= curMenuText.Count)
        {
            Debug.Log("selection is bigger than menu text!");
            curSelection = 0;
        }
        for (int i = 0; i < curMenuText.Count; i++)
        {
            if (i == curSelection && curMenuText != LoadingText)
            {
                curMenuText[i].GetComponent<Text>().color = Color.green;
            }
            else
            {
                curMenuText[i].GetComponent<Text>().color = Color.white;
            }
        }
    }

    public void ConfirmMenuSelection()
    {
        switch (curState)
        {
            case MenuState.MAIN:
                {
                    switch (curSelection)
                    {
                        case 0:
                            {
                                signalToggle = true;
                                break;
                            }
                        case 1:
                            {
                                SwitchMenu(MenuState.OPTIONS);
                                break;
                            }
                        case 2:
                            {
                                SwitchMenu(MenuState.LOADING);
                                SceneManager.LoadScene("Menu");
                                break;
                            }
                        default:
                            {
                                Debug.Log("Triggered default switch");
                                break;
                            }
                    }
                    break;
                }
            case MenuState.OPTIONS:
                {
                    switch (curSelection)
                    {
                        case 0:
                            {
                                break;
                            }
                        case 1:
                            {
                                break;
                            }
                        case 2:
                            {
                                break;
                            }
                        default:
                            {
                                Debug.Log("Triggered default switch");
                                break;
                            }
                    }
                    break;
                }
            case MenuState.LOADING:
                {
                    break;
                }
        }
    }

    public bool wantsToToggle()
    {
        // function created to indicate that the player wants to exit the menu
        // without accessing the player controller directly thougth this script
        bool wantsToggle = signalToggle;
        if(wantsToggle)
        {
            signalToggle = false;
        }
        return wantsToggle;
    }
}
