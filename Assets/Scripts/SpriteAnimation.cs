using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{

    public List<Sprite> AnimationSprites;
    public float FrameTime;
    public bool shouldExpire = false;

    private SpriteRenderer m_Renderer;
    private int curFrame = 0;
    private float Timer = 0.0f;

    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        curFrame = 0;
        m_Renderer.sprite = AnimationSprites[curFrame];
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= FrameTime)
        {
            curFrame++;
            if (curFrame == AnimationSprites.Count)
            {
                curFrame = 0;
                if (shouldExpire)
                {
                    m_Renderer.enabled = false;
                }
            }
            m_Renderer.sprite = AnimationSprites[curFrame];
            Timer = 0;
        }
    }

    public void ActivateAnimator()
    {
        m_Renderer.sprite = AnimationSprites[0];
        curFrame = 0;
        m_Renderer.enabled = true;
    }
}