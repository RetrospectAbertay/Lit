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
        LEVELSELECT,
        LOADING
    }

    private MenuState curState;
    private List<GameObject> CurMenuText = new List<GameObject>();
    public List<GameObject> MainMenuText = new List<GameObject>();
    public List<GameObject> LevelSelect = new List<GameObject>();
    public List<GameObject> LoadingText = new List<GameObject>();
    public List<GameObject> OptionsText = new List<GameObject>();
    public List<Image> SelectionIndicator = new List<Image>();
    public AudioClip UpdateSelectionSound;
    public AudioSource MusicSource;
    public AudioSource SoundSource;
    private int curSelection;
    private int selectedLevel;
    private string levelSelectString = "TIMEX";
    private int levelsUnlocked = 5;
    private int musicVolume = 0;
    private int soundVolume = 0;
    OptionsManager optionsManager;

	// Use this for initialization
	void Start () {
        SwitchMenu(MenuState.MAIN);
        optionsManager = GameObject.FindGameObjectWithTag("Options Manager").GetComponent<OptionsManager>();
        SoundSource = this.GetComponent<AudioSource>();
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
                    CurMenuText = OptionsText;
                    break;
                }
            case MenuState.LEVELSELECT:
                {
                    CurMenuText = LevelSelect;
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
        SoundSource.PlayOneShot(UpdateSelectionSound);
        if (curSelection < 0)
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
            if(i == curSelection && CurMenuText != LoadingText)
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
                                SwitchMenu(MenuState.LOADING);
                                LoadSelectedLevel();
                                break;
                            }
                        case 1:
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
            case MenuState.LOADING:
                {
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
        LevelSelect[0].GetComponent<Text>().text = tempString;
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

    void UpdateVolumeControls(bool updatingMusic, int newVolumeLevel)
    {
        string displayText = "";
        switch(newVolumeLevel)
        {
            case 0:
                {
                    displayText = "OFF";
                    break;
                }
            case 1:
                {
                    displayText = "VERY QUIET";
                    break;
                }
            case 2:
                {
                    displayText = "QUIET";
                    break;
                }
            case 3:
                {
                    displayText = "NORMAL";
                    break;
                }
            case 4:
                {
                    displayText = "LOUD";
                    break;
                }
            case 5:
                {
                    displayText = "VERY LOUD";
                    break;
                }
            default:
                {
                    Debug.Log("volume level out of range!");
                    break;
                }
        }
        if(updatingMusic)
        {
            OptionsText[1].GetComponent<Text>().text = ("MUSIC : " + displayText);
            optionsManager.UpdateVolume((float)(newVolumeLevel * 0.2), true);
            MusicSource.volume = (float)(newVolumeLevel * 0.2);
        }
        else
        {
            OptionsText[1].GetComponent<Text>().text = ("SOUND : " + displayText);
            optionsManager.UpdateVolume((float)(newVolumeLevel * 0.2), false);
            SoundSource.volume = (float)(newVolumeLevel * 0.2);
        }
    }
}
