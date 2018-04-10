using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public float m_LongJumpTime;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_HighJump;
        private float m_JumpTimer = 0.0f;
        private bool m_TogglingMenu = false;
        private bool chargingJump = false;
        private MenuInGame inGameMenu;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            inGameMenu = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<MenuInGame>();
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
                if (CrossPlatformInputManager.GetButtonDown("Charging Jump"))
                {
                    // only jump if player is in menu - otherwise, confirm selection
                    if (inGameMenu.IsInMenu())
                    {
                        inGameMenu.ConfirmMenuSelection();
                    }
                    else
                    {
                        m_JumpTimer = m_LongJumpTime;
                        chargingJump = true;
                        // animator.ChangeAnimation(AnimationPlayer.AnimationState.CHARGING);
                    }
                }
                if (CrossPlatformInputManager.GetButtonUp("Charging Jump"))
                {
                    OnReleasedJumpBtn();
                }
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    if (!chargingJump)
                    {
                        m_Jump = true;
                    }
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
                float h = 0.0f;
                bool charging = false;
                if (m_JumpTimer <= 0.0f)
                {
                    if (!chargingJump)
                    {
                        if (CrossPlatformInputManager.GetButton("Right"))
                        {
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
                m_Character.Move(h, m_HighJump, m_Jump, charging);
                if (m_Jump)
                {
                    // Reset jump variables 
                    m_Jump = false;
                    m_JumpTimer = 0.0f;
                }
                if (m_HighJump)
                {
                    m_HighJump = false;
                }
            }
        }

        void OnReleasedJumpBtn()
        {
            if (m_JumpTimer <= 0)
            {
                m_HighJump = true;
            }
            else
            {

            }
            chargingJump = false;
        }
    }
}