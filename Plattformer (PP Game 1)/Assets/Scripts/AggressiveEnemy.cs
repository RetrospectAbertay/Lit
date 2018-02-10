using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : BasicEnemy {

	// Use this for initialization
	void Start () {
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
}
