using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public float JumpDelay;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_HighJump;
        private float m_JumpTimer = 0.0f;
        private bool m_TogglingMenu = false;
        private bool chargingJump = false;
        private MenuInGame inGameMenu;
        AnimationPlayer animator;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            inGameMenu = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<MenuInGame>();
            animator = GetComponent<AnimationPlayer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_Character.FinishingGame() == false)
                {
                    m_TogglingMenu = true;
                }
            }
            if (!m_Jump || inGameMenu.IsInMenu())
            {
                // Read the jump input in Update so button presses aren't missed.
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    // only jump if player is in menu - otherwise, confirm selection
                    if (inGameMenu.IsInMenu())
                    {
                        inGameMenu.ConfirmMenuSelection();
                    }
                    else
                    {
                        m_JumpTimer = JumpDelay;
                        chargingJump = true;
                        animator.ChangeAnimation(AnimationPlayer.AnimationState.CHARGING);
                    }
                }
                if (CrossPlatformInputManager.GetButtonUp("Jump"))
                {
                    OnReleasedJumpBtn();
                }
                if (m_JumpTimer > 0.0f)
                {
                    // count down the timer - if it reaches 0, perform regular jump
                    m_JumpTimer -= Time.deltaTime;
                }
                if (inGameMenu.wantsToToggle())
                {
                    m_TogglingMenu = true;
                }
            }
        }


        private void FixedUpdate()
        {
            if (m_TogglingMenu)
            {
                m_Character.ToggleFreezeAllObjects();
                inGameMenu.ToggleMenu();
                m_TogglingMenu = false;
                // make sure player doesnt jump because they need to press space to exit menu
                m_Jump = false;
                m_HighJump = false;
            }
            if (!inGameMenu.IsInMenu())
            {
                // Read the inputs.
                bool highJump = m_HighJump;
                float h = 0.0f;
                bool charging = false;
                if (m_JumpTimer <= 0.0f)
                {
                    if (!chargingJump)
                    {
                        if (CrossPlatformInputManager.GetButton("Right"))
                        {
                            Debug.Log("Setting h input");
                            h = 1.0f;
                        }
                        if (CrossPlatformInputManager.GetButton("Left"))
                        {
                            h = -1.0f;
                        }
                    }
                    else
                    {
                        charging = true;
                    }
                }
                // Pass all parameters to the character control script.
                m_Character.Move(h, highJump, m_Jump, charging);
                if (m_Jump)
                {
                    // Reset jump variables 
                    m_Jump = false;
                    m_JumpTimer = 0.0f;
                }
                if (highJump)
                {
                    Debug.Log("Performed high jump");
                    m_HighJump = false;
                }
            }
        }

        void OnReleasedJumpBtn()
        {
            if (m_JumpTimer <= 0)
            {
                Debug.Log("high jump");
                m_HighJump = true;
            }
            else
            {
                Debug.Log("normal jump");
            }
            m_Jump = true;
            chargingJump = false;
        }
    }
}
