using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 6f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 800f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private int m_StartingHealth = 3;
        [SerializeField] private float m_InvincibilityDuration = 2;
        [SerializeField] private float m_FlickerDuration = 0.3f;
        [SerializeField] private float m_BounceOnKillForce = 300.0f;
        [SerializeField] private float m_MaxMomentum = 4.0f;
        [SerializeField] private float m_MomentumTime = 4.0f;
        private float m_momentum = 0.0f;

        private Transform m_groundCheck;    // A position marking where to check if the player is grounded.
        const float k_groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_grounded;            // Whether or not the player is grounded.
        private Rigidbody2D m_rigidbody2D;
        private bool m_facingRight = true;  // For determining which way the player is currently facing.
        private float m_beltForce = 0.0f;
        private int m_curHealth;
        private float m_invincibilityTimer;
        private float m_flickerTimer;
        private SpriteRenderer m_spriteRenderer;
        private Vector3 m_platformForce;

        private void Awake()
        {
            // Setting up references.
            m_groundCheck = transform.Find("GroundCheck");
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_curHealth = m_StartingHealth;
            m_invincibilityTimer = 0.0f;
        }


        private void FixedUpdate()
        {
            m_grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, k_groundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_grounded = true;
                    // check if the player is on a conveyer belt
                    if (colliders[i].transform.GetComponent<ConveyourBelt>())
                    {
                        // set force of belt to apply to the player
                        m_beltForce = colliders[i].transform.GetComponent<ConveyourBelt>().GetBeltForce();
                    }
                    // check for moving platform
                    if (colliders[i].transform.GetComponent<PlatformMovement>())
                    {
                        // update platform momentum
                        m_platformForce = colliders[i].transform.GetComponent<PlatformMovement>().getPlatformMovement();
                    }
                }
            }
            // handle flicker animation
            if (m_invincibilityTimer > 0)
            {
                m_flickerTimer -= Time.deltaTime;
                if (m_flickerTimer < 0)
                {
                    m_flickerTimer = m_FlickerDuration;
                    m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
                }
                m_invincibilityTimer -= Time.deltaTime;
                if (m_invincibilityTimer <= 0)
                {
                    m_spriteRenderer.enabled = true;
                    m_flickerTimer = m_FlickerDuration;
                }
            }
            // move player along with the plattform
            transform.position += (m_platformForce * Time.deltaTime);
        }

        public void Move(float move, bool crouch, bool jump)
        {
            //only control the player if grounded or airControl is turned on
            if (m_grounded || m_AirControl)
            {
                // Move the character
                m_rigidbody2D.velocity = new Vector2(move * m_MaxSpeed + m_beltForce + m_momentum, m_rigidbody2D.velocity.y);
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_facingRight)
                {
                    // ... flip the player.
                    Flip();
                    m_momentum = m_MaxMomentum;
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_facingRight)
                {
                    // ... flip the player.
                    Flip();
                    m_momentum = -m_MaxMomentum;
                }
                // determine if the player has stopped moving
                if (move < 0.1 && move > -0.1)
                {
                    // slowly move them back to 0 momentum
                    m_momentum = Mathf.Lerp(m_momentum, 0, m_MomentumTime * Time.deltaTime);
                }
            }
            if(m_grounded == false)
            {
                // reset belt force
                m_beltForce = 0.0f;
                m_platformForce = new Vector3(0, 0, 0);
            }
            // If the player should jump...
            if (m_grounded && jump)
            {
                // Add a vertical force to the player.
                m_grounded = false;
                m_rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_facingRight = !m_facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // don't apply enemy collision when player has taken damage
            if (m_invincibilityTimer <= 0)
            {
                if (other.gameObject.tag == "Enemy")
                {
                    m_curHealth--;
                    m_invincibilityTimer = m_InvincibilityDuration;
                    m_spriteRenderer.enabled = false;
                }
                if (other.gameObject.tag == "Head")
                {
                    Vector3 realGroundCheckPosition = transform.position + m_groundCheck.position;
                    if (other.GetComponentInParent<BasicEnemy>())
                    {
                        if (other.transform.position.y <= realGroundCheckPosition.y)
                        {
                            // kill enemy
                            other.GetComponentInParent<BasicEnemy>().KillEnemy();
                            // add force after killing enemy
                            m_grounded = false;
                            m_rigidbody2D.velocity = Vector2.zero;
                            m_rigidbody2D.AddForce(new Vector2(0f, m_BounceOnKillForce));
                        }
                    }
                }
            }
            if (other.gameObject.GetComponent<BouncePad>())
            {
                // apply force to bounce pad
                m_grounded = false;
                m_rigidbody2D.velocity = Vector2.zero;
                m_rigidbody2D.AddForce(other.gameObject.GetComponent<BouncePad>().GetBounceForce());
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {

        }
    }
}
