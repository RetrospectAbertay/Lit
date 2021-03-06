﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] private Vector3 m_MovementVector;
    [SerializeField] private float m_MovementDuration = 2.0f;
    private float m_movementTimer;
    private bool platformFrozen = false;

    // Use this for initialization
    void Start()
    {
        m_movementTimer = 0.0f;
        platformFrozen = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!platformFrozen)
        {
            m_movementTimer += Time.deltaTime;
            transform.position += (m_MovementVector * Time.deltaTime);
            if (m_movementTimer > m_MovementDuration)
            {
                m_MovementVector *= -1;
                m_movementTimer = 0;
            }
        }
    }

    public void TogglePlatformFreeze()
    {
        platformFrozen = !platformFrozen;
    }

    public Vector3 getPlatformMovement()
    {
        return m_MovementVector;
    }
}
