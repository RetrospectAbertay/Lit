using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public float moveSpeed;
    public Vector3 finalScale;
    public GameObject particle;
    public GameObject winningSound;
    public List<GameObject> previouslyCollectedObj;
    private Transform destinationTransform;
    private float minDistance = 0.1f;
    bool isMoving;
    private float finalLerpAmnt;
    private Vector3 initPos;

	// Use this for initialization
	void Start () {
        initPos = transform.position;
        destinationTransform = GameObject.FindGameObjectWithTag("LetterUI").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving)
        {
            float step = Time.deltaTime * moveSpeed;
            transform.position = Vector3.MoveTowards(transform.position, destinationTransform.transform.position, step);
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, (Vector3.Distance(initPos, destinationTransform.position) / Vector3.Distance(transform.position, destinationTransform.position)) * Time.deltaTime);
            if (Vector3.Distance(transform.position, destinationTransform.position) < minDistance)
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
        Instantiate(particle, destinationTransform.transform);
        Instantiate(winningSound, destinationTransform.transform);
        isMoving = false;
        for(int i = 0; i < previouslyCollectedObj.Count; i++)
        {
            if(previouslyCollectedObj[i].GetComponent<SpriteRenderer>())
            {
                previouslyCollectedObj[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
