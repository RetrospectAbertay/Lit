using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public float HighJumpTime;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_HighJump;
        private float m_JumpTimer = 0.0f;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                if(CrossPlatformInputManager.GetButtonDown("Jump"))
                {                
                    if(m_JumpTimer > 0.0f)
                    {
                        // pressed space again before the timer ran out - make sure we don't jump again later in the update
                        m_HighJump = true;
                        m_Jump = true;
                        m_JumpTimer = -0.1f;
                    }
                    else
                    {
                        m_JumpTimer = HighJumpTime;
                    }
                }
            }
            if(m_JumpTimer > 0.0f)
            {
                // count down the timer - if it reaches 0, perform regular jump
                m_JumpTimer -= Time.deltaTime;
                if(m_JumpTimer <= 0.0f)
                {
                    m_Jump = true;
                }
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool highJump = m_HighJump;
            float h = 0.0f;
            if (CrossPlatformInputManager.GetButton("Right"))
            {
                h = 1.0f;
            }
            if (CrossPlatformInputManager.GetButton("Left"))
            {
                h = -1.0f;
            }
            // Pass all parameters to the character control script.
            m_Character.Move(h, highJump, m_Jump);
            // Reset jump variables 
            m_Jump = false;
            if(highJump)
            {
                Debug.Log("Performed high jump");
                m_HighJump = false;
            }
        }
    }
}
