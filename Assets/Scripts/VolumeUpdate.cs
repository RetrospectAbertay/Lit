using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeUpdate : MonoBehaviour {

    public bool musicSource;

	// Use this for initialization
	void Start () {
        if (musicSource)
        {
            this.GetComponent<AudioSource>().volume = (float)(0.2 * PlayerPrefs.GetInt("Music Volume"));
        }
        else
        {
            this.GetComponent<AudioSource>().volume = (float)(0.2 * PlayerPrefs.GetInt("Sound Volume"));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
