using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour {

    public float blinkSpeed;
    public float animTime;
    private float blinkTimer;
    private float animTimer;
    private Image imageDisplay;

	// Use this for initialization
	void Start () {
        blinkTimer = blinkSpeed;
        animTimer = animTime;
        imageDisplay = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(animTimer > 0)
        {
            animTimer -= Time.deltaTime;
            if(blinkTimer > 0)
            {
                blinkTimer -= Time.deltaTime;
            }
            else
            {
                blinkTimer = blinkSpeed;
                imageDisplay.enabled = !imageDisplay.IsActive();
            }
            if(animTimer <= 0)
            {
                imageDisplay.enabled = true;
            }
        }
	}
}
