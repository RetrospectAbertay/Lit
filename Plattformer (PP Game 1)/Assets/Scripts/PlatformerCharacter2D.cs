using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private float m_ConveyerForce = 20.0f;
        [SerializeField] private int m_StartingHealth = 3;
        [SerializeField] private float m_InvincibilityDuration = 2;
        [SerializeField] private float m_FlickerDuration = 2;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private float m_beltForce = 0.0f;
        private int m_curHealth;
        private float m_invincibilityTimer;
        private float m_flickerTimer;
        private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_curHealth = m_StartingHealth;
            m_invincibilityTimer = 0.0f;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    // check if the player is on a conveyer belt
                    if (colliders[i].transform.GetComponent<ConveyerBelt>())
                    {
                        // set force of belt to apply to the player
                        m_beltForce = colliders[i].transform.GetComponent<ConveyerBelt>().GetBeltForce();
                    }
                }
            }
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
        }


        public void Move(float move, bool crouch, bool jump)
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed + m_beltForce, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            if(m_Grounded == false)
            {
                // reset belt force
                m_beltForce = 0.0f;
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                m_curHealth--;
                m_invincibilityTimer = m_InvincibilityDuration;
                m_spriteRenderer.enabled = false;
            }
            if (other.gameObject.tag == "Head")
            {
                Vector3 realGroundCheckPosition = transform.position + m_GroundCheck.position;
                if (other.GetComponentInParent<BasicEnemy>())
                {
                    if (other.transform.position.y <= realGroundCheckPosition.y)
                    {
                        other.GetComponentInParent<BasicEnemy>().KillEnemy();
                    }
                }
            }


            // PICK UP LETTERS
            switch (other.gameObject.tag)
            {
                case "T" :
                    other.gameObject.SetActive(false);
                    break;
                case "I":
                    other.gameObject.SetActive(false);
                    break;
                case "M":
                    other.gameObject.SetActive(false);
                    break;
                case "E":
                    other.gameObject.SetActive(false);
                    break;
                case "X":
                    other.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
