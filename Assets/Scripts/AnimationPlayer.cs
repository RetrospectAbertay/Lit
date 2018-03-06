﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public enum AnimationState
    {
        IDLE,
        WALKING,
        JUMPING
    }
    public Sprite[] IdleAnimation;
    public Sprite[] WalkingAnimation;
    public Sprite[] JumpingAnimation;
    private Sprite[] curAnimation;
    public float FrameDuration;
    private int frameCounter = 0;
    private float timer = 0.0f;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        // get sprite renderer component
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        curAnimation = IdleAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        // increment timer
        timer += Time.deltaTime;
        // if the timer exceeds the predefined frame time, we want to play the next frame
        if (timer > FrameDuration)
        {
            if (frameCounter <= curAnimation.Length)
            {
                // allocate the sprite renderer to display the frame correllating to the frame counter
                spriteRenderer.sprite = curAnimation[frameCounter];
            }
            // increment frame counter
            frameCounter++;
            // check that our frame counter has not exceeded the maximum number of frames in our animation
            if (frameCounter >= curAnimation.Length)
            {
                // otherwise we want to reset our frame counter, which will display the first frame
                frameCounter = 0;
            }
            // reset the timer for the next update
            timer = 0.0f;
        }
    }

    public void ChangeAnimation(AnimationState newAnimation)
    {
        //// switch statement checks for each possible animation state and allocates the right one
        switch (newAnimation)
        {
            case AnimationState.IDLE:
                curAnimation = IdleAnimation;
                frameCounter = 0;
                break;
            case AnimationState.JUMPING:
                curAnimation = JumpingAnimation;
                frameCounter = 0;
                break;
            case AnimationState.WALKING:
                curAnimation = WalkingAnimation;
                break;
            default:
                break;
        }
    }

    public bool RunningAirFrame()
    {
        if (curAnimation == JumpingAnimation)
        {
            return true;
        }
        return false;
    }
}