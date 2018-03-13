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

    private UnityStandardAssets._2D.Platformer2DUserControl player;
    private MenuState curState;
    private List<GameObject> CurMenuText = new List<GameObject>();
    public List<GameObject> MainMenuText = new List<GameObject>();
    public List<GameObject> Options = new List<GameObject>();
    public List<GameObject> LoadingText = new List<GameObject>();
    private int curSelection;
    private bool menuIsOpen;

    // Use this for initialization
    void Start () {
        menuIsOpen = false;
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

    public void ToggleMenu()
    {
        menuIsOpen = !menuIsOpen;
        if(menuIsOpen)
        {
            for (int i = 0; i < CurMenuText.Count; i++)
            {
                CurMenuText[i].SetActive(false);
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
        for (int i = 0; i < CurMenuText.Count; i++)
        {
            CurMenuText[i].SetActive(false);
        }
        switch (curState)
        {
            case MenuState.MAIN:
                {
                    CurMenuText = MainMenuText;
                    break;
                }
            case MenuState.OPTIONS:
                {

                    break;
                }
            case MenuState.LOADING:
                {
                    CurMenuText = LoadingText;
                    break;
                }
            default:
                {
                    break;
                }
        }
        // Activate next menu screen
        for (int i = 0; i < CurMenuText.Count; i++)
        {
            CurMenuText[i].SetActive(true);
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
            curSelection = (CurMenuText.Count - 1);
        }
        if (curSelection >= CurMenuText.Count)
        {
            Debug.Log("selection is bigger than menu text!");
            curSelection = 0;
        }
        for (int i = 0; i < CurMenuText.Count; i++)
        {
            if (i == curSelection && CurMenuText != LoadingText)
            {
                CurMenuText[i].GetComponent<Text>().color = Color.green;
            }
            else
            {
                CurMenuText[i].GetComponent<Text>().color = Color.white;
            }
        }
    }

    void ConfirmMenuSelection()
    {
        switch (curState)
        {
            case MenuState.MAIN:
                {
                    switch (curSelection)
                    {
                        case 0:
                            {
                                
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
}
