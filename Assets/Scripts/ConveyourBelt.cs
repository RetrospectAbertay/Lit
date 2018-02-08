using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyourBelt : MonoBehaviour
{

    [SerializeField] private float m_Force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetBeltForce()
    {
        return m_Force;
    }
}
