using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    private Transform m_leftWaypoint;
    private Transform m_rightWaypoint;
    private Transform m_enemyBody;
    private bool m_movingRight;

    // Use this for initialization
    void Start ()
	{
	    m_movingRight = true;
	    m_leftWaypoint = transform.Find("LeftWaypoint");
	    m_rightWaypoint = transform.Find("RightWaypoint");
	    m_enemyBody = transform.Find("EnemyBody");

	}
	
	// Update is called once per frame
	void Update () {
        // move player
		m_enemyBody.position = new Vector3(m_enemyBody.position.x + m_Speed * Time.deltaTime, m_enemyBody.position.y, m_enemyBody.position.z);
	   
        // if the m_timer exceeds the time, 
	    if (m_movingRight && m_enemyBody.position.x >= m_rightWaypoint.position.x)
	    {
            // flip sprite
	        Flip();
	    }
	    if (!m_movingRight && m_enemyBody.position.x <= m_leftWaypoint.position.x)
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
        Vector3 theScale = m_enemyBody.localScale;
        theScale.x *= -1;
        m_enemyBody.localScale = theScale;
    }

    public void KillEnemy()
    {
        Destroy(gameObject);
    }
}
