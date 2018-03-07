using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public Transform uiTransform;
    public float moveSpeed;
    public float lerpSpeed;
    public Vector3 finalScale;
    public GameObject particle;
    public GameObject winningSound;
    private float minDistance = 0.1f;
    bool isMoving;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving)
        {
            float step = Time.deltaTime * moveSpeed;
            transform.position = Vector3.MoveTowards(transform.position, uiTransform.transform.position, step);
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, Time.deltaTime);
            if (Vector3.Distance(transform.position, uiTransform.position) < minDistance)
            {
                DeactiveObject();
            }
        }
	}

    public void ToggleMovement()
    {
        isMoving = true;
    }

    public void DeactiveObject()
    {
        Debug.Log("collided with ui object");
        transform.position = uiTransform.position;
        Instantiate(particle, uiTransform.transform);
        Instantiate(winningSound, uiTransform.transform);
        isMoving = false;
    }
}
