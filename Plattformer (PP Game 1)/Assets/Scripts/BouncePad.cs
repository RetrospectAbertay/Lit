using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour {

    [SerializeField] private Vector2 m_BounceForce;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 GetBounceForce()
    {
        return m_BounceForce;
    }
}
