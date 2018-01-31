using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private GameObject player;

	// Use this for initialization
	void Start () {
        // grab the player
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
	}
}
