using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    [SerializeField] private Vector3 m_MovementVector;
    [SerializeField] private float m_MovementDuration = 2.0f;
    private float m_movementTimer;

    // Use this for initialization
    void Start ()
    {
        m_movementTimer = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
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
