using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour {

    enum MenuState
    {
        MAIN,
        OPTIONS,
        LEVELSELECT
    }

    private List<GameObject> CurMenuText = new List<GameObject>();
    public List<GameObject> MainMenuText;
    public List<GameObject> LevelSelectMenuText;
    public List<Image> SelectionIndicator;
    private int curSelection;
    private MenuState curState;

	// Use this for initialization
	void Start () {
        SwitchMenu(MenuState.MAIN);
	}
	
	// Update is called once per frame
	void Update () {
		
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
        // setup selection
        curSelection = 0;
        // apply new selection
        UpdateMenuSelection();
    }

    void UpdateMenuSelection()
    {
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
}
