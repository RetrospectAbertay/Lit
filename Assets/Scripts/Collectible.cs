using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public GameObject uiTransform;
    public float moveSpeed;
    public float minDistance;
    public GameObject particle;
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
            if(Vector3.Distance(transform.position, uiTransform.transform.position) < minDistance)
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
        uiTransform.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.SetActive(false);
        Instantiate(particle, uiTransform.transform);
    }
}
