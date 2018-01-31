using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_PathTime;
    private float m_timer;

    // Use this for initialization
    void Start ()
	{
	    m_timer = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        // move player
		transform.position = new Vector3(transform.position.x + m_Speed * Time.deltaTime, transform.position.y, transform.position.z);
	    m_timer += Time.deltaTime;
        // if the m_timer exceeds the time, 
	    if (m_timer > m_PathTime)
	    {
            // flip sprite
	        Flip();
            // inverse speed
	        m_Speed *= -1;
	        m_timer = 0;
	    }
	}

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("collided with player");
        }
    }
}
