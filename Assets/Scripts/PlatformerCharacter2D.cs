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
        [SerializeField] private float JumpUpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private float JumpFwdForce = 200f;
        [SerializeField] private float HighJumpUpForce = 600f;
        [SerializeField] private float HighJumpFwdForce = 100f;
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
        [SerializeField] private float SwitchRotationDelay = 0.2f;
        [SerializeField] private AudioClip JumpAudio;
        [SerializeField] private AudioClip WalkingAudio;
        [SerializeField] private AudioClip CollectionAudio;
        [SerializeField] private bool EndlessPlayer = false;
        [SerializeField] private float ConstantJumpForce = 1.0f;

        private Transform respawnPosition;
        private Transform groundCheck;    // A position marking where to check if the player is grounded.
        const float groundedRadius = .1f; // Radius of the overlap circle to determine if grounded
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
        private float movTimer;
        private bool inMenu;
        private GameObject mainCam;
        private AudioSource audioSource;
        private float jumpTimer = 0.3f;
        private float switchDelayTimer = 0.2f;
        AnimationPlayer animator;
        int unlockedLetters = 0;
        bool letterCollected = false;
        bool frozen = false;
        private bool requireLoadScreen = false;
        private bool jumping = false;
        private Vector2 tempVelocity;

        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            curHealth = StartingHealth;
            invincibilityTimer = 0.0f;
            // rigidbody2D.gravityScale = DefaultGravScale;
            animator = GetComponent<AnimationPlayer>();
            audioSource = GetComponent<AudioSource>();
            footstepsTimer = 0.0f;
            mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            // Determine the level that the player is in
            Scene curScene;
            curScene = SceneManager.GetActiveScene();
            if(!EndlessPlayer)
            {
                respawnPosition = GameObject.FindGameObjectWithTag("Respawn").transform;
            }
            switch (curScene.name)
            {
                case "1. T Level":
                    {
                        unlockedLetters = 0;
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
            // Set up continuous collision
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            jumpTimer = 0.0f;
        }

        private void Update()
        {
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
                bool onPlatform = false;
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        if(AirControl)
                        {
                            AirControl = false;
                        }
                        grounded = true;
                        jumping = false;
                        //rigidbody2D.velocity = Vector2.zero;
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
                            onPlatform = true;
                        }
                    }
                }
                if(!onPlatform)
                {
                    platformForce = new Vector2(0, 0);
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
                // timer that checks if player is in air and jumped recently
                if(jumpTimer > 0.0f)
                {
                    jumpTimer -= Time.deltaTime;
                    jumping = true;
                }
                else
                {
                    jumping = false;
                }
                // axisInput player along with the plattform
                transform.position += (platformForce * Time.deltaTime);
                // check if player is jumping, apply x-axis force
                if(jumping)
                {
                    if (facingRight)
                    {
                        rigidbody2D.AddForce(new Vector2(ConstantJumpForce, 0));
                    }
                    else
                    {
                        rigidbody2D.AddForce(new Vector2(-ConstantJumpForce, 0));
                    }
                }
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
                    GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<MenuInGame>().ToggleLoadScreen();
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
            // update timer that checks the last time player input was recieved
            if(movTimer > 0)
            {
                movTimer -= Time.deltaTime;
            }
            // Run down timerto see if player can start moving again after switching directions
            if(switchDelayTimer > 0)
            {
                switchDelayTimer -= Time.deltaTime;
            }
        }

        public void Move(float axisInput, bool highJump, bool jump, bool charging)
        {
            if (!letterCollected)
            {
                // this timer is used to make sure that axis input doesnt effect the jump
                if (jumpTimer <= 0.0f)
                {
                    //only control the player if grounded or airControl is turned on
                    if (grounded)
                    {
                        if (jump == false)
                        {
                            // Setup animation if its charging
                            if (charging)
                            {
                                animator.ChangeAnimation(AnimationPlayer.AnimationState.CHARGING);
                                animResetTimer = AnimTime;
                            }
                            float appliedForce = 0.0f;
                            // If the input is moving the player right and the player is facing left...
                            if (axisInput > 0.01)
                            {
                                appliedForce = MaxSpeed;
                                if (!facingRight)
                                {
                                    // ... flip the player.
                                    Flip();
                                    switchDelayTimer = SwitchRotationDelay;
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
                                    switchDelayTimer = SwitchRotationDelay;
                                }
                            }
                            // Reset velocity if player switche direction recently
                            if (switchDelayTimer > 0)
                            {
                                appliedForce = 0.0f;
                            }
                            // Move the character
                            rigidbody2D.velocity = new Vector2(appliedForce + beltForce, rigidbody2D.velocity.y);
                            //Debug.Log("moving forward");
                            // check for absolute input to trigger animations and sounds for walking
                            if (Mathf.Abs(axisInput) > 0.0f)
                            {
                                // Character is walking
                                animator.ChangeAnimation(AnimationPlayer.AnimationState.WALKING);
                                animResetTimer = AnimTime;
                                // check for when the last time was, that player input was recieved - if it was not recenlty, play footstep sound immediately
                                if (movTimer <= 0)
                                {
                                    movTimer = 0.3f;
                                    footstepsTimer = TimeBetweenFootsteps;
                                }
                                // Increment footsteps timer to see if a sound should be played
                                footstepsTimer += Time.deltaTime;
                                if (footstepsTimer >= TimeBetweenFootsteps)
                                {
                                    audioSource.PlayOneShot(WalkingAudio);
                                    footstepsTimer = 0.0f;
                                }
                            }
                            else
                            {
                                if (!charging)
                                {
                                    animator.ChangeAnimation(AnimationPlayer.AnimationState.IDLE);
                                }
                            }
                        }
                        else
                        {
                            // Player is trying to jump
                            animator.ChangeAnimation(AnimationPlayer.AnimationState.JUMPING);
                            // Add a vertical force to the player.
                            grounded = false;
                            float finalFwdForce = JumpFwdForce;
                            float finalUpForce = JumpUpForce;
                            if (highJump)
                            {
                                finalFwdForce = HighJumpFwdForce;
                                finalUpForce = HighJumpUpForce;
                            }
                            if (!facingRight)
                            {
                                finalFwdForce *= -1;
                            }
                            // reset velocity
                            rigidbody2D.velocity = new Vector2(0, 0);
                            // add jump force
                            rigidbody2D.AddForce(new Vector2(finalFwdForce, finalUpForce));
                            audioSource.PlayOneShot(JumpAudio);
                            jumping = true;
                            jumpTimer = 0.3f;
                        }
                    }
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
                            rigidbody2D.velocity = new Vector2(0, 0);
                            rigidbody2D.AddForce(new Vector2(0f, BounceOnKillForce));
                        }
                    }
                }
            }
            if (other.gameObject.GetComponent<BouncePad>())
            {
                // apply force to bounce pad
                grounded = false;
                rigidbody2D.velocity = new Vector2(0, 0);
                rigidbody2D.AddForce(other.gameObject.GetComponent<BouncePad>().GetBounceForce());
                jumpTimer = 0.3f;
                transform.position = other.gameObject.GetComponent<BouncePad>().GetBouncePosition();
            }
            if (other.gameObject.tag == "Collectible")
            {
                if (letterCollected == false)
                {
                    // other.gameObject.SetActive(false);
                    letterCollected = true;
                    int curSavedLettersUnlocked = PlayerPrefs.GetInt("Levels Unlocked");
                    if((unlockedLetters + 1) > curSavedLettersUnlocked)
                    {
                        curSavedLettersUnlocked++;
                        PlayerPrefs.SetInt("Levels Unlocked", curSavedLettersUnlocked);
                        //Debug.Log(PlayerPrefs.GetInt("Levels Unlocked"));
                        //Debug.Log(curSavedLettersUnlocked);
                    }
                    animator.ChangeAnimation(AnimationPlayer.AnimationState.IDLE);
                    // freeze rigid body
                    ToggleFreezeAllObjects();
                    mainCam.GetComponent<AudioSource>().Stop();
                    if (other.GetComponent<Collectible>())
                    {
                        other.GetComponent<Collectible>().ToggleMovement();
                    }
                    audioSource.PlayOneShot(CollectionAudio);
                }
            }

            if(other.gameObject.tag == "Death Collider")
            {
                if(EndlessPlayer == false)
                {
                    //Debug.Log("player fell to their death!");
                    transform.position = respawnPosition.position;
                    invincibilityTimer = InvincibilityDuration;
                }
            }
        }

        public void ToggleFreezeAllObjects()
        {
            frozen = !frozen;
            if(frozen)
            {
                //Debug.Log("constraining position and rotation");
                tempVelocity = rigidbody2D.velocity;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                rigidbody2D.constraints = RigidbodyConstraints2D.None;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.rotation = Quaternion.identity;
                rigidbody2D.velocity = tempVelocity;
            }
            GameObject[] movingObjects = GameObject.FindGameObjectsWithTag("MovingObject");
            // iterate through all moving objects
            for (int i = 0; i < movingObjects.Length; i++)
            {
                // freeze moving platforms
                if (movingObjects[i].GetComponent<PlatformMovement>())
                {
                    movingObjects[i].GetComponent<PlatformMovement>().TogglePlatformFreeze();
                }
            }
        }

        public bool FinishingGame()
        {
            return letterCollected;
        }

        void OnCollisionEnter2D(Collision2D other)
        {

        }
    }
}
