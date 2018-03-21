using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{

    [SerializeField] private Vector2 m_BounceForce;
    public List<Sprite> animSprites = new List<Sprite>();
    public float resetTime = 1.0f;
    public AudioClip bouncePadClip;
    private float resetTimer = 0.0f;
    private AudioSource audioSrc;
    private SpriteRenderer spriteRenderer;
    public Transform bouncePosTransform;

    // Use this for initialization
    void Start()
    {
        audioSrc = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            if(resetTimer <= 0)
            {
                spriteRenderer.sprite = animSprites[0];
            }
        }
    }

    public Vector2 GetBounceForce()
    {
        Vector2 curBounceForce = Vector2.zero;
        if (resetTimer <= 0)
        {
            resetTimer = resetTime;
            spriteRenderer.sprite = animSprites[1];
            curBounceForce = m_BounceForce;
            audioSrc.PlayOneShot(bouncePadClip);
        }
        return curBounceForce;
    }

    public Vector2 GetBouncePosition()
    {
        return bouncePosTransform.position;
    }
}
