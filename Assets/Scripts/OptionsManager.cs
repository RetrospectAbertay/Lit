using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {

    float musicVolume = 0;
    float soundVolume = 0;
    int lvlsUnlocked = 0;

	// Use this for initialization
	void Start () {
        // dont destroy options manager when switching between scenes
        DontDestroyOnLoad(this);
        // load saved settings
        soundVolume = PlayerPrefs.GetFloat("Sound Volume");
        musicVolume = PlayerPrefs.GetFloat("Music Volume");
        lvlsUnlocked = PlayerPrefs.GetInt("Levels Unlocked");
        Debug.Log(lvlsUnlocked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateVolume(float newVolume, bool music)
    {
        if(music)
        {
            musicVolume = newVolume;
            PlayerPrefs.SetFloat("Music Volume", musicVolume);
        }
        else
        {
            soundVolume = newVolume;
            PlayerPrefs.SetFloat("Sound Volume", soundVolume);
        }
    }
    public void UpdateLevelsUnlocked()
    {
        lvlsUnlocked++;
        PlayerPrefs.GetInt("Levels Unlocked");
    }    

    public int GetLevelsUnlocked()
    {
        return lvlsUnlocked;
    }

    public float GetVolume(bool music)
    {
        if(music)
        {
            return musicVolume;
        }
        else
        {
            return soundVolume;
        }
    }
}
