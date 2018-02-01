using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private Transform m_leftWaypoint;
    [SerializeField] private Transform m_rightWaypoint;
    private bool m_movingRight;

    // Use this for initialization
    void Start ()
	{
	    m_movingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
        // move player
		transform.position = new Vector3(transform.position.x + m_Speed * Time.deltaTime, transform.position.y, transform.position.z);
	   
        // if the m_timer exceeds the time, 
	    if (m_movingRight && transform.position.x >= m_rightWaypoint.position.x)
	    {
            // flip sprite
	        Flip();
	    }
	    if (!m_movingRight && transform.position.x <= m_leftWaypoint.position.x)
	    {
	        Flip();
	    }
	}

    private void Flip()
    {
        m_movingRight = !m_movingRight;
        // inverse speed
        m_Speed *= -1;
        // multiply the AI's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void KillEnemy()
    {
        Destroy(gameObject);
    }
}
