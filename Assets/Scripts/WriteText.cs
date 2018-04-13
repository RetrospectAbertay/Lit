using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteText : MonoBehaviour
{
    public List<string> Paragraphs;
    public float maxWriteSpeed;
    public float minWriteSpeed;
    public float TimeBetweenParagraphs = 2.0f;
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

    // Use this for initialization
    void Start()
    {
        // get the text object iself
        textObject = this.GetComponent<Text>();
        textObject.text = "";
        // get the contents of the string
        stringToDisplay = Paragraphs[0];
        // setup timer to init to 0
        timer = 0.0f;
        audioSource = GetComponent<AudioSource>();
        // clamp frame rate for that spectrum feel
        Application.targetFrameRate = 60;
        curWriteSpeed = Random.Range(minWriteSpeed, maxWriteSpeed);
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKeyDown("space"))
        {
            // Complete text if still writing
            if(!finishedWriting)
            {
                textObject.text = Paragraphs[curParagraph];
                finishedWriting = true;
            }
            else
            {
                if (curParagraph < Paragraphs.Count)
                {
                    // increment paragraph to display
                    curParagraph++;
                    // set string to display
                    stringToDisplay = Paragraphs[curParagraph];
                    // changing paragraph - need to wait for a short period before switching
                    paraTimer = TimeBetweenParagraphs;
                    // continue writing
                    finishedWriting = false;
                    // reset text
                    curChar = 0;
                    textObject.text = "";
                }
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
