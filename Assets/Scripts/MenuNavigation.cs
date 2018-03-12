using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour {

    enum MenuState
    {
        MAIN,
        OPTIONS,
        LEVELSELECT
    }

    private MenuState curState;
    private List<GameObject> CurMenuText = new List<GameObject>();
    public List<GameObject> MainMenuText;
    public List<GameObject> LevelSelectMenuText;
    public List<Image> SelectionIndicator;
    private int curSelection;
    private int selectedLevel;
    private string levelSelectString = "TIMEX";
    private int levelsUnlocked = 5;

	// Use this for initialization
	void Start () {
        SwitchMenu(MenuState.MAIN);

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            curSelection--;
            UpdateMenuSelection();
        }
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            curSelection++;
            UpdateMenuSelection();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmMenuSelection();
        }
        if (curSelection == 0 && curState == MenuState.LEVELSELECT)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeLevel(true);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeLevel(false);
            }
        }
    }

    void SwitchMenu(MenuState newState)
    {
        // update state
        curState = newState;
        // turn of previous menu
        for(int i = 0; i < CurMenuText.Count; i++)
        {
            CurMenuText[i].SetActive(false);
        }
        switch(curState)
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
            case MenuState.LEVELSELECT:
                {
                    CurMenuText = LevelSelectMenuText;
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
        if(curSelection < 0)
        {
            Debug.Log("selection is less than zero!");
            curSelection = (CurMenuText.Count - 1);
        }
        if(curSelection >= CurMenuText.Count)
        {
            Debug.Log("selection is bigger than menu text!");
            curSelection = 0;
        }
        for(int i = 0; i < CurMenuText.Count; i++)
        {
            if(i == curSelection)
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
        switch(curState)
        {
            case MenuState.MAIN:
                {
                    switch(curSelection)
                    {
                        case 0:
                            {
                                SwitchMenu(MenuState.LEVELSELECT);
                                selectedLevel = 0;
                                UpdateLevelSelect();
                                break;
                            }
                        case 1:
                            {
                                SwitchMenu(MenuState.OPTIONS);
                                break;
                            }
                        case 2:
                            {
                                Application.Quit();
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
            case MenuState.LEVELSELECT:
                {
                    switch (curSelection)
                    {
                        case 0:
                            {
                                break;
                            }
                        case 1:
                            {
                                LoadSelectedLevel();
                                break;
                            }
                        case 2:
                            {
                                SwitchMenu(MenuState.MAIN);
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
        }
    }

    void ChangeLevel(bool selectionIncrement)
    {
        if(selectionIncrement)
        {
            selectedLevel++;
            if(selectedLevel > (levelsUnlocked - 1))
            {
                selectedLevel = 0;
            }
        }
        else
        {
            selectedLevel--;
            if(selectedLevel < 0)
            {
                selectedLevel = levelsUnlocked - 1;
            }
        }
        UpdateLevelSelect();
    }

    void UpdateLevelSelect()
    {
        string tempString = "LEVEL : ";
        for(int i = 0; i < levelsUnlocked; i++)
        {
            if (i == selectedLevel)
            {
                tempString += "<color=#00ff00ff>";
            }
            else
            {
                tempString += "<color=#ffffffff>";
            }
            tempString += levelSelectString[i];
            if(i == levelsUnlocked)
            {
                
            }
            else
            {
                tempString += " ";
            }
            tempString += "</color>";
        }
        LevelSelectMenuText[0].GetComponent<Text>().text = tempString;
    }

    void LoadSelectedLevel()
    {
        switch(selectedLevel)
        {
            case 0:
                {
                    SceneManager.LoadScene("1. T Level");
                    break;
                }
            case 1:
                {
                    SceneManager.LoadScene("2. I Level");
                    break;
                }
            case 2:
                {
                    SceneManager.LoadScene("3. M Level");
                    break;
                }
            case 3:
                {
                    SceneManager.LoadScene("4. E Level");
                    break;
                }
            case 4:
                {
                    SceneManager.LoadScene("5. X Level");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
