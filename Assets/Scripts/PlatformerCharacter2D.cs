using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float MaxSpeed = 6f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float JumpForce = 800f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool AirControl = true;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private int StartingHealth = 3;
        [SerializeField] private float InvincibilityDuration = 2;
        [SerializeField] private float FlickerDuration = 0.3f;
        [SerializeField] private float BounceOnKillForce = 300.0f;
        [SerializeField] private float DefaultGravScale = 5.0f;
        [SerializeField] private float FinalCollectTime = 2.0f;
        [SerializeField] private float AnimTime = 1.0f;
        [SerializeField] private float TimeBetweenFootsteps;
        [SerializeField] private AudioClip JumpAudio;
        [SerializeField] private AudioClip WalkingAudio;
        [SerializeField] private AudioClip CollectionAudio;

        private Transform groundCheck;    // A position marking where to check if the player is grounded.
        const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded;            // Whether or not the player is grounded.
        private Rigidbody2D rigidbody2D;
        private bool facingRight = true;  // For determining which way the player is currently facing.
        private float beltForce = 0.0f;
        private int curHealth;
        private float invincibilityTimer;
        private float flickerTimer;
        private SpriteRenderer spriteRenderer;
        private Vector3 platformForce;
        private float dropTimer = 0.0f;
        private float animResetTimer;
        private float footstepsTimer;
        private AudioSource audioSource;
        AnimationPlayer animator;

        // Timex UI elements
        GameObject[] timexLetters;
        int unlockedLetters = 0;
        bool letterCollected = false;

        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            curHealth = StartingHealth;
            invincibilityTimer = 0.0f;
            rigidbody2D.gravityScale = DefaultGravScale;
            animator = GetComponent<AnimationPlayer>();
            timexLetters = GameObject.FindGameObjectsWithTag("LetterUI").OrderBy(go => go.name).ToArray();
            audioSource = GetComponent<AudioSource>();
            footstepsTimer = 0.0f;
            // Determine the level that the player is in
            Scene curScene;
            curScene = SceneManager.GetActiveScene();
            switch (curScene.name)
            {
                case "1. T Level":
                    {

                    }
                    break;
                case "2. I Level":
                    {
                        unlockedLetters = 1;
                    }
                    break;
                case "3. M Level":
                    {
                        unlockedLetters = 2;
                    }
                    break;
                case "4. E Level":
                    {
                        unlockedLetters = 3;
                    }
                    break;
                case "5. X Level":
                    {
                        unlockedLetters = 4;
                    }
                    break;
            }

            for (int i = 0; i < timexLetters.Length; i++)
            {
                if (i < unlockedLetters)
                {
                    timexLetters[i].SetActive(true);
                }
                else
                {
                    timexLetters[i].SetActive(false);
                }
            }
        }

        private void FixedUpdate()
        {
            // check if letter has been collected
            if (letterCollected == false)
            {
                grounded = false;

                // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
                // This can be done using layers instead but Sample Assets will not overwrite your project settings.
                Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, WhatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        grounded = true;
                        // check if the player is on a conveyer belt
                        if (colliders[i].transform.GetComponent<ConveyourBelt>())
                        {
                            // set force of belt to apply to the player
                            beltForce = colliders[i].transform.GetComponent<ConveyourBelt>().GetBeltForce();
                        }
                        // check for moving platform
                        if (colliders[i].transform.GetComponent<PlatformMovement>())
                        {
                            // update platform momentum
                            platformForce = colliders[i].transform.GetComponent<PlatformMovement>().getPlatformMovement();
                        }
                    }
                }
                if (animResetTimer > 0.0f)
                {
                    animResetTimer -= Time.deltaTime;
                    if (animResetTimer <= 0.0f)
                    {
                        animator.ChangeAnimation(AnimationPlayer.AnimationState.IDLE);
                    }
                }
                // handle flicker animation
                if (invincibilityTimer > 0)
                {
                    flickerTimer -= Time.deltaTime;
                    if (flickerTimer < 0)
                    {
                        flickerTimer = FlickerDuration;
                        spriteRenderer.enabled = !spriteRenderer.enabled;
                    }
                    invincibilityTimer -= Time.deltaTime;
                    if (invincibilityTimer <= 0)
                    {
                        spriteRenderer.enabled = true;
                        flickerTimer = FlickerDuration;
                    }
                }
                // axisInput player along with the plattform
                transform.position += (platformForce * Time.deltaTime);
            }
            else
            {
                // decrement timer
                FinalCollectTime -= Time.deltaTime;
                // flicker player
                flickerTimer -= Time.deltaTime;
                if (flickerTimer < 0)
                {
                    flickerTimer = FlickerDuration;
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                }
                if (FinalCollectTime <= 0)
                {
                    switch (unlockedLetters)
                    {
                        case 0:
                            SceneManager.LoadScene("1.1 T Level");
                            break;
                        case 1:
                            SceneManager.LoadScene("2.1 I Level");
                            break;
                        case 2:
                            SceneManager.LoadScene("3.1 M Level");
                            break;
                        case 3:
                            SceneManager.LoadScene("4.1 E Level");
                            break;
                        case 4:
                            SceneManager.LoadScene("5.1 X Level");
                            break;
                        case 5:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void Move(float axisInput, bool crouch, bool jump)
        {
            if (!letterCollected)
            {
                //only control the player if grounded or airControl is turned on
                if (grounded || AirControl)
                {
                    float appliedForce = 0.0f;
                    // If the input is moving the player right and the player is facing left...
                    if (axisInput > 0.01)
                    {
                        appliedForce = MaxSpeed;
                        if (!facingRight)
                        {
                            // ... flip the player.
                            Flip();
                        }
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (axisInput < -0.01)
                    {
                        appliedForce = -MaxSpeed;
                        if (facingRight)
                        {
                            // ... flip the player.
                            Flip();
                        }
                    }
                    // Move the character
                    rigidbody2D.velocity = new Vector2(appliedForce + beltForce, rigidbody2D.velocity.y);
                }
                if (grounded == false)
                {
                    // reset belt force
                    beltForce = 0.0f;
                    platformForce = new Vector3(0, 0, 0);
                    if (!animator.RunningAirFrame())
                    {
                        animator.ChangeAnimation(AnimationPlayer.AnimationState.JUMPING);
                    }
                }
                else
                {
                    if (animator.RunningAirFrame())
                    {
                        animator.ChangeAnimation(AnimationPlayer.AnimationState.IDLE);
                    }
                    if (Mathf.Abs(axisInput) > 0.0f)
                    {
                        // Character is walking
                        animator.ChangeAnimation(AnimationPlayer.AnimationState.WALKING);
                        animResetTimer = AnimTime;
                        // Increment footsteps timer to see if a sound should be played
                        footstepsTimer += Time.deltaTime;
                        if (footstepsTimer > TimeBetweenFootsteps)
                        {
                            audioSource.PlayOneShot(WalkingAudio);
                            footstepsTimer = 0.0f;
                        }
                    }
                }
                // If the player should jump...
                if (grounded && jump)
                {
                    // Add a vertical force to the player.
                    grounded = false;
                    rigidbody2D.AddForce(new Vector2(0f, JumpForce));
                    Debug.Log("Attempted to jump!");
                    audioSource.PlayOneShot(JumpAudio);
                }
            }
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // don't apply enemy collision when player has taken damage
            if (invincibilityTimer <= 0)
            {
                if (other.gameObject.tag == "Enemy")
                {
                    curHealth--;
                    invincibilityTimer = InvincibilityDuration;
                    spriteRenderer.enabled = false;
                }
                if (other.gameObject.tag == "Head")
                {
                    Vector3 realGroundCheckPosition = transform.position + groundCheck.position;
                    if (other.GetComponentInParent<BasicEnemy>())
                    {
                        if (other.transform.position.y <= realGroundCheckPosition.y)
                        {
                            // kill enemy
                            other.GetComponentInParent<BasicEnemy>().KillEnemy();
                            // add force after killing enemy
                            grounded = false;
                            rigidbody2D.velocity = Vector2.zero;
                            rigidbody2D.AddForce(new Vector2(0f, BounceOnKillForce));
                        }
                    }
                }
            }
            //if (other.gameObject.GetComponent<BouncePad>())
            //{
            //    // apply force to bounce pad
            //    m_grounded = false;
            //    m_rigidbody2D.velocity = Vector2.zero;
            //    m_rigidbody2D.gravityScale = m_DefaultGravScale;
            //    m_rigidbody2D.AddForce(other.gameObject.GetComponent<BouncePad>().GetBounceForce());
            //}
            if (other.gameObject.tag == "Collectible")
            {
                // collect letter
                timexLetters[(unlockedLetters)].SetActive(true);
                // other.gameObject.SetActive(false);
                letterCollected = true;
                // freeze rigid body
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                FreezeOtherObjects();
                animator.ChangeAnimation(AnimationPlayer.AnimationState.IDLE);
                if (other.GetComponent<Collectible>())
                {
                    other.GetComponent<Collectible>().ToggleMovement();
                }
                audioSource.PlayOneShot(CollectionAudio);
            }
        }

        void FreezeOtherObjects()
        {
            GameObject[] movingObjects = GameObject.FindGameObjectsWithTag("MovingObject");
            // iterate through all moving objects
            for (int i = 0; i < movingObjects.Length; i++)
            {
                // freeze moving platforms
                if (movingObjects[i].GetComponent<PlatformMovement>())
                {
                    movingObjects[i].GetComponent<PlatformMovement>().FreezePlatform();
                }
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {

        }
    }
}
