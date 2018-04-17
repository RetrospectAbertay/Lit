using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteText : MonoBehaviour
{
    public List<string> Paragraphs;
    public float maxWriteSpeed;
    public float minWriteSpeed;
    public AudioClip TypingSound;
    private AudioSource audioSource;
    Text textObject;
    string stringToDisplay;
    float paraTimer;
    float timer;
    float curWriteSpeed;
    int curChar = 0;
    int curParagraph = 0;
    bool finishedWriting = false;
    TransitionScript transitScript;

    // Use this for initialization
    void Start()
    {
        // get the text object iself
        textObject = this.GetComponent<Text>();
        textObject.text = "";
        // setup timer to init to 0
        timer = 0.0f;
        audioSource = GetComponent<AudioSource>();
        // clamp frame rate for that spectrum feel
        Application.targetFrameRate = 60;
        curWriteSpeed = Random.Range(minWriteSpeed, maxWriteSpeed);
        transitScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TransitionScript>();
        // Check if we need to write
        if (Paragraphs.Count > 0)
        {
            // get the contents of the string
            stringToDisplay = Paragraphs[0];
        }
        else
        {
            finishedWriting = true;
            stringToDisplay = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for space input
        if (Input.GetKeyDown("space"))
        {
            // Complete text if still writing
            if (!finishedWriting)
            {
                textObject.text = Paragraphs[curParagraph];
                finishedWriting = true;
            }
            else
            {
                // Check if another paragraph needs to be written
                if (curParagraph < (Paragraphs.Count - 1))
                {
                    // increment paragraph to display
                    curParagraph++;
                    if (curParagraph < (Paragraphs.Count))
                    {
                        // continue writing
                        finishedWriting = false;
                        // set string to display
                        stringToDisplay = Paragraphs[curParagraph];
                        // reset text
                        curChar = 0;
                        textObject.text = "";
                    }
                }
                else
                {
                    // If not transition to the next level
                    transitScript.TransitionToNextLvl();
                }
            }
        }
        // check if we still need to write
        if (!finishedWriting)
        {
            // increment timer
            timer += Time.deltaTime;
            if (timer > curWriteSpeed)
            {
                // display a new character
                updateText();
                // increment cur char
                curChar++;
                // reset timer
                timer = 0.0f;
                // randomise the speed at which the next letter should be written
                curWriteSpeed = Random.Range(minWriteSpeed, maxWriteSpeed);
            }
        }
    }

    void updateText()
    {
        // check if we need to switch paragraphs
        if (curChar >= stringToDisplay.Length)
        {
            // stop writing
            finishedWriting = true;
        }
        else
        {
            // update text
            textObject.text += stringToDisplay[curChar];
            // play audio
            audioSource.PlayOneShot(TypingSound);
        }
    }
}
